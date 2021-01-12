using System.Collections.Generic;
using System.Linq;
using GranbyTechTest.Models;

namespace GranbyTechTest.FulfillmentCalculator
{
    public class FifoCalculator : FulfillmentCalculator
    {
        public override FulfillmentResult Calculate(ICollection<Order> jobs, Inventory inventory)
        {
            return CalculateCore(jobs.OrderBy(x => x.CreatedAt), inventory);
        }
    }
}