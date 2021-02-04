using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.CLI
{
    public class MainMenu : ConsoleMenu
    {
        /*******************************************************************************
         * Private data:
         * Usually, a menu has to hold a reference to some type of "business objects",
         * on which all of the actions requested by the user are performed. A common 
         * technique would be to declare those private fields here, and then pass them
         * in through the constructor of the menu.
         * ****************************************************************************/

        // NOTE: This constructor could be changed to accept arguments needed by the menu
        public VendingMachine vendingMachine = new VendingMachine(0);
        public MainMenu()
        {
            //VendingMachine vendingMachine = new VendingMachine(0);
            // Add Sample menu options
            AddOption("(1) Display Vending Machine Items", DisplayVendingMachine);
            AddOption("(2) Purchase", Purchase);
            AddOption("(3) Exit", Exit);


            Configure(cfg =>
           {
               cfg.ItemForegroundColor = ConsoleColor.Cyan;
               cfg.MenuSelectionMode = MenuSelectionMode.Arrow; // KeyString: User types a key, Arrow: User selects with arrow
               cfg.KeyStringTextSeparator = ": ";
               cfg.Title = "Main Menu";
           });
        }

        private MenuOptionResult Purchase()
        {
            PurchaseMenu purchaseMenu = new PurchaseMenu(vendingMachine);
            purchaseMenu.Show();
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        public void Start()
        {
            vendingMachine.Restock();
        }

        private MenuOptionResult DisplayVendingMachine()
        {
            vendingMachine.GetInventory();
            /*
            foreach (KeyValuePair<string,Product> kvp in vendingMachine.CurrentProductStock)
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
            }*/
            return MenuOptionResult.WaitAfterMenuSelection;
        }
    }
}
