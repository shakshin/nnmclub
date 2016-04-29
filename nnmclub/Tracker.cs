using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                WebRequest req = WebRequest.Create(url);
                String response = new StreamReader(req.GetResponse().GetResponseStream(), System.Text.Encoding.GetEncoding("Windows-1251")).ReadToEnd();
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
    }
}
