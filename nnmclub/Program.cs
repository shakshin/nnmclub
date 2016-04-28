using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nnmclub
{
    class Program
    {
        static Config config = Config.getConfig();
        static void Main(string[] args)
        {
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
                    System.Console.WriteLine(String.Format("Wrong command: {0}", args[0]));
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
