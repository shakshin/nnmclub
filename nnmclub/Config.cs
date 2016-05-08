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

        public DebugLog.Level LogLevel
        {
            get
            {
                return _LogLevel;
            }

            set
            {
                _LogLevel = value;
            }
        }

        [NonSerialized]
        private DebugLog.Level _LogLevel;

        [NonSerialized]
        private static Config _cfg;

        private static String GetPath() 
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "nnmclub.xml");
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
            DebugLog.WriteLine(String.Format("Configuration save request"), DebugLog.Level.Debug);
            try
            {
                String file = GetPath();
                XmlSerializer xml = new XmlSerializer(typeof(Config));
                StreamWriter stream = new StreamWriter(file);
                xml.Serialize(stream, this);
                stream.Close();
            } catch (Exception ex)
            {
                System.Console.WriteLine("WARNING: can not write configuration: {0]", ex.Message);
                DebugLog.WriteLine(String.Format("Can not write configuration: {0}", ex.Message), DebugLog.Level.Normal);
            }
        }

        public Config() {
            Passkey = String.Empty;
            Folder = String.Empty;
            Topics = new List<Topic>();
            LogLevel = DebugLog.Level.Normal;
        }
    }
}
