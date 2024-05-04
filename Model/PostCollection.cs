using System.Collections.ObjectModel;

namespace RedditChallenge.Model
{
  public class PostCollection : Collection<Post>
  {

    public PostCollection()
    {
    }

    public PostCollection(IEnumerable<Reddit.Controllers.Post> posts)
    {
      foreach (Reddit.Controllers.Post post in posts)
      {
        Add(new Post(post));
      }
    }
  }
}
