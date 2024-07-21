using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SteamAccountFinder;

namespace SteamAccountsConsole
{
	internal static class Program
	{
		static string reset = "\x1b[0m";
		static ulong activeSteamAccount = SteamAccounts.ActiveSteamAccount;
		static ulong runningGame = SteamAccounts.RunningGame;
		static SteamUsersList allAccounts = SteamAccounts.SteamUsers;
		static List<ulong> steamIds = allAccounts.GetSteamIds();
		static ulong[] installedApps = SteamAccounts.InstalledApps;


		static void Main(string[] args)
		{

			// Determine encoding
			if (Console.OutputEncoding.CodePage != 65001)
			{
				Console.OutputEncoding = Encoding.UTF8;
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

			PrintBanner();

			PrintSteamAccounts();

			PrintInstalledApps();

			PrintFingerprints();



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
			if (account.SourcesCount > 4)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			else if (account.SourcesCount > 2)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}
			Console.Write(account.SourcesCount);
			if (account.LatestSource != DateTime.MinValue && account.LatestSource  != null)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(" (" + account.LatestSource.ToString("yyyy-MM-dd HH:mm:ss") + ")");
			}
			Console.WriteLine();
			}
		}

		public static void PrintInstalledApps()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("\nInstalled Apps:");
			Console.ForegroundColor = ConsoleColor.White;
			
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

		public static void PrintFingerprints()
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("\n\nFingerprints");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("System: ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(SteamAccounts.SystemHash);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("SteamUsers: ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(SteamAccounts.SteamUsersHash);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("SteamApps: ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(SteamAccounts.SteamApplicationsHash);
		}

		public static void PrintBanner()
		{
			string banner = @"
[38;2;0;255;255m █[38;2;2;253;253m█[38;2;4;251;252m█[38;2;6;250;251m█[38;2;8;248;250m█[38;2;10;247;248m█[38;2;13;245;247m█[38;2;15;244;246m╗[38;2;17;242;245m█[38;2;19;241;243m█[38;2;21;239;242m█[38;2;24;238;241m█[38;2;26;236;240m█[38;2;28;235;238m█[38;2;30;233;237m█[38;2;32;232;236m█[38;2;35;230;235m╗[38;2;37;229;233m█[38;2;39;227;232m█[38;2;41;226;231m█[38;2;43;224;230m█[38;2;46;223;228m█[38;2;48;221;227m█[38;2;50;220;226m█[38;2;52;218;225m╗ [38;2;57;215;222m█[38;2;59;214;221m█[38;2;61;212;220m█[38;2;63;211;218m█[38;2;65;209;217m█[38;2;68;208;216m╗ [38;2;72;205;213m█[38;2;74;203;212m█[38;2;76;202;211m█[38;2;79;200;210m╗   [38;2;87;194;205m█[38;2;90;193;203m█[38;2;92;191;202m█[38;2;94;190;201m╗     [38;2;107;181;193m█[38;2;109;179;192m█[38;2;112;178;191m█[38;2;114;176;190m█[38;2;116;175;188m█[38;2;118;173;187m╗  [38;2;125;169;183m█[38;2;127;167;182m█[38;2;129;165;181m█[38;2;131;164;180m█[38;2;134;162;178m█[38;2;136;161;177m█[38;2;138;159;176m╗ [38;2;142;156;173m█[38;2;145;155;172m█[38;2;147;153;171m█[38;2;149;152;170m█[38;2;151;150;168m█[38;2;153;149;167m█[38;2;156;147;166m╗ [38;2;160;144;163m█[38;2;162;143;162m█[38;2;164;141;161m█[38;2;167;140;160m█[38;2;169;138;158m█[38;2;171;137;157m█[38;2;173;135;156m╗ [38;2;178;132;153m█[38;2;180;131;152m█[38;2;182;129;151m╗   [38;2;191;123;146m█[38;2;193;122;145m█[38;2;195;120;143m╗[38;2;197;119;142m█[38;2;200;117;141m█[38;2;202;116;140m█[38;2;204;114;138m╗   [38;2;213;108;133m█[38;2;215;107;132m█[38;2;217;105;131m╗[38;2;219;104;130m█[38;2;222;102;128m█[38;2;224;101;127m█[38;2;226;99;126m█[38;2;228;98;125m█[38;2;230;96;123m█[38;2;233;95;122m█[38;2;235;93;121m█[38;2;237;92;120m╗[38;2;239;90;118m█[38;2;241;89;117m█[38;2;244;87;116m█[38;2;246;86;115m█[38;2;248;84;113m█[38;2;250;83;112m█[38;2;252;81;111m█[38;2;255;80;110m╗[0m
[38;2;18;242;244m ██╔════╝╚[38;2;19;241;243m═[38;2;21;239;242m═[38;2;24;238;241m█[38;2;26;236;240m█[38;2;28;235;238m╔[38;2;30;233;237m═[38;2;32;232;236m═[38;2;35;230;235m╝[38;2;37;229;233m█[38;2;39;227;232m█[38;2;41;226;231m╔[38;2;43;224;230m═[38;2;46;223;228m═[38;2;48;221;227m═[38;2;50;220;226m═[38;2;52;218;225m╝[38;2;54;217;223m█[38;2;57;215;222m█[38;2;59;214;221m╔[38;2;61;212;220m═[38;2;63;211;218m═[38;2;65;209;217m█[38;2;68;208;216m█[38;2;70;206;215m╗[38;2;72;205;213m█[38;2;74;203;212m█[38;2;76;202;211m█[38;2;79;200;210m█[38;2;81;199;208m╗ [38;2;85;196;206m█[38;2;87;194;205m█[38;2;90;193;203m█[38;2;92;191;202m█[38;2;94;190;201m║    [38;2;105;182;195m█[38;2;107;181;193m█[38;2;109;179;192m╔[38;2;112;178;191m═[38;2;114;176;190m═[38;2;116;175;188m█[38;2;118;173;187m█[38;2;120;172;186m╗[38;2;123;170;185m█[38;2;125;169;183m█[38;2;127;167;182m╔[38;2;129;165;181m═[38;2;131;164;180m═[38;2;134;162;178m═[38;2;136;161;177m═[38;2;138;159;176m╝[38;2;140;158;175m█[38;2;142;156;173m█[38;2;145;155;172m╔[38;2;147;153;171m═[38;2;149;152;170m═[38;2;151;150;168m═[38;2;153;149;167m═[38;2;156;147;166m╝[38;2;158;146;165m█[38;2;160;144;163m█[38;2;162;143;162m╔[38;2;164;141;161m═[38;2;167;140;160m═[38;2;169;138;158m═[38;2;171;137;157m█[38;2;173;135;156m█[38;2;175;134;155m╗[38;2;178;132;153m█[38;2;180;131;152m█[38;2;182;129;151m║   [38;2;191;123;146m█[38;2;193;122;145m█[38;2;195;120;143m║[38;2;197;119;142m█[38;2;200;117;141m█[38;2;202;116;140m█[38;2;204;114;138m█[38;2;206;113;137m╗  [38;2;213;108;133m█[38;2;215;107;132m█[38;2;217;105;131m║[38;2;219;104;130m╚[38;2;222;102;128m═[38;2;224;101;127m═[38;2;226;99;126m█[38;2;228;98;125m█[38;2;230;96;123m╔[38;2;233;95;122m═[38;2;235;93;121m═[38;2;237;92;120m╝[38;2;239;90;118m█[38;2;241;89;117m█[38;2;244;87;116m╔[38;2;246;86;115m═[38;2;248;84;113m═[38;2;250;83;112m═[38;2;252;81;111m═[38;2;255;80;110m╝[0m
[38;2;36;230;234m ███████╗   ██║   [38;2;37;229;233m█[38;2;39;227;232m█[38;2;41;226;231m█[38;2;43;224;230m█[38;2;46;223;228m█[38;2;48;221;227m╗  [38;2;54;217;223m█[38;2;57;215;222m█[38;2;59;214;221m█[38;2;61;212;220m█[38;2;63;211;218m█[38;2;65;209;217m█[38;2;68;208;216m█[38;2;70;206;215m║[38;2;72;205;213m█[38;2;74;203;212m█[38;2;76;202;211m╔[38;2;79;200;210m█[38;2;81;199;208m█[38;2;83;197;207m█[38;2;85;196;206m█[38;2;87;194;205m╔[38;2;90;193;203m█[38;2;92;191;202m█[38;2;94;190;201m║    [38;2;105;182;195m█[38;2;107;181;193m█[38;2;109;179;192m█[38;2;112;178;191m█[38;2;114;176;190m█[38;2;116;175;188m█[38;2;118;173;187m█[38;2;120;172;186m║[38;2;123;170;185m█[38;2;125;169;183m█[38;2;127;167;182m║     [38;2;140;158;175m█[38;2;142;156;173m█[38;2;145;155;172m║     [38;2;158;146;165m█[38;2;160;144;163m█[38;2;162;143;162m║   [38;2;171;137;157m█[38;2;173;135;156m█[38;2;175;134;155m║[38;2;178;132;153m█[38;2;180;131;152m█[38;2;182;129;151m║   [38;2;191;123;146m█[38;2;193;122;145m█[38;2;195;120;143m║[38;2;197;119;142m█[38;2;200;117;141m█[38;2;202;116;140m╔[38;2;204;114;138m█[38;2;206;113;137m█[38;2;208;111;136m╗ [38;2;213;108;133m█[38;2;215;107;132m█[38;2;217;105;131m║   [38;2;226;99;126m█[38;2;228;98;125m█[38;2;230;96;123m║   [38;2;239;90;118m█[38;2;241;89;117m█[38;2;244;87;116m█[38;2;246;86;115m█[38;2;248;84;113m█[38;2;250;83;112m█[38;2;252;81;111m█[38;2;255;80;110m╗[0m
[38;2;54;217;223m ╚════██║   ██║   ██╔══╝  █[38;2;57;215;222m█[38;2;59;214;221m╔[38;2;61;212;220m═[38;2;63;211;218m═[38;2;65;209;217m█[38;2;68;208;216m█[38;2;70;206;215m║[38;2;72;205;213m█[38;2;74;203;212m█[38;2;76;202;211m║[38;2;79;200;210m╚[38;2;81;199;208m█[38;2;83;197;207m█[38;2;85;196;206m╔[38;2;87;194;205m╝[38;2;90;193;203m█[38;2;92;191;202m█[38;2;94;190;201m║    [38;2;105;182;195m█[38;2;107;181;193m█[38;2;109;179;192m╔[38;2;112;178;191m═[38;2;114;176;190m═[38;2;116;175;188m█[38;2;118;173;187m█[38;2;120;172;186m║[38;2;123;170;185m█[38;2;125;169;183m█[38;2;127;167;182m║     [38;2;140;158;175m█[38;2;142;156;173m█[38;2;145;155;172m║     [38;2;158;146;165m█[38;2;160;144;163m█[38;2;162;143;162m║   [38;2;171;137;157m█[38;2;173;135;156m█[38;2;175;134;155m║[38;2;178;132;153m█[38;2;180;131;152m█[38;2;182;129;151m║   [38;2;191;123;146m█[38;2;193;122;145m█[38;2;195;120;143m║[38;2;197;119;142m█[38;2;200;117;141m█[38;2;202;116;140m║[38;2;204;114;138m╚[38;2;206;113;137m█[38;2;208;111;136m█[38;2;211;110;135m╗[38;2;213;108;133m█[38;2;215;107;132m█[38;2;217;105;131m║   [38;2;226;99;126m█[38;2;228;98;125m█[38;2;230;96;123m║   [38;2;239;90;118m╚[38;2;241;89;117m═[38;2;244;87;116m═[38;2;246;86;115m═[38;2;248;84;113m═[38;2;250;83;112m█[38;2;252;81;111m█[38;2;255;80;110m║[0m
[38;2;72;205;213m ███████║   ██║   ███████╗██║  ██║█[38;2;74;203;212m█[38;2;76;202;211m║ [38;2;81;199;208m╚[38;2;83;197;207m═[38;2;85;196;206m╝ [38;2;90;193;203m█[38;2;92;191;202m█[38;2;94;190;201m║    [38;2;105;182;195m█[38;2;107;181;193m█[38;2;109;179;192m║  [38;2;116;175;188m█[38;2;118;173;187m█[38;2;120;172;186m║[38;2;123;170;185m╚[38;2;125;169;183m█[38;2;127;167;182m█[38;2;129;165;181m█[38;2;131;164;180m█[38;2;134;162;178m█[38;2;136;161;177m█[38;2;138;159;176m╗[38;2;140;158;175m╚[38;2;142;156;173m█[38;2;145;155;172m█[38;2;147;153;171m█[38;2;149;152;170m█[38;2;151;150;168m█[38;2;153;149;167m█[38;2;156;147;166m╗[38;2;158;146;165m╚[38;2;160;144;163m█[38;2;162;143;162m█[38;2;164;141;161m█[38;2;167;140;160m█[38;2;169;138;158m█[38;2;171;137;157m█[38;2;173;135;156m╔[38;2;175;134;155m╝[38;2;178;132;153m╚[38;2;180;131;152m█[38;2;182;129;151m█[38;2;184;128;150m█[38;2;186;126;148m█[38;2;189;125;147m█[38;2;191;123;146m█[38;2;193;122;145m╔[38;2;195;120;143m╝[38;2;197;119;142m█[38;2;200;117;141m█[38;2;202;116;140m║ [38;2;206;113;137m╚[38;2;208;111;136m█[38;2;211;110;135m█[38;2;213;108;133m█[38;2;215;107;132m█[38;2;217;105;131m║   [38;2;226;99;126m█[38;2;228;98;125m█[38;2;230;96;123m║   [38;2;239;90;118m█[38;2;241;89;117m█[38;2;244;87;116m█[38;2;246;86;115m█[38;2;248;84;113m█[38;2;250;83;112m█[38;2;252;81;111m█[38;2;255;80;110m║[0m
[38;2;91;192;203m ╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚═╝     ╚[38;2;92;191;202m═[38;2;94;190;201m╝    [38;2;105;182;195m╚[38;2;107;181;193m═[38;2;109;179;192m╝  [38;2;116;175;188m╚[38;2;118;173;187m═[38;2;120;172;186m╝ [38;2;125;169;183m╚[38;2;127;167;182m═[38;2;129;165;181m═[38;2;131;164;180m═[38;2;134;162;178m═[38;2;136;161;177m═[38;2;138;159;176m╝ [38;2;142;156;173m╚[38;2;145;155;172m═[38;2;147;153;171m═[38;2;149;152;170m═[38;2;151;150;168m═[38;2;153;149;167m═[38;2;156;147;166m╝ [38;2;160;144;163m╚[38;2;162;143;162m═[38;2;164;141;161m═[38;2;167;140;160m═[38;2;169;138;158m═[38;2;171;137;157m═[38;2;173;135;156m╝  [38;2;180;131;152m╚[38;2;182;129;151m═[38;2;184;128;150m═[38;2;186;126;148m═[38;2;189;125;147m═[38;2;191;123;146m═[38;2;193;122;145m╝ [38;2;197;119;142m╚[38;2;200;117;141m═[38;2;202;116;140m╝  [38;2;208;111;136m╚[38;2;211;110;135m═[38;2;213;108;133m═[38;2;215;107;132m═[38;2;217;105;131m╝   [38;2;226;99;126m╚[38;2;228;98;125m═[38;2;230;96;123m╝   [38;2;239;90;118m╚[38;2;241;89;117m═[38;2;244;87;116m═[38;2;246;86;115m═[38;2;248;84;113m═[38;2;250;83;112m═[38;2;252;81;111m═[38;2;255;80;110m╝[0m
                                                                                                                     [0m
[38;2;127;167;182m ███████╗██╗███╗   ██╗██[38;2;133;163;179m█[38;2;139;159;175m█[38;2;144;155;172m█[38;2;150;151;169m█[38;2;156;147;166m╗ [38;2;168;139;159m█[38;2;173;135;156m█[38;2;179;131;152m█[38;2;185;127;149m█[38;2;191;123;146m█[38;2;197;119;142m█[38;2;202;115;139m█[38;2;208;111;136m╗[38;2;214;107;133m█[38;2;220;103;129m█[38;2;226;99;126m█[38;2;231;95;123m█[38;2;237;91;119m█[38;2;243;87;116m█[38;2;249;83;113m╗ [0m
[38;2;145;155;172m ██╔════╝██║████╗  ██║██╔══[38;2;150;151;169m█[38;2;156;147;166m█[38;2;162;143;162m╗[38;2;168;139;159m█[38;2;173;135;156m█[38;2;179;131;152m╔[38;2;185;127;149m═[38;2;191;123;146m═[38;2;197;119;142m═[38;2;202;115;139m═[38;2;208;111;136m╝[38;2;214;107;133m█[38;2;220;103;129m█[38;2;226;99;126m╔[38;2;231;95;123m═[38;2;237;91;119m═[38;2;243;87;116m█[38;2;249;83;113m█[38;2;255;80;110m╗[0m
[38;2;163;142;161m █████╗  ██║██╔██╗ ██║██║  ██║[38;2;168;139;159m█[38;2;173;135;156m█[38;2;179;131;152m█[38;2;185;127;149m█[38;2;191;123;146m█[38;2;197;119;142m╗  [38;2;214;107;133m█[38;2;220;103;129m█[38;2;226;99;126m█[38;2;231;95;123m█[38;2;237;91;119m█[38;2;243;87;116m█[38;2;249;83;113m╔[38;2;255;80;110m╝[0m
[38;2;182;130;151m ██╔══╝  ██║██║╚██╗██║██║  ██║██╔[38;2;185;127;149m═[38;2;191;123;146m═[38;2;197;119;142m╝  [38;2;214;107;133m█[38;2;220;103;129m█[38;2;226;99;126m╔[38;2;231;95;123m═[38;2;237;91;119m═[38;2;243;87;116m█[38;2;249;83;113m█[38;2;255;80;110m╗[0m
[38;2;200;117;141m ██║     ██║██║ ╚████║██████╔╝██████[38;2;202;115;139m█[38;2;208;111;136m╗[38;2;214;107;133m█[38;2;220;103;129m█[38;2;226;99;126m║  [38;2;243;87;116m█[38;2;249;83;113m█[38;2;255;80;110m║[0m
[38;2;218;105;130m ╚═╝     ╚═╝╚═╝  ╚═══╝╚═════╝ ╚══════╝╚[38;2;220;103;129m═[38;2;226;99;126m╝  [38;2;243;87;116m╚[38;2;249;83;113m═[38;2;255;80;110m╝[0m
[0m";
			//if (TerminalApi.EnableVirtualTerminalProcessing())
			//{
			Console.WriteLine(banner);
			//}

			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write("Created by:\t");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("insoulglobal");

			string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write("Version:\t");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(version);

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();
		}


		// This triggering VirusTotal for some reason
		//public static class TerminalApi
		//{
		//	private const int STD_OUTPUT_HANDLE = -11;
		//	private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

		//	[DllImport("kernel32.dll", SetLastError = true)]
		//	private static extern IntPtr GetStdHandle(int nStdHandle);

		//	[DllImport("kernel32.dll", SetLastError = true)]
		//	private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

		//	[DllImport("kernel32.dll", SetLastError = true)]
		//	private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

		//	public static bool EnableVirtualTerminalProcessing()
		//	{
		//		IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);

		//		if (!GetConsoleMode(handle, out uint mode))
		//			return false;

		//		mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;

		//		if (!SetConsoleMode(handle, mode))
		//			return false;

		//		Console.OutputEncoding = Encoding.UTF8;
		//		return true;
		//	}
		//}
	}
}