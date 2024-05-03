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
      foreach (var post in posts)
      {
        Add(new Post(post));
      }
    }
  
  }
}