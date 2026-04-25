using System;

namespace PizzeriaApp
{
    enum MenuType
    {
        Snacks = 1,
        Pizza = 2,
        Bar = 3,
        Checkout = 0
    }
    
    enum Pizza
    {
        Margarita = 1,
        Pepperoni = 2,
        Hawaiian = 3,
        FourCheese = 4,
        VillageStyle = 5,
        Anchovies = 6
    }

    enum Drink
    {
        Water = 1,
        Cola = 2,
        Juice = 3,
        Beer = 4,
        Wine = 5,
        Soda = 6
    }

    enum Snack
    {
        Fries = 1,
        Nuggets = 2,
        GarlicBread = 3,
        Chips = 4
    }

    class Program
    {
        static void ShowMenu(MenuType menu)
        {
            Console.WriteLine("\nMENU:");

            switch (menu)
            {
                case MenuType.Pizza:
                    Console.WriteLine("1 - Margherita ($8)");
                    Console.WriteLine("2 - Pepperoni ($10)");
                    Console.WriteLine("3 - Hawaiian ($9)");
                    Console.WriteLine("4 - Four Cheese ($11)");
                    Console.WriteLine("5 - Village Style ($10)");
                    Console.WriteLine("6 - Anchovies ($12)");
                    break;

                case MenuType.Bar:
                    Console.WriteLine("1 - Water ($2)");
                    Console.WriteLine("2 - Cola ($3)");
                    Console.WriteLine("3 - Juice ($4)");
                    Console.WriteLine("4 - Beer ($5)");
                    Console.WriteLine("5 - Wine ($6)");
                    Console.WriteLine("6 - Soda ($3)");
                    break;

                case MenuType.Snacks:
                    Console.WriteLine("1 - Fries ($4)");
                    Console.WriteLine("2 - Nuggets ($6)");
                    Console.WriteLine("3 - Garlic Bread ($5)");
                    Console.WriteLine("4 - Chips ($4)");
                    break;
            }
        }
        
        static int CheckInt(string message, int min, int max)
        {
            int value;

            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();

                if (int.TryParse(input, out value))
                {
                    if (value >= min && value <= max)
                        return value;
                }

                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
        
        static void Main(string[] args)
        {
            string[] items = new string[100];
            int[] quantities = new int[100];
            int[] prices = new int[100];

            int orderIndex = 0;

            Console.WriteLine("WELCOME WELCOME, ESTEEMED CUSTOMER, TO OUR HUMBLE PIZZERIA!");

            while (true)
            {
                Console.WriteLine("\nSelect menu type:");
                Console.WriteLine("1 - Snacks");
                Console.WriteLine("2 - Pizza");
                Console.WriteLine("3 - Bar");
                Console.WriteLine("0 - Checkout");

                int menuChoice = CheckInt("Select menu type:", 0, 3);

                if (menuChoice == 0)
                    break;

                MenuType selectedMenu = (MenuType)menuChoice;

                ShowMenu(selectedMenu);

                int code = 0;

                switch (selectedMenu)
                {
                    case MenuType.Pizza:
                        code = CheckInt("Enter item code:", 1, 6);
                        break;
                    case MenuType.Bar:
                        code = CheckInt("Enter item code:", 1, 6);
                        break;
                    case MenuType.Snacks:
                        code = CheckInt("Enter item code:", 1, 4);
                        break;
                }

                int quantity = CheckInt("Enter quantity:", 1, 100);

                string itemName = "";
                int price = 0;

                switch (selectedMenu)
                {
                    case MenuType.Pizza:
                        Pizza pizza = (Pizza)code;

                        switch (pizza)
                        {
                            case Pizza.Margarita:
                                itemName = "Margarita";
                                price = 8;
                                break;
                            case Pizza.Pepperoni:
                                itemName = "Pepperoni";
                                price = 10;
                                break;
                            case Pizza.Hawaiian:
                                itemName = "Hawaiian";
                                price = 9;
                                break;
                            case Pizza.FourCheese:
                                itemName = "Four Cheese";
                                price = 11;
                                break;
                            case Pizza.VillageStyle:
                                itemName = "Village Style";
                                price = 10;
                                break;
                            case Pizza.Anchovies:
                                itemName = "Anchovies";
                                price = 12;
                                break;
                        }
                        break;

                    case MenuType.Bar:
                        Drink drink = (Drink)code;

                        switch (drink)
                        {
                            case Drink.Water:
                                itemName = "Water";
                                price = 2;
                                break;
                            case Drink.Cola:
                                itemName = "Cola";
                                price = 3;
                                break;
                            case Drink.Juice:
                                itemName = "Juice";
                                price = 4;
                                break;
                            case Drink.Beer:
                                itemName = "Beer";
                                price = 5;
                                break;
                            case Drink.Wine:
                                itemName = "Wine";
                                price = 6;
                                break;
                            case Drink.Soda:
                                itemName = "Soda";
                                price = 3;
                                break;
                        }
                        break;

                    case MenuType.Snacks:
                        Snack snack = (Snack)code;

                        switch (snack)
                        {
                            case Snack.Fries:
                                itemName = "Fries";
                                price = 4;
                                break;
                            case Snack.Nuggets:
                                itemName = "Nuggets";
                                price = 6;
                                break;
                            case Snack.GarlicBread:
                                itemName = "Garlic Bread";
                                price = 5;
                                break;
                            case Snack.Chips:
                                itemName = "Chips";
                                price = 4;
                                break;
                        }
                        break;
                }

                items[orderIndex] = itemName;
                quantities[orderIndex] = quantity;
                prices[orderIndex] = price;

                orderIndex++;

                Console.WriteLine("Added to order!");
            }

            Console.WriteLine("\n~~~~~~~~~");
            Console.WriteLine("YOUR RECEIPT:");
            Console.WriteLine("~~~~~~~~~");

            int grandTotal = 0;

            for (int i = 0; i < orderIndex; i++)
            {
                int total = prices[i] * quantities[i];
                grandTotal += total;

                Console.WriteLine($"{items[i]} x{quantities[i]} - ${total}");
            }

            Console.WriteLine("~~~~~~~~~");
            Console.WriteLine($"TOTAL: ${grandTotal}");
            Console.WriteLine("~~~~~~~~~");
        }
    }
}
