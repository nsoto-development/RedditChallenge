using Reddit;

namespace RedditChallenge.Model
{
  public class Post
  {
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Score { get; set; } = 0;
    // public string Content { get; set; }
    // public int Upvotes { get; set; }
    // public int Downvotes { get; set; }
    // public int CommentsCount { get; set; }

    public Post()
    {
    }
    public Post(Reddit.Controllers.Post post)
    {
      Title = post.Title;
      Author = post.Author;
      Score = post.Score;
      // Content = post.SelfText;
      // Upvotes = post.UpVotes;
      // Downvotes = post.DownVotes;
      // CommentsCount = post.CommentCount;
    }
  }
}