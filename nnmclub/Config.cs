using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nnmclub
{
    [Serializable]
    class Config
    {
        public String passkey = String.Empty;
        public String folder = String.Empty;

        public List<Topic> topics = new List<Topic>();

        private static Config _cfg;

        private static String getPath() 
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        public static Config getConfig()
        {
            if (_cfg == null)
            {
                System.Console.WriteLine(getPath());
            }

            return _cfg;
        }
    }
}
