using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;

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

        public IEnumerable<Post> GetSubredditPosts(string subredditName)
        {
            var subreddit = _redditClient.Subreddit(subredditName);
            var posts = subreddit.Posts.GetNew(limit: 10);

            // query for users with the most posts in this subreddit
            var users = posts.Select(p => p.Author).GroupBy(a => a).OrderByDescending(g => g.Count()).Take(5);


            return posts ?? Enumerable.Empty<Post>();
        }
    
        public async Task<IEnumerable<Post>> FetchNewPostsAsync(string subredditName)
        {
            var subreddit = _redditClient.Subreddit(subredditName);

            // Create a task completion source to await the next set of new posts
            var newPostsTaskCompletionSource = new TaskCompletionSource<IEnumerable<Post>>();

            // Define the event handler for the NewUpdated event
            EventHandler<PostsUpdateEventArgs>? newPostsHandler = null;
            newPostsHandler = (sender, args) =>
            {
                newPostsTaskCompletionSource.SetResult(args.NewPosts);
            };

            subreddit.Posts.NewUpdated += newPostsHandler;

            subreddit.Posts.MonitorNew();

            // Await the task completion source for the next set of new posts
            return await newPostsTaskCompletionSource.Task;
        }
    }
}

