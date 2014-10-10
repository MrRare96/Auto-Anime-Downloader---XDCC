Auto-Anime-Downloader---XDCC
============================

Automated anime downloader using irc xdcc instead of torrents :)

THIS IS LINUX ONLY FOR NOW!!!
Yep, I made this mainly for the raspberry pi.
So, before this works you will have to set things up correctly:

1. sudo apt-get install mono-complete.

2. sudo apt-get install irssi.

3. run irssi by typing in terminal: irssi.

4. close it by typing /quit.

5. open /home/(user) and make sure that you can see hidden folders.

6. locate .irssi folder and open it.

7. put AutoAnimeDowloader.exe, config and settings.ini in it.

8. change nickname etc in config file to your liking, do not use the one provided with it, since it might be in use.

9. open settings.ini and change the search parameter to your liking, you can add multiple anime's by seperating them with a ",".
(DONT DELETE THE "^"). 

Search Examples: 

Horriblesubs 720p <- downloads all new 720p episodes from horriblesubs

Horriblesubs Psycho-Pass 2 720p, Horriblesubs Shirobako 1080p <-downloads all new psycho pass 2 eps in 720p, 
and all new shirobako 1080p episodes.

(Only works for horriblesubs for now, since the bot used in this case, only contains horriblesubs episodes)

10. start AutoAnimeDownloader.exe by typing cd /home/(user)/.irssi, or if you are already in home: cd /.irssi 
and then mono AutoAnimeDownloader.exe

11. now it works, your files will be dlld to you home/(user) folder, option to change this will be added in the future

12. to run it 24/7, you will have to lauch it from the raspberry pi itself, if you use ssh it will
stop the script when you close the ssh connection.

YOUR DONE!


Future functions:

Change Download Location 

Change Bot

In the far, far away future mal implementation, since its logically that once downloaded you watch it.

Auto remove function(like delete in 30 days);

How it works:

Scans first item on nyaa's rss, search on intel.haruhichan.com on the bot page for a pack containing the 
anime name from the rss feed, open irssi and let it automaticly join #intel and automaticly run command 
/msg [bot] xdcc send #packnumber and it starts downloading.

--- 0.1 released (first version count)
added multiple anime subscription support
possible fix for crash when anime is not yet uploaded on intel.haruhichan

known problems:
While I tested it, it crashed again because of the fix above, atleast that is what I think, it was at night and i forgot to save the ssh log.
