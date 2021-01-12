using System.Collections.Generic;

namespace GranbyTechTest.FulfillmentCalculator.Shortfall
{
    public class Shortfall
    {
        public List<ProductShortfall> Products { get; set; } = new List<ProductShortfall>();
        public List<SupplyShortfall> Supplies { get; set; } = new List<SupplyShortfall>();
    }
}