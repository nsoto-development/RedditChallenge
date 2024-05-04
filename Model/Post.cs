namespace RedditChallenge.Model
{
  public class Post
  {
    public string PostId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Score { get; set; } = 0;
    public string Permalink { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Downvotes { get; set; } = 0;
    public int Upvotes { get; set; } = 0;
    public string Thumbnail { get; set; } = "";
    public int? ThumbnailHeight { get; set; } = null;
    public int? ThumbnailWidth { get; set; } = null;

    public Post()
    {
    }
    public Post(Reddit.Controllers.Post post)
    {
      PostId = post.Id;
      Title = post.Title;
      Author = post.Author;
      Score = post.Score;
      Permalink = post.Permalink;
      CreatedAt = post.Created;
      Downvotes = post.DownVotes;
      Upvotes = post.UpVotes;

      // check if the post can be cast to a LinkPost - fill those extra properties if so
      if (post is Reddit.Controllers.LinkPost linkPost)
      {
        Thumbnail = linkPost.Thumbnail;
        ThumbnailHeight = linkPost.ThumbnailHeight;
        ThumbnailWidth = linkPost.ThumbnailWidth;
      }
    }
  }
}
