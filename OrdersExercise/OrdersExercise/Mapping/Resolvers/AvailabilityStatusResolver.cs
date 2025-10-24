using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;

namespace OrdersExercise.Mapping.Resolvers
{
    public class AvailabilityStatusResolver : IValueResolver<Order, OrderProfileDto, string>
    {
        public string Resolve(Order source, OrderProfileDto destination, string destMember, ResolutionContext context)
        {
            // Follow the specified precedence
            if (!source.IsAvailable)
                return "Out of Stock";

            if (source.IsAvailable && source.StockQuantity == 0)
                return "Unavailable";

            if (source.IsAvailable && source.StockQuantity == 1)
                return "Last Copy";

            if (source.IsAvailable && source.StockQuantity <= 5)
                return "Limited Stock";

            return "In Stock";
        }
    }
}
