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
                    Topic(args);
                    break;
                case "folder":
                    Folder(args);
                    break;
                case "passkey":
                    Passkey(args);
                    break;
                default:
                    System.Console.WriteLine("Wrong command: {0}", args[0]);
                    break;
            }
        }

        static void Topic(string[] args)
        {

        }

        static void Passkey(string[] args)
        {

        }

        static void Folder(string[] args)
        {

        }

        static void ShowHelp() 
        { 

        }
    }
}
