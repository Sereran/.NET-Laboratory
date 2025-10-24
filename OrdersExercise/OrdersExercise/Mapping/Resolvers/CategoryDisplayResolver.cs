using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;

namespace OrdersExercise.Mapping.Resolvers
{
    public class CategoryDisplayResolver : IValueResolver<Order, OrderProfileDto, string>
    {
        public string Resolve(Order source, OrderProfileDto destination, string destMember, ResolutionContext context)
        {
            return source.Category switch
            {
                OrderCategory.Fiction => "Fiction",
                OrderCategory.NonFiction => "Non-Fiction",
                OrderCategory.Technical => "Technical & Professional",
                OrderCategory.Children => "Children",
                _ => "Uncategorized",
            };
        }
    }
}

