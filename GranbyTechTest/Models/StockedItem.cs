using System;

namespace GranbyTechTest.Models
{
    public abstract class StockedItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Stock { get; private set; }

        protected StockedItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void IncreaseStock(int amount)
        {
            SetStockLevel(Stock + amount);
        }

        public void DecreaseStock(int amount)
        {
            SetStockLevel(Stock - amount);
        }

        private void SetStockLevel(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            Stock = quantity;
        }
    }
}