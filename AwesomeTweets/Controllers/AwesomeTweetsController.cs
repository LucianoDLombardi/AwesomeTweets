using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AwesomeTweets.Models;
using Data;

namespace AwesomeTweets.Controllers
{
    public class AwesomeTweetsController : Controller
    {
        private readonly TweetContext _dbContext;

        //pass in the database context using dependency injection

        public AwesomeTweetsController(TweetContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Here we pass the downloaded tweets to GetTweets view
        public IActionResult Tweets()
        {
            return View(_dbContext.AllTweets);
        }


        public IActionResult Index()
        {
            return View(_dbContext.AllTweets);
        }
              

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
