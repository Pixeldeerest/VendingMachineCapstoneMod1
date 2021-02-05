using MenuFramework;
using System;
using System.Collections.Generic;
using System.Text;
using TestingASCIIArt;

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
        VisualVendingMachine vendor = new VisualVendingMachine();
        public MainMenu()
        {
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
            vendor.Visualize();
            vendingMachine.GetInventory();
            return MenuOptionResult.WaitAfterMenuSelection;
        }
    }
}
