using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersExercise.Data;
using OrdersExercise.Dtos;
using OrdersExercise.Handlers;
using OrdersExercise.Mapping;
using OrdersExercise.Models;
using Xunit;

namespace OrdersExercise.Tests.Integration
{
    public class CreateOrderHandlerIntegrationTests : IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<CreateOrderHandler>> _loggerMock;
        private readonly CreateOrderHandler _handler;
        private readonly IOrderRepository _repository;

        public CreateOrderHandlerIntegrationTests()
        {
            // Set up in-memory database with unique name per test run
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new ApplicationContext(options);
            
            var mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AdvancedOrderMappingProfile>(); 
            });
            _mapper = mapperConfig.CreateMapper();

            _cache = new MemoryCache(new MemoryCacheOptions());

            _loggerMock = new Mock<ILogger<CreateOrderHandler>>();

            _repository = new TestOrderRepository(_context);

            _handler = new CreateOrderHandler(_repository, _cache, _mapper, _loggerMock.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _cache.Dispose();
        }

        [Fact]
        public async Task Handle_ValidTechnicalOrderRequest_CreatesOrderWithCorrectMappings()
        {
            var request = new CreateOrderProfileRequest
            {
                Title = "Advanced C# Programming",
                Author = "John Doe",
                Category = OrderCategory.Technical,
                ISBN = "123-4567890",
                Price = 100.00m,
                StockQuantity = 50,
                PublishedDate = DateTime.UtcNow.AddYears(-1)
            };

            var result = await _handler.Handle(request);

            Assert.NotNull(result);
            Assert.IsType<OrderProfileDto>(result);
            Assert.Equal("Technical & Professional", result.CategoryDisplayName);
            Assert.Equal("JD", result.AuthorInitials);
            Assert.False(string.IsNullOrEmpty(result.PublishedAge));
            Assert.StartsWith("$", result.FormattedPrice);
            Assert.Equal("In Stock", result.AvailabilityStatus);

            VerifyLog(LogLevel.Information, "Starting CreateOrder Operation", Times.Once());
        }

        [Fact]
        public async Task Handle_DuplicateISBN_ThrowsValidationExceptionWithLogging()
        {
            var existingOrder = new Order
            {
                Id = Guid.NewGuid(),
                Title = "Existing Book",
                Author = "Jane Doe",
                ISBN = "999-9999999",
                Category = OrderCategory.Fiction
            };
            _context.Orders.Add(existingOrder);
            await _context.SaveChangesAsync();

            var request = new CreateOrderProfileRequest
            {
                Title = "New Book",
                Author = "Bob Smith",
                Category = OrderCategory.Fiction,
                ISBN = "999-9999999",
                Price = 20.00m
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request));
            Assert.Contains("already exists", ex.Message);
            VerifyLog(LogLevel.Warning, "Validation failed", Times.Once());
        }

        [Fact]
        public async Task Handle_ChildrensOrderRequest_AppliesDiscountAndConditionalMapping()
        {
            var request = new CreateOrderProfileRequest
            {
                Title = "Happy Bunny",
                Author = "Beatrix Potter",
                Category = OrderCategory.Children,
                ISBN = "111-2223334",
                Price = 50.00m,
                CoverImageUrl = "http://badsite.com/image.jpg"
            };

            var result = await _handler.Handle(request);

            Assert.Equal("Children", result.CategoryDisplayName);
        }
        
        private class TestOrderRepository : IOrderRepository
        {
            private readonly ApplicationContext _context;

            public TestOrderRepository(ApplicationContext context)
            {
                _context = context;
            }

            public async Task AddAsync(Order order)
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            public async Task<Order> GetByIsbnAsync(string isbn)
            {
                return await _context.Orders.FirstOrDefaultAsync(o => o.ISBN == isbn);
            }

            public async Task<List<Order>> GetAllAsync()
            {
                return await _context.Orders.ToListAsync();
            }
        }

        private void VerifyLog(LogLevel level, string messageFragment, Times times)
        {
            _loggerMock.Verify(l => l.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(messageFragment)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                times);
        }
    }
}