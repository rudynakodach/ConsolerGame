
#pragma warning disable CA1416
#pragma warning disable CS8602
#pragma warning disable CS8604

using Consoler.Shop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Consoler
{
	class Program
	{
		public static bool canDrawProgressBars = true;

		public static Thread playtimeThread = new(Player.PlaytimeExperience);
		public static Thread incomeThread = new(Player.PassiveIncome);

		public static string userName = Environment.UserName;
		public static string computerName = Environment.UserDomainName;
		public static List<string> StartupQuotes = new() { "Common sense is not so common.", "Within the problem lies the solution.", "Trust in Allah, but tie your camel.", "If you can't convince them, confuse them.", "Make it work, make it right, make it fast. ", "Nine people can't make a baby in a month.", "The Internet? Is that thing still around?", "A clever person solves a problem. A wise \r\nperson avoids it.", "Chuck Norris counted to infinity... twice.", "C is quirky, flawed, and an enormous success", "Chuck Norris doesn’t go hunting. Chuck Norris goes killing.", "Hey! It compiles! Ship it!", "God is real, unless declared integer.", "It works on my machine.", "Keyboard not found...\nPress any key to continue...", "There is no place like 127.0.0.1", "There’s no test like production.", "Who is General Failure? And why is he reading my disk?", "I think we agree, the past is over.", "Linux is only free if your time has no value.", "On the Internet, nobody knows you’re a dog.", "One man’s constant is another man’s variable.", "PHP – Yeah, you know me." };

		private static bool isInit = true;
		private static void Init()
		{
			Console.Title = $"\n{computerName}@root/user/home";
		}

		private static void StartupAnimation()
		{
			for (int i = 0; i < r.Next(10, 20); i++)
			{
				string currentQuote = StartupQuotes[r.Next(0, StartupQuotes.Count - 1)];
				for (int x = 0; x < currentQuote.Length; x++)
				{
					Console.Write(currentQuote[x]);
					Thread.Sleep(1);
				}
				Console.WriteLine("\n");
			}
			Thread.Sleep(750);
			Console.Clear();
			Console.Write($"BOOT....................");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(" OK\n");
			Console.ForegroundColor = ConsoleColor.White;

			Thread.Sleep(550);
			foreach (char chr in "Welcome, ")
			{
				Console.Write(chr);
				Thread.Sleep(1);
			}
			Thread.Sleep(350);
			foreach (char chr in userName)
			{
				Console.Write(chr);
				Thread.Sleep(175);
			}
			Console.WriteLine("\n\n");

			Thread.Sleep(750);
		}

		public static Random r = new();


		private static int animIndex = 0;
		public static void DrawTextProgressBar(int progress, int total, ConsoleColor color, string message, bool T)
		{
			List<char> animList = new() { '|', '/', '-', '\\' };
			//draw empty progress bar
			Console.CursorLeft = 0;
			Console.Write("["); //start
			Console.CursorLeft = 31;
			Console.Write("]"); //end
			Console.CursorLeft = 1;
			float onechunk = 30.0f / total;

			//draw filled part
			int position = 1;
			for (int i = 0; i < onechunk * progress; i++)
			{
				Console.BackgroundColor = color;
				Console.CursorLeft = position++;
				Console.Write(".");
			}

			//draw unfilled part
			for (int i = position; i <= 30; i++)
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.CursorLeft = position++;
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("#");
				Console.ForegroundColor = ConsoleColor.White;
			}

			//draw the rest
			Console.CursorLeft = 35;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write(message);

			if (T)
			{
				Console.Write(" {0}", animList[animIndex]);
				animIndex++;
				if (animIndex >= 4) { animIndex -= 4; }
			}
		}

		public static async Task Main()
		{
			try
			{
				Console.WindowHeight = 50;
				Console.WindowWidth = 75;
			}
			catch { }

			if (isInit)
			{
				playtimeThread.Start();
				incomeThread.Start();
				Init();
				StartupAnimation();
				isInit = false;
			}

			Player.ExperienceBar();
			await CommandInput();

			//await Task.Run(Player.PlaytimeExperience);
		}

		public static async Task CommandInput()
		{

			Console.Write($"\n{computerName}@root/user/home > ");

			string? command = Console.ReadLine().ToLower().ToString();

			switch (command)
			{
				case "work":
					if (Player.isActionAvaible)
					{
						int workTime = r.Next(100, 150);
						Console.CursorVisible = false;
						for (int i = 0; i < workTime; i++)
						{
							DrawTextProgressBar(i, workTime, ConsoleColor.Yellow, "Working...", true);
							await Task.Delay(50);
						}
						Console.CursorVisible = true;
						float moneyGained = (float)r.NextDouble() * r.Next(0, 100 * (Player.level) / 2);
						Player.money += moneyGained;
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"\n+{moneyGained}$");
						Console.ForegroundColor = ConsoleColor.White;
						new Thread(Player.CommandDelay).Start();
					}
					else
					{
						Console.WriteLine("You cannot work yet!");
					}
					break;

				case "study" or "learn":
					if (Player.isActionAvaible)
					{
						Console.CursorVisible = false;
						int studyTime = r.Next(150, 200);
						int experienceGained = r.Next(10, 50 * Player.level / 4);
						for (int i = 0; i < studyTime; i++)
						{
							DrawTextProgressBar(i, studyTime, ConsoleColor.Cyan, "Studying...", true);
							await Task.Delay(150);
						}
						Player.GrantExperience(experienceGained);
						Console.CursorVisible = true;
						new Thread(Player.CommandDelay).Start();
					}
					else
					{
						Console.WriteLine("You cannot learn yet!");
					}
					break;

				case "crime":
					int chanceToGetCaught = 5 * Player.criminalityIndex;
					int rolledNumber = r.Next(0, 100);
					if (rolledNumber > chanceToGetCaught)
					{
						new Thread(Player.CrimeDelay).Start();
						float moneyGained = (float)r.NextDouble() * r.Next(50, 100 * Player.level / 2);
						int experienceGained = r.Next(25, 100 * Player.level / 4);
						Console.WriteLine($"You commited a crime and it paid off!");

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"+{moneyGained:0.00}$");

						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write($" +{experienceGained} EXP");
						Console.ForegroundColor = ConsoleColor.White;
						Player.GrantExperience(experienceGained);
						Player.money += moneyGained;
					}
					else
					{
						int cooldown = r.Next(15, 30);
						int moneyLost = r.Next(50, 50 * Player.level);
						Console.WriteLine("You tried to commit a crime and got caught!");
						Console.WriteLine("For your offences you are sentenced for {0}s of jail time.",cooldown*2);
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"-{moneyLost}$");
						Console.ForegroundColor = ConsoleColor.White;
						Thread.Sleep(cooldown * 2000);
					}
					break;

				case "stat":
					Console.WriteLine($"\nLevel: {Player.level}\nExperience: {Player.exp}/{Player.ExpToLevelUp}\nMoney: {Player.money:0.00}$");
					break;

				case "reset":
					Console.WriteLine("Delete the current save file? Y/N");
					char commandChoice = Console.ReadKey().KeyChar;

					if (commandChoice == 'y' || commandChoice == 'Y')
					{
						Player.level = 0;
						Player.exp = 0;
						Player.money = 0;
						PlayerDataManagement.Save();
					}
					break;

				case "sus": //sussy :eyes:
					try
					{
						Console.Beep(200, 300);

						Thread.Sleep(350);

						Console.Beep(250, 300);
						Console.Beep(300, 300);
						Console.Beep(350, 300);
						Console.Beep(400, 300);
						Console.Beep(350, 300);
						Console.Beep(300, 300);
						Console.Beep(250, 300);

						Thread.Sleep(550);

						Console.Beep(250, 200);
						Console.Beep(300, 200);
						Console.Beep(250, 200);
					}
					catch
					{
						Console.WriteLine("Unknown command."); //spoofing for non-windows machines
					}

					break;

				case string s when s.StartsWith("game") && s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1].Trim() is "num":
					int correctNumber = r.Next(0, 10);
					Console.WriteLine("Guess a number between 0 - 10");
					int puzzleInput = int.Parse(Console.ReadLine());
					if (puzzleInput == correctNumber)
					{
						int expGranted = r.Next(0, 10 * Player.level / 4);
						int moneyGranted = r.Next(25, 100 * Player.level);

						Console.WriteLine("Your guess was correct!");
						Player.GrantExperience(expGranted);
						Player.money += moneyGranted;
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"+{moneyGranted}$");
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.WriteLine($"+{expGranted} EXP");
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						Console.WriteLine($"Your guess was incorrect! The answer was {correctNumber}...");
					}
					break;

				case "cls" or "clear":
					Console.Clear();
					Console.WriteLine("Console cleared.");
					break;

				case "save":
					PlayerDataManagement.Save();
					break;

				case "load":
					PlayerDataManagement.Load();
					break;

				case "shop" or "store":
					canDrawProgressBars = false;
					Console.Clear();
					Consoler.Shop.Store.ShopInterface();
					return;

				default:
					Console.WriteLine("Unknown command.");
					break;
			}
			await CommandInput();
		}
	}

	class Player
	{
		public static bool isCrimeAvaible = true;
		public static bool isActionAvaible = true;
		public static float money = 0;
		public static int level = 1;

		public static float passiveIncome = 0;

		public static int criminalityIndex = 1;

		public static int ExpToLevelUp => 100 + (10 * level);
		public static int exp = 0;

		public static void PlaytimeExperience()
		{
			while (true)
			{
				Thread.Sleep(30000);
				GrantExperience(Program.r.Next(0, 5 * level / 2));
			}
		}

		public static void PassiveIncome()
		{
			money += passiveIncome;

			Thread.Sleep(1000);
		}

		public static void CommandDelay()
		{
			isActionAvaible = false;
			Thread.Sleep(5000);
			isActionAvaible = true;
		}

		public static void CrimeDelay()
		{
			int cooldown = Program.r.Next(15, 30);
			isCrimeAvaible = false;
			Thread.Sleep(cooldown * 1000);
			isCrimeAvaible = false;
		}

		public static void ExperienceBar()
		{
			if (!Program.canDrawProgressBars)
			{
				return;
			}
			string moneyString = money.ToString("0.00");
			int x = Console.GetCursorPosition().Left;
			int y = Console.GetCursorPosition().Top;
			Console.SetCursorPosition(10, 3);
			Program.DrawTextProgressBar(exp, ExpToLevelUp, ConsoleColor.Cyan, "", false);
			Console.SetCursorPosition(18 - moneyString.Length, 4);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(moneyString);
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(x, y);
		}

		public static void GrantExperience(int expGained)
		{
			exp += expGained;
			if (exp >= ExpToLevelUp)
			{
				exp -= ExpToLevelUp;
				level++;
			}
			ExperienceBar();
		}
	}
	public class PlayerDataManagement
	{
		private static readonly string saveRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		private static readonly string gamesDirectory = saveRoot + @"\Consoler";
		private static readonly string finalDirectory = gamesDirectory + @"\save.json";
		private static readonly string storeDirectory = gamesDirectory + @"\store.json";

		public static void Save()
		{
			if (Directory.Exists(gamesDirectory))
			{
				if (File.Exists(finalDirectory) && File.Exists(storeDirectory))
				{

					Dictionary<string, float> playerData = new()
					{
						{ "money", Player.money },
						{ "playerPassiveIncome", Player.passiveIncome },
						{ "level", Player.level },
						{ "experience", Player.exp },
						{ "criminalityLevel", Player.criminalityIndex }
					};

					List<Dictionary<string, float>> shopItems = new() { Store.shopInventory };

					Tuple<Dictionary<string, float>, List<Dictionary<string, float>>> saveData = new(item1: playerData, item2: shopItems);

					var jsonData = JsonConvert.SerializeObject(saveData, Formatting.Indented);

					Console.WriteLine(jsonData.ToString());

					File.WriteAllText(finalDirectory, jsonData);
					Console.WriteLine("Data saved to {0}", finalDirectory);
				}
				else
				{
					if(!File.Exists(storeDirectory))
					{
                        File.Create(storeDirectory);
                    }
                    if (!File.Exists(finalDirectory))
					{
                        File.Create(finalDirectory);
                    }
                    Save();
				}
			}
			else
			{
				Directory.CreateDirectory(gamesDirectory);
				Save();
			}
		}
		public static void Load()
		{
			if (File.Exists(finalDirectory))
			{
				string rawSaveData = File.ReadAllText(finalDirectory);
				var jsonRoot = JToken.Parse(rawSaveData);

				Console.WriteLine(jsonRoot);
				Console.ReadLine();

				Player.level = (int)jsonRoot.SelectToken("level");
				Player.exp = (int)jsonRoot.SelectToken("experience");
				Player.money = (float)jsonRoot.SelectToken("money");
				Player.criminalityIndex = (int)jsonRoot.SelectToken("criminalityIndex");
				Player.passiveIncome = (float)jsonRoot.SelectToken("playerPassiveIncome");

				Console.WriteLine("Data loaded.");
			}
		}
	}
}