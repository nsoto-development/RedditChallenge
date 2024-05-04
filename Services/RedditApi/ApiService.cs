using Reddit;
using Reddit.Controllers;
using RedditChallenge.Constants;

namespace RedditChallenge.Services.RedditApi
{
  public class ApiService
  {
    private readonly RedditClient _redditClient;
    private readonly AuthService _authService;

    private string? _authToken;

    public ApiService(HttpClient httpClient, string clientId, string clientSecret, string userAgent)
    {
      // need to obtain OAuth token from Reddit API
      _authService = new AuthService(httpClient, clientId, clientSecret, userAgent);
      _authToken = _authService.GetAccessTokenAsync().Result;

      _redditClient = new RedditClient(appId: clientId, appSecret: clientSecret, userAgent: userAgent, accessToken: _authToken);
    }

    public bool ArePostsMonitored { get; private set; }

    public IEnumerable<Reddit.Controllers.Post> GetSubredditPosts(string subredditName, int maxResults = 50)
    {
      Subreddit subreddit = _redditClient.Subreddit(subredditName);
      List<Reddit.Controllers.Post> posts = subreddit.Posts.GetNew(limit: maxResults);

      return posts ?? Enumerable.Empty<Reddit.Controllers.Post>();
    }

    public IEnumerable<Reddit.Controllers.Post> GetTopSubredditPosts(string subredditName, TimeFrame timeFrame = TimeFrame.All, int maxResults = 100)
    {
      Subreddit subreddit = _redditClient.Subreddit(subredditName);
      List<Reddit.Controllers.Post> posts = subreddit.Posts.GetTop(t: nameof(timeFrame).ToLower(), limit: maxResults);

      return posts ?? Enumerable.Empty<Reddit.Controllers.Post>();
    }

    // public async Task<IEnumerable<Reddit.Controllers.Post>> FetchNewPostsAsync(string subredditName)
    // {
    //   // this is a work in progress out of curiosity in an effort to push post updates to the client - does not work yet
    //   // i would not ever leave code such as this in a production environment, but since it's a private work, it is here until I can get it working
    //   throw new NotImplementedException();

    //   Subreddit subreddit = _redditClient.Subreddit(subredditName);

    //   // Create a task completion source to await the next set of new posts
    //   TaskCompletionSource<IEnumerable<Reddit.Controllers.Post>> newPostsTaskCompletionSource = new TaskCompletionSource<IEnumerable<Reddit.Controllers.Post>>();

    //   // Define the event handler for the NewUpdated event
    //   EventHandler<PostsUpdateEventArgs>? newPostsHandler = null;
    //   newPostsHandler = (sender, args) =>
    //   {
    //     newPostsTaskCompletionSource.SetResult(args.NewPosts);
    //   };

    //   subreddit.Posts.NewUpdated += newPostsHandler;

    //   subreddit.Posts.MonitorNew();

    //   // Await the task completion source for the next set of new posts
    //   return await newPostsTaskCompletionSource.Task;
    // }

    // public async Task HandleWebSocket(WebSocket webSocket, string subredditName)
    // {
    //   // this is a work in progress out of curiosity in an effort to push post updates to the client - does not work yet
    //   // i would not ever leave code such as this in a production environment, but since it's a private work, it is here until I can get it working
    //   throw new NotImplementedException();
    //   Subreddit subreddit = _redditClient.Subreddit(subredditName);

    //   subreddit.Posts.NewUpdated += async (sender, args) =>
    //   {
    //     List<Reddit.Controllers.Post> incomingPosts = args.NewPosts; // Get the updated posts from the event arguments

    //     // convert this to our model
    //     PostCollection postsToSend = new PostCollection(incomingPosts);

    //     // Convert updated posts to JSON or any desired format
    //     string jsonData = JsonConvert.SerializeObject(postsToSend);

    //     // Send JSON data to WebSocket clients
    //     byte[] buffer = Encoding.UTF8.GetBytes(jsonData);

    //     try
    //     {
    //       await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    //     }
    //     catch (Exception ex)
    //     {
    //       Console.WriteLine($"Error sending data to WebSocket clients: {ex.Message}");
    //     }
    //   };

    //   subreddit.Posts.MonitorNew();

    //   await Task.CompletedTask;
    // }

  }
}

