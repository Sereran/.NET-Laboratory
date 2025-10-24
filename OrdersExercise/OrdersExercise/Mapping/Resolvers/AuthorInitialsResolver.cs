using AutoMapper;
using OrdersExercise.Dtos;
using OrdersExercise.Models;
using System;

namespace OrdersExercise.Mapping.Resolvers
{
    public class AuthorInitialsResolver : IValueResolver<Order, OrderProfileDto, string>
    {
        public string Resolve(Order source, OrderProfileDto destination, string destMember, ResolutionContext context)
        {
            var author = source.Author?.Trim();
            if (string.IsNullOrEmpty(author))
                return "?";

            var parts = author.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return parts[0].Substring(0, 1).ToUpperInvariant();
            }

            var first = parts[0].Substring(0, 1);
            var last = parts[parts.Length - 1].Substring(0, 1);
            return (first + last).ToUpperInvariant();
        }
    }
}

