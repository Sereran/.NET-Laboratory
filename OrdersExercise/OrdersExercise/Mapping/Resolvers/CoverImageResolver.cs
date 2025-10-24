using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;

namespace OrdersExercise.Mapping.Resolvers
{
    public class CoverImageResolver : IValueResolver<Order, OrderProfileDto, string?>
    {
        public string? Resolve(Order source, OrderProfileDto destination, string? destMember, ResolutionContext context)
        {
            if (source.Category == OrderCategory.Children)
                return null;

            return source.CoverImageUrl;
        }
    }
}

