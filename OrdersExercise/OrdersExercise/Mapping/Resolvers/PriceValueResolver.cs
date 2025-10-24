using System;
using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;

namespace OrdersExercise.Mapping.Resolvers
{
    public class PriceValueResolver : IValueResolver<Order, OrderProfileDto, decimal>
    {
        public decimal Resolve(Order source, OrderProfileDto destination, decimal destMember, ResolutionContext context)
        {
            if (source.Category == OrderCategory.Children)
                return decimal.Round(source.Price * 0.9m, 2);

            return source.Price;
        }
    }
}
