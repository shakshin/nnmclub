using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nnmclub
{
    [Serializable]
    public class Topic
    {
        [NonSerialized]
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [NonSerialized]
        private String _title;

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [NonSerialized]
        private String _lastDownloaded;

        public String LastDownloaded
        {
            get { return _lastDownloaded; }
            set { _lastDownloaded = value; }
        }

        public static void List()
        {
            DebugLog.WriteLine(String.Format("Topics list command"), DebugLog.Level.Debug);
            Config config = Config.Get();
            System.Console.WriteLine("Topics:");
            System.Console.WriteLine();
            DebugLog.WriteLine(String.Format("Topics list  size: {0}", config.Topics.Count), DebugLog.Level.Debug);
            foreach (Topic topic in config.Topics)
            {
                DebugLog.WriteLine(String.Format("topic id {0}", topic.Id), DebugLog.Level.Debug);
                System.Console.WriteLine(
                    "{0}: {1}",
                    topic.Id,
                    topic.Title ?? "No title resolved for topic"
                );
                System.Console.WriteLine("  Last downloaded: {0}", topic.LastDownloaded ?? "never");
                System.Console.WriteLine();    
            } 
            System.Console.WriteLine();
        }

        public static void Add(int id)
        {
            DebugLog.WriteLine(String.Format("Topic add command specified"), DebugLog.Level.Verbose);
            Config config = Config.Get();
            DebugLog.WriteLine(String.Format("Checking topic existance"), DebugLog.Level.Debug);
            foreach (Topic topic in config.Topics)
            {
                if (topic.Id == id)
                {
                    System.Console.WriteLine("Topic with id {0} is already in list", id);
                    DebugLog.WriteLine(String.Format("Topic is already in list"), DebugLog.Level.Verbose);
                    return;
                }
            }
            DebugLog.WriteLine(String.Format("Creating new topic instance"), DebugLog.Level.Debug);
            Topic t = new Topic();
            t.Id = id;
            t.Title = Tracker.ResolveTopicTitle(id);
            DebugLog.WriteLine(String.Format("Inserting record into list"), DebugLog.Level.Debug);
            config.Topics.Add(t);
            System.Console.WriteLine("Topic with id {0} was added to list", id);
            DebugLog.WriteLine(String.Format("New topic added: {0} ({1})", id, t.Title), DebugLog.Level.Normal);
            config.Save();
        }

        public static void Delete(int id)
        {
            DebugLog.WriteLine(String.Format("Topic delete command specified"), DebugLog.Level.Verbose);
            Config config = Config.Get();
            Topic t = null;
            DebugLog.WriteLine(String.Format("Checking topic existance"), DebugLog.Level.Debug);
            foreach (Topic topic in config.Topics)
            {
                if (topic.Id == id)
                {
                    t = topic;
                    break;
                }
            }
            if (t == null)
            {
                System.Console.WriteLine("Topic with id {0} is not in list", id);
                DebugLog.WriteLine(String.Format("Topic is not in list"), DebugLog.Level.Verbose);
            }
            else
            {
                DebugLog.WriteLine(String.Format("Removing record from list"), DebugLog.Level.Debug);
                config.Topics.Remove(t);
                System.Console.WriteLine("Topic with id {0} was removed from list", id);
                DebugLog.WriteLine(String.Format("Topic deleeted from list: {0} ({1})", id, t.Title), DebugLog.Level.Normal);
                config.Save();
            }
        }
    }
}
