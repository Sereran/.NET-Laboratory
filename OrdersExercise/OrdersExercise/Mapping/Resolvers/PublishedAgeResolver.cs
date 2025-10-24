using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;
using System;

namespace OrdersExercise.Mapping.Resolvers
{
    public class PublishedAgeResolver : IValueResolver<Order, OrderProfileDto, string>
    {
        public string Resolve(Order source, OrderProfileDto destination, string destMember, ResolutionContext context)
        {
            var published = source.PublishedDate;
            var days = (DateTime.UtcNow - published).TotalDays;

            if (days < 30)
                return "New Release";

            if (days < 365)
            {
                var months = (int)Math.Floor(days / 30);
                return $"{months} months old";
            }

            if (days < 1825)
            {
                var years = (int)Math.Floor(days / 365);
                return $"{years} years old";
            }

            return "Classic";
        }
    }
}

