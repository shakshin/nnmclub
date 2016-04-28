using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace nnmclub
{
    [Serializable]
    public class Config
    {
        [NonSerialized]
        private string _passkey;

        public string Passkey
        {
            get { return _passkey; }
            set { _passkey = value; }
        }

        [NonSerialized]
        private string _folder;

        public string Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        public List<Topic> Topics { get; set; }

        [NonSerialized]
        private static Config _cfg;

        private static String GetPath() 
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + Path.DirectorySeparatorChar + "nnmclub.xml";
        }

        public static Config Get()
        {
            if (_cfg == null)
            {
                _cfg = new Config();
                String file = GetPath();
                try
                {
                    if (File.Exists(file) && new FileInfo(file).Length > 0) 
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(Config));
                        FileStream stream = new FileStream(file, FileMode.Open);
                        _cfg = (Config) xml.Deserialize(stream);
                        stream.Close();
                    }
                } catch (Exception ex) {
                    System.Console.WriteLine("WARNING: configuration can not be loaded ({0]). Empty configuration used.", ex.Message);
                    _cfg = new Config();
                }
            }

            return _cfg;
        }

        public void Save()
        {
            String file = GetPath();
            XmlSerializer xml = new XmlSerializer(typeof(Config));
            StreamWriter stream = new StreamWriter(file);
            xml.Serialize(stream, this);
            stream.Close();
        }

        public Config() {
            Passkey = String.Empty;
            Folder = String.Empty;
            Topics = new List<Topic>();

        }
    }
}
