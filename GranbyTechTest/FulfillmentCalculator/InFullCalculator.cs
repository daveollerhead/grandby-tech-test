using System.Collections.Generic;
using System.Linq;
using GranbyTechTest.Extensions;
using GranbyTechTest.Models;

namespace GranbyTechTest.FulfillmentCalculator
{
    public class InFullCalculator : FulfillmentCalculator
    {
        public override FulfillmentResult Calculate(ICollection<Order> jobs, Inventory inventory)
        {
            var results = new List<FulfillmentResult>();

            var permutations = jobs.ToList().GeneratePermutations();
            foreach (var permutation in permutations)
            {
                var clone = inventory.Clone();
                var result = CalculateCore(permutation, clone);
                results.Add(result);
            }

            return results.OrderByDescending(x => x.OrdersFulfilled).FirstOrDefault();
        }
    }
}