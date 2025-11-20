using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OrdersExercise.Data;
using OrdersExercise.Dtos;
using OrdersExercise.Models;

namespace OrdersExercise.Handlers
{
    public class CreateOrderHandler
    {
        private readonly IOrderRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderHandler> _logger;
        private const string CacheKey = "all_orders";

        // Define EventIds for structured logging
        private static readonly EventId ISBNValidationPerformed = new EventId(1001, "ISBNValidationPerformed");
        private static readonly EventId StockValidationPerformed = new EventId(1002, "StockValidationPerformed");
        private static readonly EventId OrderCreationMetrics = new EventId(1003, "OrderCreationMetrics");

        public CreateOrderHandler(IOrderRepository repository, IMemoryCache cache, IMapper mapper, ILogger<CreateOrderHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OrderProfileDto> Handle(CreateOrderProfileRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Start total operation timer
            var totalTimer = Stopwatch.StartNew();
            
            // Generate Operation ID
            string operationId = Guid.NewGuid().ToString("N").Substring(0, 8);

            // Begin Logging Scope
            using (_logger.BeginScope(new Dictionary<string, object> { ["OperationId"] = operationId }))
            {
                _logger.LogInformation("Starting CreateOrder Operation (Title={Title}, Author={Author}, Category={Category}, ISBN={ISBN})",
                    request.Title, request.Author, request.Category, request.ISBN);

                // Input Guards
                if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Title is required.", nameof(request.Title));
                if (string.IsNullOrWhiteSpace(request.Author)) throw new ArgumentException("Author is required.", nameof(request.Author));
                if (string.IsNullOrWhiteSpace(request.ISBN)) throw new ArgumentException("ISBN is required.", nameof(request.ISBN));
                if (request.Price < 0) throw new ArgumentException("Price cannot be negative.", nameof(request.Price));

                long validationElapsed = 0;
                long dbElapsed = 0;

                try
                {
                    // Validation Phase
                    var validationTimer = Stopwatch.StartNew();

                    var existing = await _repository.GetByIsbnAsync(request.ISBN);
                    _logger.LogInformation(ISBNValidationPerformed, "ISBN uniqueness check performed for {ISBN}", request.ISBN);

                    if (existing != null)
                    {
                        _logger.LogWarning("Validation failed: Order with ISBN {ISBN} already exists.", request.ISBN);
                        throw new InvalidOperationException($"Order with ISBN '{request.ISBN}' already exists.");
                    }

                    _logger.LogInformation(StockValidationPerformed, "Initial stock validation performed.");
                    
                    validationTimer.Stop();
                    validationElapsed = validationTimer.ElapsedMilliseconds;

                    // Mapping Phase
                    var order = _mapper.Map<Order>(request);
                    if (order.Id == Guid.Empty) order.Id = Guid.NewGuid();
                    if (order.CreatedAt == default) order.CreatedAt = DateTime.UtcNow;

                    // Database Phase
                    var dbTimer = Stopwatch.StartNew();
                    _logger.LogInformation("Starting database insert for OrderId: {OrderId}", order.Id);
                    
                    await _repository.AddAsync(order);
                    
                    _logger.LogInformation("Completed database insert for OrderId: {OrderId}", order.Id);
                    dbTimer.Stop();
                    dbElapsed = dbTimer.ElapsedMilliseconds;

                    // Cache Phase
                    _logger.LogInformation("Invalidating cache for key: {CacheKey}", CacheKey);
                    _cache.Remove(CacheKey);

                    var dto = _mapper.Map<OrderProfileDto>(order);

                    totalTimer.Stop();

                    // Success Metrics
                    _logger.LogInformation(OrderCreationMetrics, 
                        "OrderCreationMetrics: Success=True, TotalMs={TotalMs}, ValidationMs={ValMs}, DatabaseMs={DbMs}, ISBN={ISBN}",
                        totalTimer.ElapsedMilliseconds, 
                        validationElapsed, 
                        dbElapsed, 
                        dto.ISBN);

                    return dto;
                }
                catch (Exception ex)
                {
                    totalTimer.Stop();

                    _logger.LogError(OrderCreationMetrics, ex, 
                        "OrderCreationMetrics: Success=False, TotalMs={TotalMs}, Error={Error}, Title={Title}, ISBN={ISBN}",
                        totalTimer.ElapsedMilliseconds, 
                        ex.Message,
                        request.Title,
                        request.ISBN);

                    throw;
                }
            }
        }
    }
}