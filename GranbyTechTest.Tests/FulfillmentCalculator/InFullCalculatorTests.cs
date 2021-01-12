using GranbyTechTest.FulfillmentCalculator;
using GranbyTechTest.Models;
using GranbyTechTest.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GranbyTechTest.Tests.FulfillmentCalculator
{
    [TestClass]
    public class InFullCalculatorTests
    {
        private readonly InFullCalculator _sut = new InFullCalculator();

        [TestMethod]
        public void Calculate_OneLargeOrderAndMultipleSmallOrders_SkipsLargeOrderToFulfillSmallOrders()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(2, result.OrdersFulfilled);
        }

        [TestMethod]
        public void Calculate_SingleItemAndMultiItemOrders_FulfillsSingleItemOrdersToPadFulfilledNumbers()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddCuddlyToy(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(4, result.OrdersFulfilled);
        }

        [TestMethod]
        public void Calculate_EnoughStockForOneOrderOnly_FulfillsOneOrder()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 1, cuddlyToys: 1, boxes: 2, bubbleWrap: 2);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
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
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2, boxes: 1);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(1, result.OrdersFulfilled);
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
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(1, result.OrdersFulfilled);
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
        }
    }
}
