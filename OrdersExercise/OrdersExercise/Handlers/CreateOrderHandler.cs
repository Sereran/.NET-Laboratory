using System;
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

            // Basic validation
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.", nameof(request.Title));

            if (string.IsNullOrWhiteSpace(request.Author))
                throw new ArgumentException("Author is required.", nameof(request.Author));

            if (string.IsNullOrWhiteSpace(request.ISBN))
                throw new ArgumentException("ISBN is required.", nameof(request.ISBN));

            if (request.Price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(request.Price));

            // Log the creation attempt with requested details
            _logger.LogInformation("Creating order (Title={Title}, Author={Author}, Category={Category}, ISBN={ISBN})",
                request.Title, request.Author, request.Category, request.ISBN);

            try
            {
                // Check ISBN uniqueness
                var existing = await _repository.GetByIsbnAsync(request.ISBN);
                if (existing != null)
                {
                    _logger.LogWarning("Cannot create order: ISBN {ISBN} already exists.", request.ISBN);
                    throw new InvalidOperationException($"Order with ISBN '{request.ISBN}' already exists.");
                }

                // Map request to domain Order using advanced mapping profile
                var order = _mapper.Map<Order>(request);

                // Ensure Id/CreatedAt are set by mapping profile; if not, ensure defaults
                if (order.Id == Guid.Empty)
                    order.Id = Guid.NewGuid();

                if (order.CreatedAt == default)
                    order.CreatedAt = DateTime.UtcNow;

                await _repository.AddAsync(order);

                _cache.Remove(CacheKey);

                var dto = _mapper.Map<OrderProfileDto>(order);

                _logger.LogInformation("Order created successfully (Id={Id}, ISBN={ISBN})", dto.Id, dto.ISBN);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order (Title={Title}, ISBN={ISBN})", request.Title, request.ISBN);
                throw;
            }
        }
    }
}

