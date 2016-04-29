using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace nnmclub
{
    public class Tracker
    {
        private static String BaseURL = "http://nnmclub.to/";

        public static String ResolveTopicTitle(int id)
        {
            String title = null;
            try
            {
                String url = String.Format("{0}forum/viewtopic.php?t={1}", BaseURL, id);
                String response = HttpGet(url);
                Regex r = new Regex(@"<title>(.+)</title>");
                MatchCollection matches = r.Matches(response);
                title = matches[0].Groups[1].Value;
            }
            catch (Exception ex)
            {
                title = null;
            }
            return title;
        }

        public static void Poll() 
        {
            Config config = Config.Get();
            String response = String.Empty; ;
            try
            {
                String url = String.Format("{0}forum/rss2.php?h=1&uk={1}", BaseURL, config.Passkey);
                response = HttpGet(url);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("FATAL: HTTP request failed: {0}", ex.Message);
                return;
            }

            List<int> ids = new List<int>();
            foreach (Topic t in config.Topics)
            {
                ids.Add(t.Id);
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response);
            foreach (XmlNode item in xml.DocumentElement.FirstChild)
            {
                if (item.Name == "item")
                {
                    try
                    {
                        String description = item.SelectSingleNode("description").InnerText;
                        Regex r = new Regex(@"t=(\d+)");
                        MatchCollection matches = r.Matches(description);
                        int id = int.Parse(matches[0].Groups[1].Value);
                        if (ids.Contains(id))
                        {
                            String dlLink = item.SelectSingleNode("link").InnerText;
                            System.Console.WriteLine("Downloading torrent: {0}", item.SelectSingleNode("title").InnerText);
                            if (Download(dlLink, id))
                            {
                                foreach (Topic t in config.Topics)
                                {
                                    if (t.Id == id)
                                    {
                                        t.LastDownloaded = DateTime.Now.ToString();
                                        config.Save();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
            }
        }

        private static Boolean Download(String url, int id)
        {
            try
            {
                String file = String.Format("nnmclub-{0}.torrent", id);
                String path = Path.Combine(Config.Get().Folder, file);


                WebClient web = new WebClient();
                web.DownloadFile(url, path);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("WARNING: Torrent download failed: {0}", ex.Message);
                return false;
            }
        }

        private static String HttpGet(String url)
        {
            WebRequest req = WebRequest.Create(url);
            return new StreamReader(req.GetResponse().GetResponseStream(), System.Text.Encoding.GetEncoding("Windows-1251")).ReadToEnd();
        }
    }
}
