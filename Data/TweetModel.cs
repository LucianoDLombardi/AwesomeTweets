using System;
using System.Collections.Generic;

namespace Data
{
    //Tweet Model
    public class TweetModel
    {
        public string id { get; set; }
        public DateTime stamp { get; set; }
        public string text { get; set; }
    }

    //Class to check for duplicate tweets
    public class TweetComparer : IEqualityComparer<TweetModel>
    {
        public bool Equals(TweetModel x, TweetModel y)
        {
            if (x == null || y == null)
                return false;
            else if (x.text == y.text)
                return true;
            else
                return false;
        }

        public int GetHashCode(TweetModel obj)
        {
            return obj.text.GetHashCode();
        }

      
    }
}
