using Microsoft.AspNetCore.SignalR;
using RedditChallenge.Services.RedditApi;
using RedditChallenge.Model;
using System.Threading.Tasks;
using Reddit.Controllers.EventArgs;

namespace RedditChallenge.Hubs
{
  public class PostsHub : Hub
  {
      private readonly ApiService _redditService;

      public PostsHub(ApiService redditService)
      {
          _redditService = redditService;
      }
      
      public async IAsyncEnumerable<Post> StreamNewPostsAsync(string subredditName)
      {
          while (true)
          {
              // Fetch new posts from the specified subreddit
              var newPosts = await _redditService.FetchNewPostsAsync(subredditName);

              // Yield each new post to clients
              foreach (var post in newPosts)
              {
                  Post newPost = new Post(post);
                  yield return newPost;
              }

              

            //   // Optionally introduce a delay before fetching new posts again
            //   await Task.Delay(TimeSpan.FromSeconds(10));
          }
      }




  }
}

