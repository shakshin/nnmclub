using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace nnmclub
{
    public class DebugLog
    {
        public enum Level { Normal, Verbose, Debug};

        private static StreamWriter _file = null;
        
        public static void WriteLine(String message, Level level)
        {
            try
            {
                if (_file == null)
                    Init();
                Config cfg = Config.Get();
                if (cfg.LogLevel < level)
                    return;

                _file.WriteLine("{0}: {1}", DateTime.Now.ToString(new CultureInfo("ru-RU")), message);
            }
            catch (Exception ex)
            {
                
            }
        }

        public static void WriteLine(String message)
        {
            WriteLine(message, Level.Normal);
        }

        private static void Init()
        {
            if (_file != null)
                return;
            try
            {
                _file = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "nnmclub.log"), true, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _file = null;
                System.Console.WriteLine(ex.Message);
            }
        }

        public static void Close()
        {
            if (_file == null)
                return;

            _file.Close();
        }
    }
}
