using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoIRCDownloader
{
	public class AID
	{
		public static void Main (string[] args)
		{
			ServicePointManager.ServerCertificateValidationCallback = (p1, p2, p3, p4) => true;




		

			readRSS (getSettings("settings.ini"));


			Console.ReadLine();
		}

		public static string rss_data = string.Empty;
		public static string Previous_Aname = string.Empty;
		public static string botname = string.Empty;

		public static void readRSS(string url){

			var webClient = new WebClient();
			while (true) {
				ProcessRSS (webClient.DownloadString (url));
				//---wait 2 seconds before restarting rss refresh
				Thread.Sleep(2000);
			}

		}

		public static string readIntel(string url){

			var webClient = new WebClient();

			string intel = webClient.DownloadString (url);
				//---wait 2 seconds before restarting rss refresh
			
			return intel;
		}

		public static string getSettings(string source){
			var configopen = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			var content = new StreamReader(configopen, Encoding.Default);

			string setcontent = content.ReadToEnd();
			string[] settings = setcontent.Split ('^');
			string[] searchset = settings[0].Split ('=');
			string search = searchset[1];
			string[] botset = settings[1].Split ('=');
			botname = botset[1];

			search = search.Replace (" ", "+");
			string url = "http://www.nyaa.se/?page=rss&term=" + search;
			Console.WriteLine ("bot is set: " + botname);
			Console.WriteLine ("url is set: " + url);

			return url;
			
		}

		public static void getConfig(string filesource, int packnum, string bot){

			//StreamReader file = File.OpenText(filesource);
			var configopen = new FileStream(filesource, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			var content = new StreamReader(configopen, Encoding.Default);

			string concontent = content.ReadToEnd();
			searchConfig(concontent, packnum, bot);

		}

		public static void searchConfig(string content, int packnumb, string bot){



			int PosOfPack = content.IndexOf ("#");
			int PosOfBot = content.IndexOf ("/msg");
			string packnuminc = content.Substring (PosOfPack);
			string botnameinc = content.Substring (PosOfBot + 4);
			int PosOfPackEnd = packnuminc.IndexOf(";");
			int PosOfBotEnd = botnameinc.IndexOf("xdcc");
			string packnum = packnuminc.Substring(0, PosOfPackEnd);
			string botname = botnameinc.Substring(0, PosOfBotEnd);
			//Console.WriteLine ("packnum start: " + packnum);
			//Console.WriteLine("botname is: " + botname);

			replaceConfig(content, PosOfPack, PosOfPackEnd, packnumb, PosOfBot + 4, PosOfBotEnd, bot, packnum);



		}

		public static void replaceConfig(string oldcontent, int packposstart, int packpostend, int packnumnew, int PosOfBot, int PosOfBotEnd, string bot, string oldpacknum){

			int packnumnewint = 0;
			if (check == "false") {
				packnumnewint = packnumnew - 1;
			} else {
				packnumnewint = packnumnew; 
			}
			oldpacknum = oldpacknum.Substring(1);
			int packnumoldint = Convert.ToInt32(oldpacknum.Trim());
			string packnumnews = packnumnewint.ToString();



			var newConfig = new StringBuilder(oldcontent);
			newConfig.Remove(packposstart, packpostend);
			newConfig.Insert(packposstart, "#" + packnumnews);
			newConfig.Remove(PosOfBot, PosOfBotEnd);
			newConfig.Insert(PosOfBot, " " + bot + " ");
			string newConfigString = newConfig.ToString();

			if(packnumnewint == packnumoldint || packnumnewint < packnumoldint){
				Console.WriteLine("pack stayed same as previous, no updated on config needed, irssi wont launch " + oldpacknum);


			} else {


				System.IO.File.WriteAllText ("config", newConfigString);

				//Console.WriteLine(oldcontent);
				//Console.WriteLine("________________________________");
				Console.WriteLine("packnum changed, config will be updated, irssi will launch when coded " + oldpacknum);
				StartIRC();

			}
		}

		public static string check = "false";

		public static void ProcessRSS(string data){


			//--- getting rss data from readRSS()
			string rss = data;
			//--- starting to search for searchparam in rss 
			int Start_Position = rss.IndexOf ("<item>");
			//--- cutting eveything away till pos of first position of searchparam
			string item_start = rss.Substring (Start_Position + 6);
			//---starting to search for searchparam in item_start <- important
			int End_Position = item_start.IndexOf ("</item>");
			//---complete search, put result in string item_complete
			string item_complete = item_start.Substring (0, End_Position);
			//---starting to search for searchparam in item_complete
			int Start_Pos_Aname = item_complete.IndexOf ("<title>");
			//---cutting everything away till pos of Start_Pos_Aname;
			string Aname_start = item_complete.Substring (Start_Pos_Aname + 7);
			//---starting to search for searchparam in Aname_start <- important!
			int End_Pos_Aname = Aname_start.IndexOf ("</title>");
			//---complete search, put result into Aname
			string Aname = Aname_start.Substring (0, End_Pos_Aname);
			//---saving item into rss_data, shared string
			rss_data = Aname;

			//---checking if Aname is already used
			if (Aname == Previous_Aname) {
				Console.WriteLine ("Previous Aname Is the same, continueing search");
			} else {
				Console.WriteLine ("Anime Name = " + Aname);
				Console.WriteLine ("Start search for anime pack");
				LatestRelease ();
			}

			//---setting previous aname to current aname
			Previous_Aname = Aname;

		}

		public static void LatestRelease(){

			string intel = readIntel ("http://intel.haruhichan.com/?b=92");
			string packnum = "0";
			//---getting rss data here
			string aname = rss_data;
			check = "false";
			//---end of getting rss data

			int PosOfPack = intel.IndexOf (aname);
			string intelwpack = intel.Substring (PosOfPack);
			intelwpack = intelwpack.Substring(0, 300);
			int PosOfPackNum = intelwpack.IndexOf ("</td><td>") + 9;
			string packnumunref = intelwpack.Substring (PosOfPackNum);
			int PosOfPackNumEnd = packnumunref.IndexOf ("</td><td>");
			if (PosOfPackNumEnd < 1) {

				intelwpack = intel.Substring (PosOfPack - 100);
				intelwpack = intelwpack.Substring(0, 300);
				PosOfPackNum = intelwpack.IndexOf ("</td><td>") + 9;
				packnumunref = intelwpack.Substring (PosOfPackNum);
				PosOfPackNumEnd = packnumunref.IndexOf ("</td><td>");
				packnum = packnumunref.Substring (0, PosOfPackNumEnd);
				check = "true";

			} else {

				packnum = packnumunref.Substring (0, PosOfPackNumEnd);
				check = "false";
			}

			int packnumber = Convert.ToInt32 (packnum);

			Console.WriteLine (packnum);
			getConfig ("config", packnumber, botname);

		}

		public static void StartIRC(){
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.EnableRaisingEvents=false; 
			proc.StartInfo.FileName = "irssi";
			proc.Start();
			System.Threading.Thread.Sleep(300000);
			proc.Kill();
		}


	}
}

