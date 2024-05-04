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
      // get url parameter for max results
      int maxResults = Request.Query.ContainsKey("maxResults") && !string.IsNullOrEmpty(Request.Query["maxResults"]) ? int.Parse(Request.Query["maxResults"]) : 50;

      PostCollection posts = new PostCollection(_redditService.GetTopSubredditPosts(subredditName, Constants.TimeFrame.All, maxResults));
      ViewData["posts"] = posts;
      ViewData["subredditName"] = subredditName;
      ViewData["authorWithMostPosts"] = GetAuthorWithMostPosts(posts);

      return View();
    }

    private string? GetAuthorWithMostPosts(PostCollection posts)
    {
      Dictionary<string, int> authorTally = new Dictionary<string, int>();

      foreach (var post in posts)
      {
        if (authorTally.ContainsKey(post.Author))
        {
          authorTally[post.Author]++;
        }
        else
        {
          authorTally[post.Author] = 1;
        }
      }

      string? authorWithMostPosts = null;
      int maxPostCount = 0;

      foreach (var kvp in authorTally)
      {
        if (kvp.Value > maxPostCount)
        {
          maxPostCount = kvp.Value;
          authorWithMostPosts = kvp.Key;
        }
      }

      // if there is a tie for first place, only the first author found will be returned
      return authorWithMostPosts;
    }
  }
}
