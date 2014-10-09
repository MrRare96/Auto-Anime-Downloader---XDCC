Auto-Anime-Downloader---XDCC
============================

Automated anime downloader using irc xdcc instead of torrents :)

THIS IS LINUX ONLY FOR NOW!!!
Yep, I made this mainly for the raspberry pi.
So, before this works you will have to set things up correctly:

1. sudo apt-get install mono-complete
2. sudo apt-get install irssi
3. run irssi by typing in terminal: irssi
4. close it by typing /quit
5. open /home and make sure that you can see hidden folders
6. locate .irssi folder and open it
7. put AutoAnimeDowloader.exe, config and settings.ini in it
8. change nickname etc in config file to your liking, do not use the one provided with it, since it might be in use
9. open settings.ini and change the search string to your liking(DONT DELETE THE "^"). (for example change it to horriblesubs 720p, it will dl all upcomming horriblesubs 720p files)
10. start AutoAnimeDownloader.exe by typing cd /home/.irssi, or if you are already in home: cd /.irssi and then mono AutoAnimeDownloader.exe
11. now it works, your files will be dlld to you home folder, option to change this will be added in the future

YOUR DONE!


Future functions:
Change Download Location 
Change Bot
Add multiple anime subscription
In the far, far away future mal implementation, since its logically that once downloaded you watch it.
Auto remove function(like delete in 30 days);

How it works:

Scans first item on nyaa's rss, search on intel.haruhichan.com on the bot page for a pack containing the anime name from the rss feed, open irssi and let it automaticly join #intel and automaticly run command /msg [bot] xdcc send #packnumber and it starts downloading.

