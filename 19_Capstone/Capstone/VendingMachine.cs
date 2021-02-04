using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone
{
    public class VendingMachine
    {
        private double Balance { get; set; }
        public Dictionary<string, Product> CurrentProductStock { get; set; }
        private Dictionary<string, int> SalesReport = new Dictionary<string, int>() { { "Potato Crisps", 0 }, { "Stackers", 0 },{ "Grain Waves", 0 }, { "Cloud Popcorn", 0 },
            {"Moonpie",0 },{"Cowtales",0 },{ "Wonka Bar",0 },{"Crunchie",0 },{"Skor",0 },{"Cola",0 },{"Dr. Salt", 0 }, {"Mountain Melter", 0 },{"Heavy",0 },
            {"Diet Cola",0 },{"U-Chews",0 },{"Little League Chew",0 },{"Chiclets",0 },{"Triplemint",0 } };
        private double TotalSales;

        public VendingMachine(double balance)
        {
            this.Balance = balance;
            CurrentProductStock = new Dictionary<string, Product>();
        }

        public void Restock()
        {
            string path = @"..\..\..\..\vendingmachine.csv";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] lineArray = line.Split("|");
                        Product product = new Product(lineArray[1], double.Parse(lineArray[2]), lineArray[3]);
                        CurrentProductStock.Add(lineArray[0], product);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public double GetBalance()
        {
            return this.Balance;
        }

        public double UpdateBalance(double money)
        {
            this.Balance += money;
            return this.Balance;
        }

        public void GetInventory()
        {
            foreach (KeyValuePair<string, Product> kvp in this.CurrentProductStock)
            {
                string quantity = "";
                if (kvp.Value.Quantity == 0)
                {
                    quantity = "SOLD OUT";
                }
                else
                {
                    quantity = kvp.Value.Quantity.ToString();
                }
                Console.WriteLine($"{kvp.Value.Name} at {kvp.Key} has {quantity} remaining, and costs {kvp.Value.Price}");
            }
        }

        public void Dispense(string key)
        {
            if (this.Balance >= this.CurrentProductStock[key].Price)
            {
                // balance before the dispense
                double balanceBefore = this.Balance;

                Console.WriteLine("*Clunk Clunk*");
                // update quantity for dispensing
                this.CurrentProductStock[key].Quantity -= 1;

                // display consumption message
                Console.WriteLine(this.CurrentProductStock[key].ConsumptionMessage);

                // update balance
                this.Balance -= this.CurrentProductStock[key].Price;

                // add the dispense into the log
                this.PurchaseLog(this.CurrentProductStock[key].Name, balanceBefore);

                // update sales report.  increment the counter
                SalesReport[this.CurrentProductStock[key].Name]++;
                TotalSales += this.CurrentProductStock[key].Price;

            }
            else
            {
                Console.WriteLine("Need more money for that product");
            }
        }

        public int[] FinishTransaction()
        {
            int[] change = new int[3];
            double quarter = 0.25;
            int quarters = 0;
            double nickel = 0.05;
            int nickels = 0;
            double dime = 0.10;
            int dimes = 0;
            decimal finishBalance = (decimal)this.Balance;

            double balanceBefore = this.Balance;

            if (finishBalance !=0 )
            {
                quarters = (int)(finishBalance / (decimal)quarter);
                finishBalance -= (decimal)(quarters*quarter);
                dimes = (int)(finishBalance / (decimal)dime);
                finishBalance -= (decimal)(dimes*dime);
                nickels = (int)(finishBalance / (decimal)nickel);
                finishBalance -= (decimal)(nickels*nickel);
                this.Balance = 0;

                change[0] = quarters;
                change[1] = dimes;
                change[2] = nickels;

                // log the give change
                this.PurchaseLog("GIVE CHANGE", balanceBefore);

                decimal sum = (decimal)(quarters * quarter + dimes * dime + nickels * nickel);
                Console.WriteLine($"Your Change is: {quarters} quarters, {dimes} dimes, {nickels} nickels, or {sum:C2}");
                
            }
                return change;
        }

        public void PurchaseLog(string method, double balanceBefore)
        {
            string path = @"..\..\..\..\Log.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(path,true))
                {
                    DateTime now = DateTime.Now;
                    sw.WriteLine($"{now} {method}: {balanceBefore:C2} {this.Balance:C2}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteSalesReport()
        {
            DateTime time = DateTime.Now;
            string timeString = time.ToString();
            timeString = timeString.Replace(@":", ".");
            timeString = timeString.Replace("/", ".");

            string path = @"..\..\..\..\SalesReport "+timeString+".txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (KeyValuePair<string,int> kvp in SalesReport)
                    {
                        sw.WriteLine($"{kvp.Key}|{kvp.Value}");
                    }
                    sw.WriteLine($"\nTOTAL: {this.TotalSales:C2}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }   
}
