using System;
using System.Threading;

#pragma warning disable CA1416

namespace Consoler
{
	class Program
	{

		public static string userName = Environment.UserName;
		public static List<string> StartupQuotes = new() { "The Internet? Is that thing still around?", "Chuck Norris counted to infinity... twice.", "C is quirky, flawed, and an enormous success", "Chuck Norris doesn’t go hunting. Chuck Norris goes killing.", "Hey! It compiles! Ship it!", "God is real, unless declared integer.", "It works on my machine.", "Keyboard not found...\nPress any key to continue...", "There is no place like 127.0.0.1", "There’s no test like production.", "Who is General Failure? And why is he reading my disk?", "I think we agree, the past is over.", "Linux is only free if your time has no value.", "On the Internet, nobody knows you’re a dog.", "One man’s constant is another man’s variable.", "PHP – Yeah, you know me." };

		bool isInit = true;
		public static Dictionary<string, bool> canPerformAction = new();
		private static void Init()
		{
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
			Console.SetCursorPosition(0, 0);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Thread.Sleep(750);
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.White;
			Thread.Sleep(750);
			Console.WriteLine("\n");
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Thread.Sleep(750);
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
			Thread.Sleep(750);
			Console.Clear();
		}

		public static Random r = new();

		public static void drawTextProgressBar(int progress, int total, ConsoleColor color, string message)
		{
			//draw empty progress bar
			Console.CursorLeft = 0;
			Console.Write("["); //start
			Console.CursorLeft = 32;
			Console.Write("]"); //end
			Console.CursorLeft = 1;
			float onechunk = 30.0f / total;

			//draw filled part
			int position = 1;
			for (int i = 0; i < onechunk * progress; i++)
			{
				Console.BackgroundColor = color;
				Console.CursorLeft = position++;
				Console.Write(" ");
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
		}

		public static async Task Main()
		{
			Console.WindowHeight = 50;
			Console.WindowWidth = 75;

			StartupAnimation();


			await Player.ExperienceBar();
			Console.WriteLine("\nBOOT SUCCESSFUL.\nWelcome, {0}\n\n\n", userName);

			await CommandInput();
			//await Task.Run(Player.PlaytimeExperience);
		}

		public static async Task CommandInput()
		{
			Console.Write("\n@root/user/home > ");
			string? command = Console.ReadLine().ToLower();

			switch (command)
			{
				case "work":
					if (canPerformAction["work"] == true)
					{
                        int workTime = r.Next(100, 250);
                        Console.CursorVisible = false;
                        for (int i = 0; i < workTime; i++)
                        {
                            drawTextProgressBar(i, workTime, ConsoleColor.Yellow, "Working...");
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
						Console.WriteLine("You cannot work yet!\n{0} remaining...");
					}
					break;


                case "study" or "learn":
					int studyTime = r.Next(250, 350);
					int experienceGained = r.Next(0, 50 * Player.level / 4);
					for (int i = 0; i < studyTime; i++)
					{
						drawTextProgressBar(i, studyTime, ConsoleColor.Cyan, "Studying...");
						await Task.Delay(150);
					}
					await Player.GrantExperience(experienceGained);
					break;

				case "crime":
					int chanceToGetCaught = 5;
					int rolledNumber = r.Next(0, 100);
					if(rolledNumber > chanceToGetCaught)
					{
						int moneyGained = r.Next(50, 100 * Player.level / 2);
						experienceGained = r.Next(25,100 * Player.level / 4);
						Console.WriteLine($"You commited a crime and it paid off!");
						
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"+{moneyGained}$");
						
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write($" +{experienceGained} EXP");
						Console.ForegroundColor = ConsoleColor.White;
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

				case "sus": //sussy :eyes:
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

				default:   
					Console.WriteLine("Unknown command.");
					break;
			}
			await CommandInput();
		}
	}


	class Player
	{
		public static int money = 0;
		public static int level = 1;
		public static int expToLevelUp = 100;
		public static int exp = 0;

		public static async Task PlaytimeExperience()
		{
			while (true)
			{
				await Task.Delay(5000);
				await GrantExperience(Program.r.Next(0, 100 * level / 2));
			}
		}

		public static async Task ExperienceBar()
		{
			int x = Console.GetCursorPosition().Left;
			int y = Console.GetCursorPosition().Top;
			Console.SetCursorPosition(10, 4);
			Program.drawTextProgressBar(exp, expToLevelUp, ConsoleColor.Cyan, "");
			Console.SetCursorPosition(35, 5);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"${money}");
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(x, y);
		}

		public static async Task GrantExperience(int expGained)
		{
			exp += expGained;
			if (exp >= expToLevelUp)
			{
				exp -= expToLevelUp;
				level++;
			}
			await ExperienceBar();
		}
	
		public static bool ChangeActionAvaibility(bool T)
		{
			return T = true;
		}

	}
}