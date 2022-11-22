using System;

namespace Consoler.Shop
{
    public static class Store
    {
        public static float playerMoney => Player.money;

        const string UNDERLINE = "\x1B[4m";
        const string RESET = "\x1B[0m";

        public static Dictionary<string, float> shopInventory = new()
        {
            { "Sluchawki Tomusia", 75f },
            { "Kamerka Tomasza", 200f },
            { "Królik", 500f }
        };

        public static async void ShopInterface()
        {
            Console.Write(">");
            string command = Console.ReadLine().Trim();

            Console.WriteLine("\n");

            switch (command)
            {
                case "list" or "inventory" or "items":

                    foreach (var item in shopInventory)
                    {
                        Console.WriteLine($"{UNDERLINE}{item.Key}{RESET}");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{item.Value:0.00}$");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("\n");
                    }
                    break;

                case string s when s.StartsWith("buy") && s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 2:
                    List<string> z = s.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                    z.RemoveAt(0);
                    string r = string.Join(" ", z);
                    if (shopInventory.ContainsKey(r))
                    {
                        if (playerMoney >= shopInventory[r])
                        {
                            Console.Write("Do you want to buy {0} for {1}? Y/N\n", r, shopInventory[r]);
                            string commandSelection = Console.ReadKey().KeyChar.ToString().ToLower();
                            Console.Write("\n");

                            if (commandSelection == "y")
                            {
                                Player.money -= shopInventory[r];
                                Player.playerInventory.Add(r, shopInventory[r]);
                                switch (r)
                                {
                                    case "Sluchawki Tomusia":
                                        Player.passiveIncome += 0.05f;
                                        break;
                                    case "Królik":
                                        Player.passiveIncome += 0.75f;
                                        break;
                                    case "Kamerka Tomasza":
                                        Player.passiveIncome += 0.75f;
                                        break;
                                }
                                shopInventory.Remove(r);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("You can't afford that item.");
                    }
                    break;

                case "exit" or "leave" or "home":
                    Game.canDrawProgressBars = true;
                    await Consoler.Game.CommandInput();
                    return;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
            ShopInterface();
        }
    }
}