using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrdersExercise.Models;

namespace OrdersExercise.Data
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();
        private readonly object _lock = new();

        public Task AddAsync(Order order)
        {
            lock (_lock)
            {
                _orders.Add(order);
            }

            return Task.CompletedTask;
        }

        public Task<List<Order>> GetAllAsync()
        {
            List<Order> snapshot;
            lock (_lock)
            {
                snapshot = _orders.Select(o => o).ToList();
            }

            return Task.FromResult(snapshot);
        }

        public Task<Order?> GetByIsbnAsync(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return Task.FromResult<Order?>(null);

            Order? found;
            lock (_lock)
            {
                found = _orders.FirstOrDefault(o => string.Equals(o.ISBN, isbn, System.StringComparison.OrdinalIgnoreCase));
            }

            return Task.FromResult(found);
        }
    }
}

