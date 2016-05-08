using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;

namespace nnmclub
{
    public class Tracker
    {
        private static String BaseURL = "https://nnmclub.to/";

        public static String ResolveTopicTitle(int id)
        {
            DebugLog.WriteLine(String.Format("Will try to resolve topic title"), DebugLog.Level.Verbose);
            String title = null;
            try
            {
                String url = String.Format("{0}forum/viewtopic.php?t={1}", BaseURL, id);
                String response = HttpGet(url);
                DebugLog.WriteLine(String.Format("HTTP request complete"), DebugLog.Level.Debug);
                Regex r = new Regex(@"<title>(.+)</title>");
                MatchCollection matches = r.Matches(response);
                title = matches[0].Groups[1].Value;
                DebugLog.WriteLine(String.Format("Content filter matches"), DebugLog.Level.Debug);
            }
            catch (Exception ex)
            {
                DebugLog.WriteLine(String.Format("Resolution failed: {0}", ex.Message), DebugLog.Level.Verbose);
                title = null;
            }
            return title;
        }

        public static void Poll() 
        {
            DebugLog.WriteLine(String.Format("Run command specified"), DebugLog.Level.Verbose);
            Config config = Config.Get();
            String response = String.Empty;
            DebugLog.WriteLine(String.Format("Fetching RSS feed"), DebugLog.Level.Normal);
            try
            {
                String url = String.Format("{0}forum/rss2.php?h=1&uk={1}", BaseURL, config.Passkey);
                response = HttpGet(url);
                DebugLog.WriteLine(String.Format("HTTP request complete"), DebugLog.Level.Debug);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("FATAL: HTTP request failed: {0}", ex.Message);
                DebugLog.WriteLine(String.Format("Fetch failed: {0}", ex.Message), DebugLog.Level.Normal);
                return;
            }
            DebugLog.WriteLine(String.Format("RSS feed fetched"), DebugLog.Level.Normal);

            DebugLog.WriteLine(String.Format("Preparing topic filter. Topics list size: {0}", config.Topics.Count), DebugLog.Level.Debug);
            List<int> ids = new List<int>();
            foreach (Topic t in config.Topics)
            {
                ids.Add(t.Id);
            }

            DebugLog.WriteLine(String.Format("Parsing RSS feed document"), DebugLog.Level.Verbose);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response);
            DebugLog.WriteLine(String.Format("RSS parsed. Searching."), DebugLog.Level.Verbose);
            foreach (XmlNode item in xml.DocumentElement.FirstChild)
            {
                if (item.Name == "item")
                {
                    DebugLog.WriteLine(String.Format("Found ITEM entry"), DebugLog.Level.Verbose);
                    try
                    {
                        String description = item.SelectSingleNode("description").InnerText;
                        String guid = item.SelectSingleNode("guid").InnerText;
                        Regex r = new Regex(@"t=(\d+)");
                        MatchCollection matches = r.Matches(description);
                        int id = int.Parse(matches[0].Groups[1].Value);
                        if (ids.Contains(id))
                        {
                            DebugLog.WriteLine(String.Format("Item ID matched"), DebugLog.Level.Verbose);
                            DebugLog.WriteLine(String.Format("Matched item GUID: {0}", guid), DebugLog.Level.Debug);
                            String dlLink = item.SelectSingleNode("link").InnerText;
                            DebugLog.WriteLine(String.Format("Download URL: {0}", dlLink), DebugLog.Level.Debug);
                            DebugLog.WriteLine(String.Format("Fetching topic id {0}, {1]", id, item.SelectSingleNode("title").InnerText), DebugLog.Level.Normal);
                            System.Console.WriteLine("Downloading torrent: {0}", item.SelectSingleNode("title").InnerText);
                            if (Download(dlLink, id))
                            {
                                DebugLog.WriteLine(String.Format("Download sucessfull"), DebugLog.Level.Debug);
                                foreach (Topic t in config.Topics)
                                {
                                    if (t.Id == id)
                                    {
                                        DebugLog.WriteLine(String.Format("Updating last downloaded stamp for topic"), DebugLog.Level.Verbose);
                                        t.LastDownloaded = DateTime.Now.ToString(new CultureInfo("ru-RU"));
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
            DebugLog.WriteLine(String.Format("Will download file"), DebugLog.Level.Debug);
            try
            {
                String file = String.Format("nnmclub-{0}.torrent", id);
                String path = Path.Combine(Config.Get().Folder, file);
                DebugLog.WriteLine(String.Format("Target path: {0}", path), DebugLog.Level.Debug);

                WebClient web = new WebClient();
                web.DownloadFile(url, path);
                DebugLog.WriteLine(String.Format("Fetch complete"), DebugLog.Level.Normal);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("WARNING: Torrent download failed: {0}", ex.Message);
                DebugLog.WriteLine(String.Format("Fetch failed: {0}", ex.Message), DebugLog.Level.Normal);
                return false;
            }
        }

        private static String HttpGet(String url)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; // to ignore bad SSL certificate
            WebRequest req = WebRequest.Create(url);
            return new StreamReader(req.GetResponse().GetResponseStream(), System.Text.Encoding.GetEncoding("Windows-1251")).ReadToEnd();
        }
    }
}
