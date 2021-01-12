using System;
using System.Collections.Generic;
using System.Linq;

namespace GranbyTechTest.Models
{
    public class Order
    {
        public int Id { get; }
        private readonly ICollection<OrderLine> _items = new List<OrderLine>();
        public IEnumerable<OrderLine> Items => _items.ToList();
        public DateTime CreatedAt { get; private set; }
        public DeliveryOption DeliveryOption { get; }

        public Order(int id, DeliveryOption deliveryOption)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            DeliveryOption = deliveryOption;
        }

        public void AddItem(int productId, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            _items.Add(new OrderLine(productId, quantity));
        }

        // for testing purposes, i want to be able to change the created at date
        // to test the FIFO order.
        public void TamperWithOrderDate(int orderedInDays)
        {
            CreatedAt = DateTime.UtcNow.AddDays(orderedInDays);
        }
    }
}