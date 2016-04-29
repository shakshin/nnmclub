using System;
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
                case "folder":
                    Folder(args);
                    break;
                case "passkey":
                    Passkey(args);
                    break;
                case "run":
                    if (args.Length != 1)
                    {
                        System.Console.WriteLine("");
                    }
                    if (config.Passkey == String.Empty)
                    {
                        System.Console.WriteLine("Passkey is not configured");
                        return;
                    }
                    if (config.Folder == String.Empty)
                    {
                        System.Console.WriteLine("Download folder is not configured");
                        return;
                    }
                    Tracker.Poll();
                    break;
                default:
                    System.Console.WriteLine("Wrong command: {0}", args[0]);
                    break;
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
                            catch (Exception ex)
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
                            catch (Exception ex)
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
            }
            else
            {
                System.Console.WriteLine("Wrong number of arguments");
            }
        }

        static void ShowHelp() 
        { 

        }
    }
}
