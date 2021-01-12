using System;
using System.Collections.Generic;
using GranbyTechTest.Models;
using GranbyTechTest.Tests.Constants;

namespace GranbyTechTest.Tests.Helpers
{
    public class JobsBuilder
    {
        private readonly List<Order> _orders = new List<Order>();
        private Order _current;
        private int _orderCount;

        public JobsBuilder CreateNewOrder(DeliveryOption deliveryOption)
        {
            var order = new Order(_orderCount++, deliveryOption);
            _current = order;
            _orders.Add(order);
            return this;
        }

        public JobsBuilder AddXboxGame(int quantity)
        {
            if (_current == null)
                throw new InvalidOperationException($"Must call {nameof(CreateNewOrder)} before you can add items to an order");

            _current.AddItem(ProductIds.XboxGame, quantity);
            return this;
        }

        public JobsBuilder AddCuddlyToy(int quantity)
        {
            if (_current == null)
                throw new InvalidOperationException($"Must call {nameof(CreateNewOrder)} before you can add items to an order");

            _current.AddItem(ProductIds.CuddlyToy, quantity);
            return this;
        }

        // for hacking order dates when testing FIFO method.
        public JobsBuilder ToBeOrderedIn(int days)
        {
            _current.TamperWithOrderDate(days);
            return this;
        }

        public List<Order> Build()
        {
            return _orders;
        }
    }
}
