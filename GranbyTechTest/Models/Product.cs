using System.Collections.Generic;
using System.Linq;

namespace GranbyTechTest.Models
{
    public class Product : StockedItem
    {
        public Product(int id, string name)
            : base(id, name)
        {
            SetRequiredBoxes(0);
            SetRequiredBubbleWrap(0);
        }

        public Product(int id, string name, int requiredBoxes, int requiredBubbleWrap)
            : base(id, name)
        {
            SetRequiredBoxes(requiredBoxes);
            SetRequiredBubbleWrap(requiredBubbleWrap);
        }

        private readonly List<Supplies> _requiredSupplies = new List<Supplies>();
        public IEnumerable<Supplies> RequiredSupplies => _requiredSupplies.ToList();

        private void SetRequiredBoxes(int amount)
        {
            var boxes = Supplies.Boxes();
            boxes.IncreaseStock(amount);
            _requiredSupplies.Add(boxes);
        }

        private void SetRequiredBubbleWrap(int amount)
        {
            var bubbleWrap = Supplies.BubbleWrap();
            bubbleWrap.IncreaseStock(amount);
            _requiredSupplies.Add(bubbleWrap);
        }
    }
}