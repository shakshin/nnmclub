#NoNaMe Club torrent tracker client

This utility designed for automated download torrent files from NoNaMe Club tracker.
It can download .torrent files for list of monitored topics when it is updated. This
is usefull for serials as example.

Usage:
  nnmclub.exe <command> [arguments]            
  
Commands:

    help    - this help screen
    passkey - set/show your passkey
    folder  - set/show download folder
    topic   - control your topics list
    run     - do poll procedure

Passkey command:

    With no arguments passed will show your configured passkey.
    With one argument passed will set your passkey value to specified string.

Folder command:

    With no arguments passed will show configured download folder path.
    With one argument passed will set download folder to specified path.

Topic command:

    Controls monitored topics list. Subcommands:
    
        list   - displays list of monitored topics
        add    - add topic to list. Topic id has to be specified as argument
        delete - removes topic from list. Topic id has to be specified as argument
    
Run command:

    Do main functionality:
    
        - fetch RSS feed from tracker
        - search fetched RSS feed for monitored topics by ids
        - download torrrent files for matched topics

