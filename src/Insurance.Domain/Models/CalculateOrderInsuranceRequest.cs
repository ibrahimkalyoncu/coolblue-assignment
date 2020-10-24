using System.Collections.Generic;
using System.Collections.Immutable;

namespace Insurance.Domain.Models
{
    public class CalculateOrderInsuranceRequest
    {
        private readonly List<OrderItemDto> _items;

        public CalculateOrderInsuranceRequest(List<OrderItemDto> items)
        {
            _items = items;
        }

        public ImmutableList<OrderItemDto> Items => _items.ToImmutableList();
    }
}