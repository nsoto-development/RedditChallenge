using Microsoft.AspNetCore.Mvc;

using RedditChallenge.Model;
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

            PostCollection posts = new PostCollection(_redditService.GetSubredditPosts(subredditName));
            ViewData["posts"] = posts;

            return View();
        }
    }
}