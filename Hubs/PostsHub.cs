using Microsoft.AspNetCore.SignalR;
using RedditChallenge.Services.RedditApi;
using RedditChallenge.Model;

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
      // this is a work in progress out of curiosity in an effort to push post updates to the client - does not work yet
      // i would not ever leave code such as this in a production environment, but since it's a private work, it is here until I can get it working
      while (true)
      {
        // Fetch new posts from the specified subreddit
        IEnumerable<Reddit.Controllers.Post> newPosts = await _redditService.FetchNewPostsAsync(subredditName);

        // Yield each new post to clients
        foreach (Reddit.Controllers.Post post in newPosts)
        {
          Post newPost = new Post(post);
          yield return newPost;
        }

        // Optionally introduce a delay before fetching new posts again
        await Task.Delay(TimeSpan.FromSeconds(10));
      }
    }
  }
}

