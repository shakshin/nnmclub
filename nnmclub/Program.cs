﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nnmclub
{
    class Program
    {
        static Config config;
        static void Main(string[] args)
        {
            Parse(args);
            DebugLog.Close();
        }

        static void Parse(string[] args)
        {
            config = Config.Get();
            config.Save();

            if (args.Length == 0) {
                ShowHelp();
                return;
            }
            
            switch (args[0].ToLower())
            {
                case "help":
                    ShowHelp();
                    break;
                case "topic":
                    Topics(args);
                    break;
                case "loglevel":
                    LogLevel(args);
                    break;
                case "folder":
                    Folder(args);
                    break;
                case "passkey":
                    Passkey(args);
                    break;
                case "run":
                    DebugLog.WriteLine(String.Format("Processing Run command"), DebugLog.Level.Verbose);
                    if (args.Length != 1)
                    {
                        DebugLog.WriteLine(String.Format("Wrong number of arguments"), DebugLog.Level.Verbose);
                        System.Console.WriteLine("Wrong number of argumentts");
                    }
                    if (config.Passkey == String.Empty)
                    {
                        System.Console.WriteLine("Passkey is not configured");
                        DebugLog.WriteLine(String.Format("Passkey is not configured"), DebugLog.Level.Verbose);
                        return;
                    }
                    if (config.Folder == String.Empty)
                    {
                        System.Console.WriteLine("Download folder is not configured");
                        DebugLog.WriteLine(String.Format("Download folder is not configured"), DebugLog.Level.Verbose);
                        return;
                    }
                    Tracker.Poll();
                    break;
                default:
                    System.Console.WriteLine("Wrong command: {0}", args[0]);
                    break;
            }
        }

        static void LogLevel(string[] args)
        {
            if (args.Length == 1)
            {
                System.Console.WriteLine("Log level: {0}", config.LogLevel);
            }
            else if (args.Length == 2)
            {
                switch (args[1].ToLower())
                {
                    case "normal":
                        config.LogLevel = DebugLog.Level.Normal;
                        break;
                    case "verbose":
                        config.LogLevel = DebugLog.Level.Verbose;
                        break;
                    case "debug":
                        config.LogLevel = DebugLog.Level.Debug;
                        break;
                    default:
                        System.Console.WriteLine("Log level not recognized. Normal level set.");
                        config.LogLevel = DebugLog.Level.Normal;
                        break;
                }
                
                config.Save();
                System.Console.WriteLine("Log level configured");
                DebugLog.WriteLine(String.Format("Log level set to {0}", config.LogLevel), DebugLog.Level.Normal);
            }
            else
            {
                System.Console.WriteLine("Wrong number of arguments");
            }
        }

        static void Topics(string[] args)
        {
            if (args.Length == 1)
            {
                Topic.List();
            }
            else
            {
                switch (args[1])
                {
                    case "list":
                        Topic.List();
                        break;
                    case "add":
                        if (args.Length == 3)
                        {
                            int id = 0;
                            try
                            {
                                id = int.Parse(args[2]);
                            } 
                            catch (Exception)
                            {
                                System.Console.WriteLine("Wrong topic id provided");
                                return;
                            }
                            Topic.Add(id);
                        }
                        else
                        {
                            System.Console.WriteLine("Wrong number of arguments");
                        }
                        break;
                    case "delete":
                        if (args.Length == 3)
                        {
                            int id = 0;
                            try
                            {
                                id = int.Parse(args[2]);
                            } 
                            catch (Exception)
                            {
                                System.Console.WriteLine("Wrong topic id provided");
                                return;
                            }
                            Topic.Delete(id);
                        }
                        else
                        {
                            System.Console.WriteLine("Wrong number of arguments");
                        }
                        break;
                    default:
                        System.Console.WriteLine("Wrong command: {0}", args[1]);
                        break;
                }
            }
        }

        static void Passkey(string[] args)
        {
            if (args.Length == 1)
            {
                if (config.Passkey == String.Empty)
                {
                    System.Console.WriteLine("Your passkey is not set for now");
                } 
                else
                {
                    System.Console.WriteLine("Your passkey: {0}", config.Passkey);
                }
                return;
            }
            else if (args.Length == 2)
            {
                config.Passkey = args[1];
                config.Save();
                System.Console.WriteLine("Passkey configured");
                DebugLog.WriteLine(String.Format("Passkey set to '{0}'", config.Passkey), DebugLog.Level.Normal);
            }
            else
            {
                System.Console.WriteLine("Wrong number of arguments");
            }
        }

        static void Folder(string[] args)
        {
            if (args.Length == 1)
            {
                if (config.Folder == String.Empty)
                {
                    System.Console.WriteLine("Your download folder is not set for now");
                }
                else
                {
                    System.Console.WriteLine("Download folder: {0}", config.Folder);
                }
                return;
            }
            else if (args.Length == 2)
            {
                config.Folder = args[1];
                config.Save();
                System.Console.WriteLine("Download folder configured");
                DebugLog.WriteLine(String.Format("Download folder set to '{0}'", config.Folder), DebugLog.Level.Normal);
            }
            else
            {
                System.Console.WriteLine("Wrong number of arguments");
            }
        }

        static void ShowHelp() 
        {
            System.Console.WriteLine(@"
NoNaMe Club torrent tracker client
Original code by Sergey V. Shakshin

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
    Controls monitored topics list. 
    Subcommands:
        list   - displays list of monitored topics
        add    - add topic to list. Topic id has to be specified as argument
        delete - removes topic from list. Topic id has to be specified as argument
    
Run command:
    Do main functionality:
        - fetch RSS feed from tracker
        - search fetched RSS feed for monitored topics by ids
        - download torrrent files for matched topics

    Notice: RSS feed URL generated with h-argument to show topics updated in one hour.
            Scheduling run command once at hour is recomended.

");
        }
    }
}
