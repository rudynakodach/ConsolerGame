
#pragma warning disable CA1416
#pragma warning disable CS8602
#pragma warning disable CS8604

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Consoler
{
	class Program
	{

		public static string userName = Environment.UserName;
		public static string computerName = Environment.UserDomainName;
		public static List<string> StartupQuotes = new() { "Common sense is not so common.", "Within the problem lies the solution.", "Trust in Allah, but tie your camel.", "If you can't convince them, confuse them.", "Make it work, make it right, make it fast. ", "Nine people can't make a baby in a month.", "The Internet? Is that thing still around?", "A clever person solves a problem. A wise \r\nperson avoids it.", "Chuck Norris counted to infinity... twice.", "C is quirky, flawed, and an enormous success", "Chuck Norris doesn’t go hunting. Chuck Norris goes killing.", "Hey! It compiles! Ship it!", "God is real, unless declared integer.", "It works on my machine.", "Keyboard not found...\nPress any key to continue...", "There is no place like 127.0.0.1", "There’s no test like production.", "Who is General Failure? And why is he reading my disk?", "I think we agree, the past is over.", "Linux is only free if your time has no value.", "On the Internet, nobody knows you’re a dog.", "One man’s constant is another man’s variable.", "PHP – Yeah, you know me." };

		private static bool isInit = true;
		public static Dictionary<string, bool> canPerformAction = new();
		private static void Init()
		{
			Console.Title = $"\n{computerName}@root/user/home";
			canPerformAction.Add("work", true);
			canPerformAction.Add("crime", true);
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
				Thread.Sleep(200);
			}
			Console.WriteLine("\n\n");

			Thread.Sleep(750);
		}

		public static Random r = new();


		private static int animIndex = 0;
		public static void drawTextProgressBar(int progress, int total, ConsoleColor color, string message, bool T)
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
				Init();
				//StartupAnimation();
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
					if (canPerformAction["work"] == true)
					{
						canPerformAction["work"] = false;
						int workTime = r.Next(100, 150);
						Console.CursorVisible = false;
						for (int i = 0; i < workTime; i++)
						{
							drawTextProgressBar(i, workTime, ConsoleColor.Yellow, "Working...", true);
							await Task.Delay(50);
						}
						Console.CursorVisible = true;
						int moneyGained = r.Next(0, 100 * (Player.level) / 2);
						Player.money += moneyGained;
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"\n+{moneyGained}$");
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						Console.WriteLine("You cannot work yet!");
					}
					break;


				case "study" or "learn":
					Console.CursorVisible = false;
					int studyTime = r.Next(150, 200);
					int experienceGained = r.Next(10, 50 * Player.level / 4);
					for (int i = 0; i < studyTime; i++)
					{
						drawTextProgressBar(i, studyTime, ConsoleColor.Cyan, "Studying...", true);
						await Task.Delay(150);
					}
					Player.GrantExperience(experienceGained);
					Console.CursorVisible = true;
					break;

				case "crime":
					int chanceToGetCaught = 5;
					int rolledNumber = r.Next(0, 100);
					if (rolledNumber > chanceToGetCaught)
					{
						int moneyGained = r.Next(50, 100 * Player.level / 2);
						experienceGained = r.Next(25, 100 * Player.level / 4);
						Console.WriteLine($"You commited a crime and it paid off!");

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"+{moneyGained}$");

						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write($" +{experienceGained} EXP");
						Console.ForegroundColor = ConsoleColor.White;
						Player.GrantExperience(experienceGained);
						Player.money += moneyGained;
					}
					else
					{
						int moneyLost = r.Next(50, 50 * Player.level);
						Console.WriteLine("You tried to commit a crime and got caught!");
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"-{moneyLost}$");
						Console.ForegroundColor = ConsoleColor.White;
					}
					break;

				case "stat":
					Console.WriteLine($"\nLevel: {Player.level}\nExperience: {Player.exp}/{Player.expToLevelUp}\nMoney: {Player.money}");
					break;

				case "reset":
					Console.WriteLine("Delete the current save file? Y/N");
					char commandChoice = Console.ReadKey().KeyChar;

					if(commandChoice == 'y' || commandChoice == 'Y')
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

						Console.Beep(300, 275);
						Console.Beep(350, 275);
						Console.Beep(300, 275);
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

						Console.WriteLine("Your gess was correct!");
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
					Console.Clear();
					await Shop();
					return;

				default:
					Console.WriteLine("Unknown command.");
					break;
			}
			await CommandInput();
		}

		//todo
		public static async Task Shop()
		{
			string command = Console.ReadLine().ToLower().Trim();

			switch (command)
			{
				case "exit" or "close":
					await CommandInput();
					return;

				default:
					Console.WriteLine("Unknown command.");
					break;
			}

			await Shop();
		}
	}


	class Player
	{
		public static int money = 0;
		public static int level = 1;
		public static int expToLevelUp => 100 + (10 * level);
		public static int exp = 0;

		public static async Task PlaytimeExperience()
		{
			while (true)
			{
				await Task.Delay(5000);
				GrantExperience(Program.r.Next(0, 100 * level / 2));
			}
		}

		public static void ExperienceBar()
		{
			int x = Console.GetCursorPosition().Left;
			int y = Console.GetCursorPosition().Top;
			Console.SetCursorPosition(10, 2);
			Program.drawTextProgressBar(exp, expToLevelUp, ConsoleColor.Cyan, "", false);
			Console.SetCursorPosition(15, 3);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"${money}");
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(x, y);
		}

		public static void GrantExperience(int expGained)
		{
			exp += expGained;
			if (exp >= expToLevelUp)
			{
				exp -= expToLevelUp;
				level++;
			}
			ExperienceBar();
		}
	}
	public class PlayerDataManagement
	{
		private static string saveRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		private static string gamesDirectory = saveRoot + @"\Consoler";
		private static string finalDirectory = gamesDirectory + @"\save.json";
		public static void Save()
		{
			if (Directory.Exists(gamesDirectory))
			{
				if (File.Exists(finalDirectory))
				{
					Dictionary<string, int> data = new();
					data.Add("money", Player.money);

					Dictionary<string, int> playerLevel = new();
					data.Add("level", Player.level);

					Dictionary<string, int> playerExperience = new();
					data.Add("experience", Player.exp);

					var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

					File.WriteAllText(finalDirectory, jsonData);
					Console.WriteLine("Data saved to {0}",finalDirectory);
				}
				else
				{
					File.Create(finalDirectory);
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
			if(File.Exists(finalDirectory))
			{
				string rawSaveData = File.ReadAllText(finalDirectory);
				var jsonRoot = JToken.Parse(rawSaveData);

				Player.level = (int)jsonRoot.SelectToken("level");
				Player.exp = (int)jsonRoot.SelectToken("experience");
				Player.money = (int)jsonRoot.SelectToken("money");

				Console.WriteLine("Data loaded.");
			}
		}
	}
}