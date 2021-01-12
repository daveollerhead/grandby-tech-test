namespace GranbyTechTest.FulfillmentCalculator.Shortfall
{
    public abstract class ItemShortFall
    {
        public int ItemId { get; }
        public int StockRequired { get; }

        protected ItemShortFall(int itemId, int stockRequired)
        {
            ItemId = itemId;
            StockRequired = stockRequired;
        }
    }
}