using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace Data
{
    public class TweetContext : DbContext
    {
        public DbSet<TweetModel> AllTweets { get; set; }

        static readonly string baseURI = "https://badapi.Iqvia.io/";

        //Initialize comparer to check for duplcate twees
        static readonly TweetComparer tweetComp = new TweetComparer();

        // Use HttpClient.
        static HttpClient client;
        static Newtonsoft.Json.JsonSerializerSettings js;

        static readonly string startDate = "2016-01-01";
        static readonly string endDate = "2018-01-01";

        public TweetContext(DbContextOptions<TweetContext> options)
            : base(options)
        {
          
        }

        public TweetContext()
        {

        }

        public static bool DownloadTweets(TweetContext context)
        {

            var tweetListhttp = DownloadTweetsHTTP().ToList();
            
            if (tweetListhttp == null)//Something went wrong
            {
                return false;
            }

            foreach (var tweet in tweetListhttp)
            {
                context.AllTweets.Add(tweet);
            }

            context.SaveChanges();

            return true;
        }

        static bool InitializeHTTP()
        {
            try
            {
                // Use HttpClient.
                client = new HttpClient()
                {
                    BaseAddress = new Uri(baseURI)
                };

                //Initialize deserialization with datetime using UTC
                js = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc
                };
            }
            catch { }


            return true;
        }
        private static List<TweetModel> DownloadTweetsHTTP()
        {
            if (!InitializeHTTP())
            {
                return null;
            }

            //List of tweets with no duplicates
            List<TweetModel> filteredtweets = new List<TweetModel>();

            // Target.
            string strGetRequest = "api/v1/Tweets?startDate=" + startDate + "&endDate=" + endDate;

            try
            {
                var tweetResult = GetTweetsAsync(strGetRequest).Result;
                if (tweetResult == null)
                {
                    return null;
                }

                //Populate filtered list with the first Get results
                filteredtweets = tweetResult.ToList();

                // Now loop until we have reached the desired end date. 
                while (tweetResult.Count > 1)
                {
                    //Set new start date to the date of the last item in the previous set of tweets
                    var newStartDate = tweetResult[tweetResult.Count - 1].stamp.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    strGetRequest = "api/v1/Tweets?startDate=" + newStartDate + "&endDate=" + endDate;

                    tweetResult = GetTweetsAsync(strGetRequest).Result;
                    if (tweetResult == null)
                    {
                        return null;
                    }

                    //Get number of tweets
                    int nCount = tweetResult.Count;

                    //Avoid duplicates in the get
                    tweetResult = tweetResult.Distinct<TweetModel>(tweetComp).AsParallel().ToList<TweetModel>();

                    //Add to filtered tweets
                    filteredtweets.AddRange(tweetResult);

                    //If less than 100 we are getting the last bit of tweets for the desired date range
                    if (nCount < 100)
                    {
                        break;
                    }
                }

            }
            catch
            {
                return null;
            }

            //Ensure last add tweets are distinct
            filteredtweets = filteredtweets.Distinct<TweetModel>(new TweetComparer()).AsParallel().ToList<TweetModel>();

            return filteredtweets;



        }

        private static async Task<List<TweetModel>> GetTweetsAsync(string strGetRequest)
        {
            try
            {
                //Make the get request
                HttpResponseMessage response = await client.GetAsync(strGetRequest);

                var responsePhrase = response.ReasonPhrase;
                if (!responsePhrase.Equals("OK"))
                {
                    return null;
                }

                HttpContent content = response.Content;

                // ... Read the string.
                string result = await content.ReadAsStringAsync();



                //Get the list of responses
                var tweetResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TweetModel>>(result, js);

                tweetResult.Distinct<TweetModel>(tweetComp).AsParallel().ToList<TweetModel>();

                //Populate filtered list with the first Get results
                return tweetResult;

            }
            catch
            {

            }

            return null;

        }
    }
}
