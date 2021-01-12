using System;
using System.Collections.Generic;
using System.Linq;
using GranbyTechTest.Constants;

namespace GranbyTechTest.Models
{
    public class Inventory
    {
        private readonly ICollection<Product> _products = new List<Product>();
        private readonly ICollection<Supplies> _supplies = new List<Supplies>();

        public Product FindProduct(int id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }

        public Supplies FindSupplies(int id)
        {
            return _supplies.FirstOrDefault(x => x.Id == id);
        }

        public void AddProduct(int id, string name, int stockLevel)
        {
            var product = _products.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (product != null)
            {
                product.IncreaseStock(stockLevel);
                return;
            }

            var nextId = _products.Count + 1;
            var newProduct = new Product(nextId, name, requiredBoxes: 1, requiredBubbleWrap: 1);
            newProduct.IncreaseStock(stockLevel);
            _products.Add(newProduct);
        }

        public void AddBoxes(int quantity)
        {
            AddSupplies(SuppliesNames.Boxes, quantity, Supplies.Boxes());
        }

        public void AddBubbleWrap(int quantity)
        {
            AddSupplies(SuppliesNames.BubbleWrap, quantity, Supplies.BubbleWrap());
        }

        public bool HasStock(Order order)
        {
            var supplies = new Dictionary<int, int>();
            foreach (var item in order.Items)
            {
                var inProducts = _products.FirstOrDefault(x => x.Id == item.ProductId);
                if (inProducts == null || inProducts.Stock < item.Quantity)
                    return false;

                foreach (var supply in inProducts.RequiredSupplies)
                {
                    if (supplies.ContainsKey(supply.Id))
                        supplies[supply.Id] += supply.Stock * item.Quantity;
                    else
                        supplies.Add(supply.Id, supply.Stock * item.Quantity);
                }
            }

            foreach (var kvp in supplies)
            {
                var inSupplies = _supplies.FirstOrDefault(x => x.Id == kvp.Key);
                if (inSupplies == null || inSupplies.Stock < kvp.Value)
                    return false;
            }

            return true;
        }

        private bool HasStock(int productId, int quantity)
        {
            var item = _products.FirstOrDefault(x => x.Id == productId);
            if (item == null || item.Stock < quantity)
                return false;

            if (!item.RequiredSupplies.Any())
                return true;

            foreach (var supplies in item.RequiredSupplies)
            {
                var inSupplies = _supplies.FirstOrDefault(x => x.Id == supplies.Id);
                if (inSupplies == null || inSupplies.Stock < supplies.Stock * quantity)
                    return false;
            }

            return true;
        }

        public void ReduceStock(int productId, int quantity)
        {
            if (!HasStock(productId, quantity))
                throw new InvalidOperationException("Requested quantity of stock to be reduced exceeded total stock level for product");

            var itemInInventory = _products.First(x => x.Id == productId);
            itemInInventory.DecreaseStock(quantity);

            foreach (var inventory in itemInInventory.RequiredSupplies)
            {
                var inSupplies = _supplies.First(x => x.Id == inventory.Id);
                inSupplies.DecreaseStock(inventory.Stock);
            }
        }

        public Inventory Clone()
        {
            var clone = new Inventory();
            foreach (var product in _products)
            {
                clone.AddProduct(product.Id, product.Name, product.Stock);
            }

            clone.AddBoxes(_supplies.FirstOrDefault(x => x.Name == SuppliesNames.Boxes)?.Stock ?? 0);
            clone.AddBubbleWrap(_supplies.FirstOrDefault(x => x.Name == SuppliesNames.BubbleWrap)?.Stock ?? 0);

            return clone;
        }

        private void AddSupplies(string name, int stockLevel, Supplies supplies)
        {
            var supply = _supplies.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (supply != null)
            {
                supply.IncreaseStock(stockLevel);
                return;
            }

            supplies.IncreaseStock(stockLevel);
            _supplies.Add(supplies);
        }
    }
}