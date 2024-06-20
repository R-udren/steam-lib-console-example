using System;
using SteamAccountFinder;

namespace SteamAccountsConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
            // Determine encoding
            if (Console.OutputEncoding.CodePage != 65001)
			{
				Console.OutputEncoding = System.Text.Encoding.UTF8;
			}

			// Capturing global exceptions
			AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
			{
				if (eventArgs.ExceptionObject is Exception exception)
				{
					string exceptionMessage = exception.Message;
					Console.WriteLine("Unhandled exception: " + exceptionMessage);
				}
				else
				{
					Console.WriteLine("Unhandled exception occurred.");
				}
			};

            PrintSteamAccounts();

            Console.ForegroundColor = ConsoleColor.White;
			Console.Write("\nPress Enter to exit...");
			Console.ReadLine();

		}

		private static void PrintSteamAccounts()
		{
			if (!SteamAccounts.IsSteamInstalled)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Steam is not installed.");
				return;
			}

			ulong activeSteamAccount = SteamAccounts.ActiveSteamAccount;
			ulong runningGame = SteamAccounts.RunningGame;
			var allAccounts = SteamAccounts.SteamUsers;
			allAccounts.SortByLatestSource(true);
			var steamIds = allAccounts.GetSteamIds();
			ulong[] installedApps = SteamAccounts.InstalledApps;


			Console.ForegroundColor = ConsoleColor.Red;
			if (SteamAccounts.IsSteamRunning)
			{
				Console.Write("Active Steam Account: ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(activeSteamAccount);
			}
			else
			{
				Console.WriteLine("Steam is not running.");
			}

			Console.ForegroundColor = ConsoleColor.Yellow;
			if (SteamAccounts.IsGameRunning)
			{
				Console.Write("Running Game: ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(runningGame);
			}
			else
			{
				Console.WriteLine("Game is not running.");
			}

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write($"\nFound {allAccounts.Count} Steam Accounts: ");

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(string.Join(", ", steamIds));

			foreach (var account in allAccounts)
			{
				if (account.Username != null)
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("\nUsername: ");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write(account.Username);
				}

				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("\nSteam ID: ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(account.SteamId);
				if (account.IsActive)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(" - Active!");
				}

				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("\nProfile URL: ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine(account.ProfileUrl);



				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("Sources Count: ");
				if (account.SourcesCount > 2)
				{
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				Console.Write(account.SourcesCount);
				if (account.LatestSource != DateTime.MinValue)
					Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(" (" + account.LatestSource.ToString("yyyy-MM-dd HH:mm:ss") + ")");

			}
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("\nInstalled Apps:");
			Console.ForegroundColor = ConsoleColor.White;
			// Draw matrix of installed apps
			int columns = 5;
			int rows = (int)Math.Ceiling((double)installedApps.Length / columns);

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					int index = i * columns + j;
					if (index < installedApps.Length)
					{
						Console.Write(installedApps[index].ToString().PadLeft(10));
					}
				}
				Console.WriteLine();
			}
		}
	}
}