using System.Linq;
using GranbyTechTest.FulfillmentCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GranbyTechTest.Tests.Extensions
{
    public static class FulfillmentResultExtensions
    {
        public static void AssertProductShortfall(this FulfillmentResult result, int productId, int expected)
        {
            var shortfall = result.Shortfall.Products.FirstOrDefault(x => x.ItemId == productId);
            Assert.IsNotNull(shortfall);
            Assert.AreEqual(expected, shortfall.StockRequired);
        }

        public static void AssertSuppliesShortfall(this FulfillmentResult result, int supplyId, int expected)
        {
            var shortfall = result.Shortfall.Supplies.FirstOrDefault(x => x.ItemId == supplyId);
            Assert.IsNotNull(shortfall);
            Assert.AreEqual(expected, shortfall.StockRequired);
        }
    }
}