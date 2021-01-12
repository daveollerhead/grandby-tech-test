using System.Collections.Generic;
using System.Linq;
using GranbyTechTest.Extensions;
using GranbyTechTest.Models;

namespace GranbyTechTest.FulfillmentCalculator
{
    public class OnTimeInFullCalculator : FulfillmentCalculator
    {
        public override FulfillmentResult Calculate(ICollection<Order> jobs, Inventory inventory)
        {
            var nextDayJobs = jobs.Where(x => x.DeliveryOption == DeliveryOption.NextDay).ToList();
            var dayAfterJobs = jobs.Except(nextDayJobs).ToList();

            var nextDayJobsPermutations = nextDayJobs.GeneratePermutations();
            var dayAfterJobsPermutations = dayAfterJobs.GeneratePermutations();

            var permutations = new List<List<Order>>();
            foreach (var nextDayPermutation in nextDayJobsPermutations)
            {
                foreach (var dayAfterPermutation in dayAfterJobsPermutations)
                {
                    var combined = nextDayPermutation.Union(dayAfterPermutation).ToList();
                    permutations.Add(combined);
                }
            }

            var results = new List<FulfillmentResult>();
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
