using System;
using Gtk;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
		if (i > 0 || location == "" || location == null) {
			AnimeListed.Buffer.Text = "New Animes added:" + "\n";
			names.Append (AnimeEntry.Text + "\n");
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
		

		Logging.Text = "Anime added to subscriptions: ";
		i++;

	}
		


	protected void AddBot (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
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
		string settingstring = "search=" + anames + "\n ^ \nbotname=CR-NL|NEW \n ^ \nruntime = 4";
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
		string[] dltimeset = settings[2].Split ('=');
		string dltimecurrent = dltimeset [1];
		dltimeold = "runtime = " + dltimecurrent;



		string dltimestr = dltime.Text;
		int dltimeint = 0;
		try{

			dltimeint = Convert.ToInt32(dltimestr);
			dltimenew = "runtime = " + dltimestr;
			Logging.Text = "new dltime = " +  dltimenew;
			newdltime = true;


		} catch {
			Logging.Text = "you did not use a number, this value should be a number";
			newdltime = false;
		} 




	}
}
