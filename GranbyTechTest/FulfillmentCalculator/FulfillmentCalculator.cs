using System;
using System.Collections.Generic;
using System.Linq;
using GranbyTechTest.FulfillmentCalculator.Shortfall;
using GranbyTechTest.Models;

namespace GranbyTechTest.FulfillmentCalculator
{
    public abstract class FulfillmentCalculator
    {
        private readonly Dictionary<int, int> _productShortfall = new Dictionary<int, int>();
        private readonly Dictionary<int, int> _supplyShortfall = new Dictionary<int, int>();

        public virtual FulfillmentResult Calculate(ICollection<Order> jobs, Inventory inventory)
        {
            return CalculateCore(jobs, inventory);
        }

        protected FulfillmentResult CalculateCore(IEnumerable<Order> jobs, Inventory inventory)
        {
            var fulfilledInFull = 0;
            foreach (var order in jobs)
            {
                if (inventory.HasStock(order))
                {
                    fulfilledInFull++;
                    foreach (var orderItem in order.Items)
                    {
                        inventory.ReduceStock(orderItem.ProductId, orderItem.Quantity);
                    }
                }
                else
                {
                    foreach (var orderItem in order.Items)
                    {
                        AddToShortfall(orderItem.ProductId, orderItem.Quantity, inventory);
                    }
                }
            }

            var shortfall = CalculateShortfall(inventory);

            return new FulfillmentResult(fulfilledInFull, shortfall);
        }

        private void AddToShortfall(int productId, int amount, Inventory inventory)
        {
            AddToProductShortfall(productId, amount);

            var product = inventory.FindProduct(productId);
            if (product == null || !product.RequiredSupplies.Any())
                return;

            foreach (var supply in product.RequiredSupplies)
            {
                AddToSupplyShortfall(supply.Id, supply.Stock * amount);
            }
        }

        private Shortfall.Shortfall CalculateShortfall(Inventory inventory)
        {
            var shortfall = new Shortfall.Shortfall();

            foreach (var (key, value) in _productShortfall)
            {
                var inInventory = inventory.FindProduct(key);
                if (inInventory == null)
                {
                    shortfall.Products.Add(new ProductShortfall(key, value));
                    continue;
                }

                shortfall.Products.Add(new ProductShortfall(key, Math.Max(0, value - inInventory.Stock)));
            }

            foreach (var (key, value) in _supplyShortfall)
            {
                var inInventory = inventory.FindSupplies(key);
                if (inInventory == null)
                {
                    shortfall.Supplies.Add(new SupplyShortfall(key, value));
                    continue;
                }

                shortfall.Supplies.Add(new SupplyShortfall(key, Math.Max(0, value - inInventory.Stock)));
            }

            _supplyShortfall.Clear();
            _productShortfall.Clear();

            return shortfall;
        }

        private void AddToProductShortfall(int productId, int amount)
        {
            if (_productShortfall.ContainsKey(productId))
            {
                _productShortfall[productId] += amount;
            }
            else
            {
                _productShortfall.Add(productId, amount);
            }
        }

        private void AddToSupplyShortfall(int productId, int amount)
        {
            if (_supplyShortfall.ContainsKey(productId))
            {
                _supplyShortfall[productId] += amount;
            }
            else
            {
                _supplyShortfall.Add(productId, amount);
            }
        }
    }
}