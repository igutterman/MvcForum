using System.Collections;

namespace MvcForum.Models
{
    public class ForumThread : IEnumerable<Post>
    {
        public int Id { get; set; }
        public List<Post> Posts { get; set; }

        public ForumThread()
        {
            Posts = new List<Post>();
        }

        public ForumThread(Post post)
        {
            Posts = new List<Post>();
            Posts.Add(post);
        }

        public void addPost(Post post)
            => Posts.Add(post);

        public void sortPostsByTimestamp()
        {
            Posts.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));   //if you need descending sort, swap x and y on the right-hand side of the arrow =>. – 

        }

        public DateTime getLastPostTime()
        {
            DateTime last = DateTime.MinValue;
            foreach (var post in Posts)
            {
                if (post.Timestamp > last)
                {
                    last = post.Timestamp;
                }
            }
            return last;
        }

        public IEnumerator<Post> GetEnumerator()
        {
            foreach (var post in Posts)
            {
                yield return post;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //possible: add a "generateDisplayThread" method to only enable controller to only pass needed posts to the view
        //ex: if thread has many replies, pass op-post and last two replies, if thread has one reply, pass both posts.
    }
}
