using System;
using System.Data;

namespace Consoler.Shop
{
	public static class Store
	{
		public static float playerMoney => Player.money;

        const string UNDERLINE = "\x1B[4m";
        const string RESET = "\x1B[0m";

        public static Dictionary<string, float> shopInventory = new()
		{
			{"Laptop Igora", 150f },
			{"tomasz", 5f },
			{"rudzielec", 10f }
		};

		public static async void ShopInterface()
		{
			Console.Write(">");
			string command = Console.ReadLine().ToLower().Trim();

			Console.WriteLine("\n");

			switch (command)
			{
				case "list" or "inventory" or "items":

					foreach (var item in shopInventory)
					{
						Console.WriteLine($"{UNDERLINE}{item.Key}{RESET}");
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"{item.Value.ToString("0.00")}$");
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write("\n");
					}
					break;

				case string s when s.StartsWith("buy") && s.Split(' ',StringSplitOptions.RemoveEmptyEntries).Length >= 2 && shopInventory.ContainsKey(s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]):
					List<string> z = s.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
					z.RemoveAt(0);
					string r = string.Join("", z);
                    if (playerMoney >= shopInventory[r])
					{
						Console.Write("Do you want to buy {0} for {1}? Y/N", r, shopInventory[s.Split(' ',StringSplitOptions.RemoveEmptyEntries)[1]]);
						string commandSelection = Console.ReadKey().KeyChar.ToString().ToLower();
					
						if(commandSelection == "y")
						{
							Player.money -= shopInventory[s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]];
							shopInventory.Remove(r);
							//inventory system in the future
						}
						else
						{
							break;
						}
					}
					else
					{
						Console.WriteLine("You can't afford that item.");
					}
					break;

				case "exit" or "leave" or "home":
					Program.canDrawProgressBars = true;
					await Consoler.Program.CommandInput();
					return;

				default:
					Console.WriteLine("Unknown command.");
					break;
			}
			ShopInterface();
		}
	}
}

