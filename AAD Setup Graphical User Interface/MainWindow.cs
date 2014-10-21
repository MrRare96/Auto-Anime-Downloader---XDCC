using System;
using Gtk;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		AnimeListed.Buffer.Text = "Welcome to the setup gui for your automated anime downloader!" + "\n" + "\n" + "This is in pre-pre, maybe even more pre alpha!" + "\n" + "\n" + "There are a few things you should know:" + "\n" + "\n" +
			"1. You will need to open or create a new settings file." + "\n" + "When you create one, it will be saved into the same folder where this program is." + "\n" +  "2. you will have to put the config file into the same folder where the AutoAnimeDownloader.exe is." + "\n" +
			"3. After adding an anime subscription and/or adding a new DL time," + "\n" + " You need to press the save button!" + "\n" + "Your DL time should be the number that comes after this calculation: " + "\n" + "10000 / your download speed / 60 = dl time" + "\n" + "you need to round it to a hole number! for example mine is 1.85, so i need to fill in 2" +  "\n" + "\n" +
			"THIS PROGRAM IS ONLY ABLE TO CREATE THE SETTINGS.INI," + "\n" + " ITS NOT ABLE TO LAUNCH THE PROGRAM" + "\n" +  "\n" + "Maybe i forgot something, anyway:" + "\n" + "Thanks for downloading and using this!" + "\n" + "\n" + 
			"A complete tutorial can be found here:" + "\n" + "https://github.com/RareAMV/Auto-Anime-Downloader---XDCC" + "\n";

		string intelcontent = readIntel ("http://intel.haruhichan.com/");
		int posstartbotlist = intelcontent.IndexOf ("<a href=\"javascript:botPackList");
		string bothtmlcode = intelcontent.Substring (posstartbotlist);
		int posendbotlist = intelcontent.IndexOf ("</a><br /><br />");
		bothtmlcode = bothtmlcode.Substring (0, posendbotlist);

		string[] boturl = explode ("<br />", bothtmlcode);
		Logging.Text = boturl [3];
		int boturlx = boturl.Length;

		int x = 0;
		int y = 0;


		while (x != boturlx) {

			try{
				string botnameunref = boturl [x];
				int posstartboturlnum = botnameunref.IndexOf (":");
				string boturlnum = botnameunref.Substring (posstartboturlnum);
				int posendboturlnum = botnameunref.IndexOf (";");
				boturlnum = boturlnum.Substring (0, posendboturlnum);

				int posofstartnum = boturlnum.IndexOf("(");
				boturlnum = boturlnum.Substring(posofstartnum);
				int posofendnum = boturlnum.IndexOf(")");
				boturlnum = boturlnum.Substring(1, posofendnum - 1);

				int posstartname = botnameunref.IndexOf(";\">");
				string botname = botnameunref.Substring(posstartname);
				int posendname = botname.IndexOf("</a");
				botname = botname.Substring(3, posendname - 3);
				botnames.Add(botname);
				botnums.Add(boturlnum);
			
				botlistbox.AppendText (global::Mono.Unix.Catalog.GetString (botname));
			} catch {
				//botlistbox.AppendText (global::Mono.Unix.Catalog.GetString ("SOMETHING WENT WRONG ON " + x.ToString()));
				y++;
				if (y == 5) {
					break;
				}
			}
			x++;

		}

	
	}

	public static List<string> botnames = new List<string>();
	public static List<string> botnums = new List<string>();

	public static string[] explode(string separator, string source) {

		return source.Split (new string[] { separator }, StringSplitOptions.None);

	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
		
	public static StringBuilder names = new StringBuilder ();

	public static int i = 0;

	protected void AddToSub (object sender, EventArgs e)
	{

		string botnumber = String.Empty;
		string bot = botlistbox.ActiveText;
		bot = bot.Substring (0, bot.IndexOf("(") - 1);
		int listlength = botnames.Count;
		int y = 0;
		while (y != listlength) {

			if (botnames [y].Contains (bot)) {
				botnumber = botnums [y];
				break;
			}
			y++;
		}



		if (i > 0 || location == "" || location == null) {
			AnimeListed.Buffer.Text = "New Animes added:" + " subscribed using bot: " + bot + "\n";
			names.Append (AnimeEntry.Text + "@" + bot +"@" + botnumber + "\n");
		} else {
			int arrlength = knownanimes.Length;
			int x = 0;
			AnimeListed.Buffer.Text = "Currently Subscribed to:" + "\n";

			while (x != arrlength) {
				if (knownanimes [x].Contains("p")) {

					names.Append (knownanimes [x] + "\n");
				} else {
					Logging.Text = "stuff is empty, ignore this its fine!";
				}
				x++;
			}

		}

		AnimeListed.Buffer.Text = names + "\n";	
		

		Logging.Text = "Anime added to subscriptions: " + AnimeEntry.Text + " botnumber = " + botnumber;
		i++;

	}
		




	public static string[] knownanimes;
	public static string settingcontent = String.Empty;
	public static string location;
	public static int pos_ofnames = 0;
	public static string[] settings;
	public bool newdltime = false;
	public static string dltimenew = String.Empty;
	public static string dltimeold = String.Empty;

	protected void OnOpenSettingsClicked (object sender, EventArgs e)
	{
		location = settingsloc.Text;
		Logging.Text = "Location: " + location;

		try{
		var configopen = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		var content = new StreamReader(configopen, Encoding.Default);
		string setcontent = content.ReadToEnd ();
		settingcontent = setcontent;

		pos_ofnames = setcontent.IndexOf ("^");

		setcontent = setcontent.Replace(Environment.NewLine, "");
		string[] settings = setcontent.Split ('^');
		string[] searchset = settings[0].Split ('=');
		string search = searchset[1];
		knownanimes = search.Split (',');
		} catch {
			Logging.Text = "Location is not a valid location or settings.ini does not exists.";
		}


	}

	protected void OnSaveClicked (object sender, EventArgs e)
	{
		if (location == "" || location == null) {

			Logging.Text = "You need to open a settings.ini file first!";

		} else {
			string anames = names.ToString ().Replace ("\n", ",");
			int anameslength = anames.Length;
			anames = anames.Substring (0, anameslength - 1);
			pos_ofnames = settingcontent.IndexOf ("^")  - 7;
			var newConfig = new StringBuilder (settingcontent);
			newConfig.Remove (7, pos_ofnames);
			newConfig.Insert (7, anames);
			string newConfigString = newConfig.ToString ();

			if (newdltime == true) {
				AnimeListed.Buffer.Text = dltimenew + "\n";	
				int posoftime = newConfigString.IndexOf ("runtime");
				string tempcon = newConfigString.Substring (posoftime);
				int lengthoftime = tempcon.Length;

				var newConfig2 = new StringBuilder (newConfigString);
				newConfig2.Remove (posoftime, lengthoftime);
				newConfig2.Insert (posoftime, dltimenew);
				newConfigString = newConfig2.ToString ();
				newConfigString = newConfigString + " \n ^ \nirssiloc = ";


			}
			if (newirssiloc == true) {

				int posofirssiloc = newConfigString.IndexOf ("irssiloc");
				string tempcon = newConfigString.Substring (posofirssiloc);
				int lengthofirssiloc = tempcon.Length;

				var newConfig3 = new StringBuilder (newConfigString);
				newConfig3.Remove (posofirssiloc, lengthofirssiloc);
				newConfig3.Insert (posofirssiloc , irssiloc);
				newConfigString = newConfig3.ToString ();
			}
			System.IO.File.WriteAllText (location, newConfigString);
			Logging.Text = "Settings file saved to: " + location;
		}
	}

	protected void OnCreatesettingfileClicked (object sender, EventArgs e)
	{
		string anames = names.ToString().Replace ("\n",",");
		int anameslength = anames.Length;
		if (anameslength > 1) {
			anames = anames.Substring (0, anameslength - 1);
		} 

		string settingstring = "search=" + anames + "\n ^ \nruntime = 0 \n ^ \nirssiloc = " + irssiloc;
		location = "settings.ini";
		//File.Create (location);
		System.IO.File.WriteAllText (location, settingstring);
		Logging.Text = "Settings file created/ready for editting(its in the folder where this app is)";

		try{
			var configopen = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			var content = new StreamReader(configopen, Encoding.Default);
			string setcontent = content.ReadToEnd ();
			settingcontent = setcontent;

			pos_ofnames = setcontent.IndexOf ("^");

			setcontent = setcontent.Replace(Environment.NewLine, "");
			settings = setcontent.Split ('^');
			string[] searchset = settings[0].Split ('=');
			string search = searchset[1];
			knownanimes = search.Split (',');
		} catch {
			Logging.Text = "Location is not a valid location or settings.ini does not exists.";
		}

	}

	protected void OnDltimesetClicked (object sender, EventArgs e)
	{
		try{
			string[] dltimeset = settings[1].Split ('=');
			string dltimecurrent = dltimeset [1];
			dltimeold = "runtime = " + dltimecurrent;



			string dltimestr = dltime.Text;
			int dltimeint = 0;
	

			dltimeint = Convert.ToInt32(dltimestr);
			dltimenew = "runtime = " + dltimestr;
			Logging.Text = "new dltime = " +  dltimenew;
			newdltime = true;


		} catch {
			Logging.Text = "you did not use a number, this value should be a number or you did not open settings.ini";
			newdltime = false;
		} 




	}




	public static string readIntel(string url){


		var webClient = new WebClient();

		string intel = webClient.DownloadString (url);


		return intel;
	}


	public static string irssiloc = String.Empty;
	public static bool newirssiloc = false;

	protected void OnIrssilocClicked (object sender, EventArgs e)
	{
		irssiloc = "irssiloc = " + @locationofirs.Text;
		newirssiloc = true;
	}
}
