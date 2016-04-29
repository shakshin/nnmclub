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
            Config config = Config.Get();
            System.Console.WriteLine("Topics:");
            System.Console.WriteLine();
            foreach (Topic topic in config.Topics)
            {
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
            Config config = Config.Get();
            foreach (Topic topic in config.Topics)
            {
                if (topic.Id == id)
                {
                    System.Console.WriteLine("Topic with id {0} is already in list", id);
                    return;
                }
            }
            Topic t = new Topic();
            t.Id = id;
            t.Title = Tracker.ResolveTopicTitle(id);
            config.Topics.Add(t);
            System.Console.WriteLine("Topic with id {0} was added to list", id);
            config.Save();
        }

        public static void Delete(int id)
        {
            Config config = Config.Get();
            Topic t = null;
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
            }
            else
            {
                config.Topics.Remove(t);
                System.Console.WriteLine("Topic with id {0} was removed from list", id);
                config.Save();
            }
        }
    }
}
