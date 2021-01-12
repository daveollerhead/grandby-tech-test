using GranbyTechTest.FulfillmentCalculator;
using GranbyTechTest.Models;
using GranbyTechTest.Tests.Constants;
using GranbyTechTest.Tests.Extensions;
using GranbyTechTest.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GranbyTechTest.Tests.FulfillmentCalculator
{
    [TestClass]
    public class OnTimeInFullCalculatorTests
    {
        private readonly OnTimeInFullCalculator _sut = new OnTimeInFullCalculator();

        [TestMethod]
        public void Calculate_OnlyEnoughStockToFulFillNextDayOrders_FulfillsNextDayOrdersOnly()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2);

            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.DayAfter)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.DayAfter)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(1, result.OrdersFulfilled);
        }

        [TestMethod]
        public void Calculate_CanFulfillCombinationOfOrderDeliveryOptions_FulfillsAllNextDayBeforeOptimizingDayAfter()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 3, cuddlyToys: 3);


            var jobs = new JobsBuilder()
                .CreateNewOrder(DeliveryOption.NextDay)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.DayAfter)
                .AddXboxGame(2)
                .AddCuddlyToy(2)
                .CreateNewOrder(DeliveryOption.DayAfter)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.DayAfter)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(3, result.OrdersFulfilled);
        }

        [TestMethod]
        public void Calculate_CannotFulfillAllNextDayOrders_FulfillsAsManyNextDayAsPossbileBeforeFulfillingAnyDayAfterOrders()
        {
            var inventory = InventoryBuilder.Create(xboxGames: 3, cuddlyToys: 3);

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
                .CreateNewOrder(DeliveryOption.DayAfter)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .CreateNewOrder(DeliveryOption.DayAfter)
                .AddXboxGame(1)
                .AddCuddlyToy(1)
                .Build();

            var result = _sut.Calculate(jobs, inventory);

            Assert.AreEqual(3, result.OrdersFulfilled);
            result.AssertProductShortfall(ProductIds.XboxGame, 3);
            result.AssertProductShortfall(ProductIds.CuddlyToy, 3);
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
            var inventory = InventoryBuilder.Create(xboxGames: 2, cuddlyToys: 2, boxes: 0);

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
