using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;

namespace AwesomeTweets.Controllers
{
    public class GetTweetsController : Controller
    {
        private readonly TweetContext _dbContext;

        //pass in the database context using dependency injection

        public GetTweetsController(TweetContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Here we pass the downloaded tweets to GetTweets view
        public IActionResult GetTweets()
        {
            return View(_dbContext.AllTweets);
        }

    }
}