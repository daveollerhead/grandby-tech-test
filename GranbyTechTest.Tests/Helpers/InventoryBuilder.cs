using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GranbyTechTest.Models;
using GranbyTechTest.Tests.Constants;

namespace GranbyTechTest.Tests.Helpers
{
    public class InventoryBuilder
    {
        public static Inventory Create(int xboxGames, int cuddlyToys, int boxes = 10, int bubbleWrap = 10)
        {
            var inventory = new Inventory();
            inventory.AddProduct(ProductIds.XboxGame, "Xbox Game", xboxGames);
            inventory.AddProduct(ProductIds.CuddlyToy, "Cuddly Toy", cuddlyToys);

            inventory.AddBoxes(boxes);
            inventory.AddBubbleWrap(bubbleWrap);

            return inventory;
        }
    }
}
