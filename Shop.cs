using System;
using System.Data;

namespace Consoler.Shop
{
	public static class Store
	{
		public static float playerMoney => Player.money;

		public static Dictionary<string, float> shopInventory = new()
		{
			{"test", 30f },
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
						Console.WriteLine($"{item.Key}");
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"{item.Value.ToString("0.00")}$");
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write("\n");
					}

					break;

				case string s when s.StartsWith("buy") && s.Split(' ',StringSplitOptions.RemoveEmptyEntries).Length >= 2 && shopInventory.ContainsKey(s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]):
					if(playerMoney >= shopInventory[s.Split(' ',StringSplitOptions.RemoveEmptyEntries)[1]])
					{
						Console.Write("Do you want to buy {0} for {1}? Y/N", s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1], shopInventory[s.Split(' ',StringSplitOptions.RemoveEmptyEntries)[1]]);
						string commandSelection = Console.ReadKey().KeyChar.ToString().ToLower();
					
						if(commandSelection == "y")
						{
							Player.money -= shopInventory[s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]];
							//inventory system in the future
						}
						else
						{
							break;
						}
					}
					else
					{
						Console.WriteLine("dupa");
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

