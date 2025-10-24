using System;
using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;
using OrdersExercise.Mapping.Resolvers;

namespace OrdersExercise.Mapping
{
    public class AdvancedOrderMappingProfile : Profile
    {
        public AdvancedOrderMappingProfile()
        {
            // Map from request to entity
            CreateMap<CreateOrderProfileRequest, Order>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore()); // UpdatedAt should be ignored when creating

            // Map from entity to DTO using custom resolvers for computed/display values
            CreateMap<Order, OrderProfileDto>()
                .ForMember(d => d.Price, opt => opt.MapFrom<PriceValueResolver>())
                .ForMember(d => d.FormattedPrice, opt => opt.MapFrom<PriceFormatterResolver>())
                .ForMember(d => d.CoverImageUrl, opt => opt.MapFrom<CoverImageResolver>())
                .ForMember(d => d.CategoryDisplayName, opt => opt.MapFrom<CategoryDisplayResolver>())
                .ForMember(d => d.PublishedAge, opt => opt.MapFrom<PublishedAgeResolver>())
                .ForMember(d => d.AuthorInitials, opt => opt.MapFrom<AuthorInitialsResolver>())
                .ForMember(d => d.AvailabilityStatus, opt => opt.MapFrom<AvailabilityStatusResolver>());
        }
    }
}
