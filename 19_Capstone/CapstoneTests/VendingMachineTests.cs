using Capstone;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MenuFramework;
using System.IO;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTests
    {
        [DataTestMethod]
        [DataRow("Crunch Crunch, Yum!", "A1")]
        [DataRow("Glug Glug, Yum!", "C3")]
        [DataRow("Munch Munch, Yum!", "B4")]
        [DataRow("Chew Chew, Yum!", "D2")]
        public void TestDispenseString(string expectedString, string key)
        {
            //Arrange
            VendingMachine vendingMachine = new VendingMachine(0);
            vendingMachine.Restock();

            //Act

            //Assert
            Assert.AreEqual(expectedString, vendingMachine.CurrentProductStock[key].ConsumptionMessage);

        }

        //How to test for something we've already planned for
        //[DataTestMethod]
        //[DataRow("", "F2")]
        //public void TestInvalidDispenseCode(string expectedString, string key)
        //{
        //    //Arrange
        //    VendingMachine vendingMachine = new VendingMachine(0);
        //    vendingMachine.Restock();

        //    //Act

        //    //Assert
        //    Assert.ThrowsException<KeyNotFoundException>()
        //}

        [DataTestMethod]
        [DataRow(5.00, 1.95, "A1")]
        [DataRow(10.00, 8.55, "A2")]
        [DataRow(7.50, 6.00, "B2")]
        [DataRow(5.00, 3.75, "C1")]
        [DataRow(0.00, 0.00, "A1")]
        public void TestDispenseBalance(double balance, double expectedBalanceAfterPurchase, string key)
        {
            //Arrange
            VendingMachine vendingMachine = new VendingMachine(balance);
            vendingMachine.Restock();

            //Act
            vendingMachine.Dispense(key);

            //Assert
            Assert.AreEqual((decimal)expectedBalanceAfterPurchase, (decimal)vendingMachine.GetBalance());
        }

        [DataTestMethod]
        [DataRow(3, 2, "C3")]
        //[DataRow(0, 0, "A1")] -- THIS IS ACCOUNTED FOR IN PURCHASE MENU CALLER IN SELECT PRODUCT()
        [DataRow(5, 4, "D2")]
        [DataRow(1, 0, "B4")]
        [DataRow(4, 3, "A1")]
        public void TestDispenseQuantity(int quantity, int expectedQuantityAfterPurchase, string key)
        {
            //Arrange
            VendingMachine vendingMachine = new VendingMachine(10);
            vendingMachine.Restock();
            vendingMachine.CurrentProductStock[key].Quantity = quantity;

            //Act
            vendingMachine.Dispense(key);

            //Assert
            Assert.AreEqual(expectedQuantityAfterPurchase, vendingMachine.CurrentProductStock[key].Quantity);
        }

        [DataTestMethod]
        [DataRow(3.30, 13, 0, 1)]
        [DataRow(3.40, 13, 1, 1)]
        [DataRow(0.00, 0, 0, 0)]
        [DataRow(10.00, 40, 0, 0)]
        //Cannot have a negative balance in the first place
        public void TestFinishedTransaction(double balanceBefore, int expectedQuarters,int expectedDimes,int expectedNickels)
        {

            //Arrange
            VendingMachine vendingMachine = new VendingMachine(balanceBefore);
            vendingMachine.Restock();

            //Act
            int[] change = vendingMachine.FinishTransaction();
            int actualQuarters = change[0];
            int actualDimes = change[1];
            int actualNickels = change[2];

            //Assert
            Assert.AreEqual(expectedQuarters, actualQuarters);
            Assert.AreEqual(expectedDimes, actualDimes);
            Assert.AreEqual(expectedNickels, actualNickels);
        }

        [DataTestMethod]
        [DataRow(5.00, 5.00)]
        [DataRow(10.00, 10.00)]
        [DataRow(0.00, 0.00)]
        public void TestGetBalance(double balance, double expectedBalance)
        {
            //Arrange
            VendingMachine vendingMachine = new VendingMachine(balance);

            //Act

            //Assert
            Assert.AreEqual(expectedBalance, vendingMachine.GetBalance());
        }
        //[TestMethod]
        //public void TestGetInventory()
        //{
            //Testing is a quant is 0 "SOLD OUT"

            //foreach (KeyValuePair<string, Product> kvp in this.CurrentProductStock)
            //{
            //    string quantity = "";
            //    if (kvp.Value.Quantity == 0)
            //    {
            //        quantity = "SOLD OUT";
            //    }
            //    else
            //    {
            //        quantity = kvp.Value.Quantity.ToString();
            //    }
            //    Console.WriteLine($"{kvp.Value.Name} at {kvp.Key} has {quantity} remaining, and costs {kvp.Value.Price}");
        //    }
        //}
        
        //[TestMethod]
        //public void TestPurchaseLog()
        //{
        //    string path = @"..\..\..\..\Log.txt";
        //    using (StreamReader sr = new StreamReader(path))
        //    {

        //    }

        //}


        [DataTestMethod]
        [DataRow("A1")]
        [DataRow("A2")]
        [DataRow("A3")]
        [DataRow("A4")]
        [DataRow("B1")]
        [DataRow("B2")]
        [DataRow("B3")]
        [DataRow("B4")]
        [DataRow("C1")]
        [DataRow("C2")]
        [DataRow("C3")]
        [DataRow("C4")]
        [DataRow("D1")]
        [DataRow("D2")]
        [DataRow("D3")]
        [DataRow("D4")]
        public void TestRestock(string key)
        {
            //Arrange
            VendingMachine vendingMachine = new VendingMachine(0);

            //Act
            vendingMachine.Restock();

            //Assert
            Assert.AreEqual(5, vendingMachine.CurrentProductStock[key].Quantity);

        }


        [DataTestMethod]
        [DataRow(5.00, 5.00, 10.00)]
        [DataRow(0.00, 5.00, 5.00)]
        [DataRow(3.40, 5.00, 8.40)]
        [DataRow(5.50, 1.00, 6.50)]
        public void TestUpdateBalance(double balance, double money, double expectedBalance)
        {
            //Arrange
            VendingMachine vendingMachine = new VendingMachine(balance);

            //Act


            //Assert
            Assert.AreEqual(expectedBalance, vendingMachine.UpdateBalance(money));

        }
    }
}
