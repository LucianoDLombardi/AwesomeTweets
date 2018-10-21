using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;

namespace AwesomeTweets.Controllers
{
    public class AllTweetsController : Controller
    {
        private readonly TweetContext _dbContext;

        //pass in the database context using dependency injection

        public AllTweetsController(TweetContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Here we pass the downloaded tweets to GetTweets view
        public IActionResult Tweets()
        {
            return View(_dbContext.AllTweets);
        }

    }
}