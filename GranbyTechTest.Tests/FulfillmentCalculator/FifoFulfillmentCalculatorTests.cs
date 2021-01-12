using GranbyTechTest.FulfillmentCalculator;
using GranbyTechTest.Models;
using GranbyTechTest.Tests.Constants;
using GranbyTechTest.Tests.Extensions;
using GranbyTechTest.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GranbyTechTest.Tests.FulfillmentCalculator
{
    [TestClass]
    public class FifoFulfillmentCalculatorTests
    {
        private readonly FifoCalculator _sut = new FifoCalculator();

        [TestMethod]
        public void Calculate_NoStockToFulfillLastOrder_ShortfallContainsStockNeededToFulfillLastOrder()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 3, cuddlyToys: 3);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .ToBeOrderedIn(days: 1) // last order
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(3)
                .AddCuddlyToy(3)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(1, result.OrdersFulfilled);
            result.AssertProductShortfall(ProductIds.XboxGame, 2);
            result.AssertProductShortfall(ProductIds.CuddlyToy, 2);
        }

        [TestMethod]
        public void Calculate_AllOrdersFulfilled_ReturnsFullOrderCountNoShortfall()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(2, result.OrdersFulfilled);
            Assert.AreEqual(0, result.Shortfall.Products.Count);
            Assert.AreEqual(0, result.Shortfall.Supplies.Count);
        }

        [TestMethod]
        public void Calculate_NotEnoughXboxGamesToGoAround_ReturnsShortfallInXboxGames()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 1, cuddlyToys: 2);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);
            Assert.AreEqual(1, result.OrdersFulfilled);
            result.AssertProductShortfall(ProductIds.XboxGame, 1);
            result.AssertProductShortfall(ProductIds.CuddlyToy, 0);
        }

        [TestMethod]
        public void Calculate_EnoughStockForOneOrderOnly_FulfillsOneOrder()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 1, cuddlyToys: 1, boxes: 2, bubbleWrap: 2);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .ToBeOrderedIn(days: -1) // first order
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(1, result.OrdersFulfilled);
        }

        [TestMethod]
        public void Calculate_OnlyOneBox_CanOnlyFulfillOneSingleItemOrder()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2, boxes: 1, bubbleWrap: 10);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(1, result.OrdersFulfilled);
            result.AssertSuppliesShortfall(SupplyIds.Boxes, 4);
        }

        [TestMethod]
        public void Calculate_OnlyOneBubbleWrap_CanOnlyFulfillOneSingleItemOrder()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2, boxes: 10, bubbleWrap: 1);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(1, result.OrdersFulfilled);
            result.AssertSuppliesShortfall(SupplyIds.BubbleWrap, 5);
        }

        [TestMethod]
        public void Calculate_NoBoxes_CannotFulfillAnyOrders()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2, boxes: 0, bubbleWrap: 1);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(0, result.OrdersFulfilled);
            result.AssertSuppliesShortfall(SupplyIds.Boxes, 1);
        }

        [TestMethod]
        public void Calculate_NoBubbleWrap_CannotFulfillAnyOrders()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2, boxes: 10, bubbleWrap: 0);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(0, result.OrdersFulfilled);
            result.AssertSuppliesShortfall(SupplyIds.BubbleWrap, 1);
        }
    }
}
