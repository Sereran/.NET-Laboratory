using System;
using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;

namespace OrdersExercise.Mapping.Resolvers
{
    public class PriceFormatterResolver : IValueResolver<Order, OrderProfileDto, string>
    {
        public string Resolve(Order source, OrderProfileDto destination, string destMember, ResolutionContext context)
        {
            decimal price = source.Price;
            if (source.Category == OrderCategory.Children)
            {
                price = decimal.Round(price * 0.9m, 2);
            }

            return price.ToString("C2");
        }
    }
}
