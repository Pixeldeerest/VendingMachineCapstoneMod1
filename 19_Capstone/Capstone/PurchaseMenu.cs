using Capstone.CLI;
using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;
using TestingASCIIArt;

namespace Capstone
{
    public class PurchaseMenu : ConsoleMenu
    {
        public VendingMachine vendingMachine;
        VisualVendingMachine vendor = new VisualVendingMachine();
        public PurchaseMenu(VendingMachine vendingMachine)
        {
            this.vendingMachine = vendingMachine;
            AddOption("(1) Feed Money", FeedMoney);
            AddOption("(2) Select Product", SelectProduct);
            AddOption("(3) Finish Transaction", FinishTransaction);
            AddOption("", SalesReport);

            Configure(cfg =>
            {
                cfg.ItemForegroundColor = ConsoleColor.Yellow;
                cfg.MenuSelectionMode = MenuSelectionMode.Arrow; // KeyString: User types a key, Arrow: User selects with arrow
                cfg.KeyStringTextSeparator = ": ";
                cfg.Title = "Purchase Menu";
                cfg.SelectedItemForegroundColor = ConsoleColor.Red;
            });
        }

        protected override void OnAfterShow()
        {
            Console.WriteLine($"\nCurrent Money Provided: {vendingMachine.GetBalance():C2}");
        }

        private MenuOptionResult SalesReport()
        {
            vendingMachine.WriteSalesReport();
            Console.WriteLine("Sales Report Created");
            return MenuOptionResult.WaitAfterMenuSelection;
        }
        private MenuOptionResult FeedMoney()
        {
            bool isValidData = false;
            while (!isValidData)
            {
                try
                {
                    Console.Write("Please insert cash ($1, $2, $5, or $10)(please enter just the number (no $)): ");
                    double balanceBefore = vendingMachine.GetBalance();
                    int money = int.Parse(Console.ReadLine());
                    if (money == 1 || money == 2 || money == 5 || money == 10)
                    {
                        vendingMachine.UpdateBalance(money);
                        isValidData = true;
                        vendingMachine.PurchaseLog("FEED MONEY", balanceBefore);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter either $1, $2, $5, or $10");
                }
            }
            return MenuOptionResult.DoNotWaitAfterMenuSelection;

        }

        private MenuOptionResult SelectProduct()
        {
            vendor.Visualize();
            vendingMachine.GetInventory();
                // TODO update this loop so it differentiates between a quantity exception and an identifier exception
                //Exception QuantityException = new Exception();
                //Exception IdentifierException = new Exception();
            try
            {
                Console.Write("\nPlease enter a letter (A-D): ");
                string input = Console.ReadLine().ToUpper();
                if (input.Length > 1)
                {
                    throw new Exception();
                }
                Console.Write("Please enter a number (1-4): ");
                string input2 = Console.ReadLine();
                if (input2.Length > 1)
                {
                    throw new Exception();
                }
                //Call VendingMachine.MakeSelection()
                //Code smell, really long names
                input += input2;
                for(int i = 0; i < vendingMachine.CurrentProductStock.Count; i++)
                {
                    if (vendingMachine.CurrentProductStock.ContainsKey(input))
                    {
                        if (vendingMachine.CurrentProductStock[input].Quantity != 0)
                        {
                            vendingMachine.Dispense(input);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Product is SOLD OUT!");
                            throw new QuantityException("Quantity Exception", input, vendingMachine.CurrentProductStock[input].Name);
                        }
                    }
                    if (i == vendingMachine.CurrentProductStock.Count - 1)
                    {
                        throw new Exception();
                    }
                }
            }
            catch (QuantityException ex)
            {
                //Could refer to the code from ex.
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("You did not enter a valid option");
            }
            
        return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult FinishTransaction()
        {
            int[] change = vendingMachine.FinishTransaction();
            //return sum in same array
            int sum = 0;
            //TODO - Actually change to get sum
            Console.WriteLine($"Your Change is: {change[0]} quarters, {change[1]} dimes, {change[2]} nickels, or {sum:C2}");
            return Exit();
        }

    }
}
