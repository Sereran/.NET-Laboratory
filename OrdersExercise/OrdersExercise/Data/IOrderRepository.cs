using System.Collections.Generic;
using System.Threading.Tasks;
using OrdersExercise.Models;

namespace OrdersExercise.Data
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIsbnAsync(string isbn);
        Task AddAsync(Order order);
        Task<List<Order>> GetAllAsync();
    }
}

