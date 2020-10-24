using System.Collections.Generic;
using System.Collections.Immutable;

namespace Insurance.Domain.Models
{
    public class OrderDto
    {
        private readonly List<OrderItemDto> _items;

        public OrderDto(List<OrderItemDto> items)
        {
            _items = items;
        }

        public ImmutableList<OrderItemDto> Items => _items.ToImmutableList();
    }
}