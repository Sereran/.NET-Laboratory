using Microsoft.EntityFrameworkCore;

namespace OrdersExercise.Models;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; } 
}