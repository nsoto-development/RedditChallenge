using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Reddit.Controllers.EventArgs;
using RedditChallenge.Models;
using RedditChallenge.Services.RedditApi;

namespace RedditChallenge.Controllers
{
    public class SubredditController : Controller
    {
        private readonly ApiService _redditService;

        public SubredditController(ApiService redditService)
        {
            _redditService = redditService;
        }

        public IActionResult Index(string subredditName)
        {
            // Your logic to retrieve and display metrics for the specified subreddit
            ViewData["subredditName"] = subredditName;
            ViewData["posts"] = _redditService.GetSubredditPosts(subredditName);

            

            return View();
        }


    }



}