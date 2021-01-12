using System.Collections.Generic;
using GranbyTechTest.Models;

namespace GranbyTechTest.FulfillmentCalculator
{
    public class FulfillmentResult
    {
        public int OrdersFulfilled { get; set; }
        public Shortfall.Shortfall Shortfall { get; }

        public FulfillmentResult(int ordersFulfilled, Shortfall.Shortfall shortfall)
        {
            OrdersFulfilled = ordersFulfilled;
            Shortfall = shortfall;
        }
    }
}