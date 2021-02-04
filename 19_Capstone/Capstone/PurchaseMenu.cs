using Capstone.CLI;
using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class PurchaseMenu : ConsoleMenu
    {
        public VendingMachine vendingMachine;
        public PurchaseMenu(VendingMachine vendingMachine)
        {
            //VendingMachine vendingMachine = new VendingMachine(0);
            // Add Sample menu options
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
                            break;
                        }
                    }
                    if (i == vendingMachine.CurrentProductStock.Count - 1)
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("You did not enter a valid option");
            }
            //catch (Exception QuantityException)
            //{
            //
            //}
            
        return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult FinishTransaction()
        {
            vendingMachine.FinishTransaction();
            return Exit();
        }

    }
}
