using PostHubAPI.Data;
using PostHubAPI.Models;

namespace PostHubAPI.Services
{
    public class PostService
    {
        private readonly PostHubAPIContext _context;

        public PostService(PostHubAPIContext context)
        {
            _context = context;
        }

        public async Task<Post> CreatePost(string title, Hub hub, Comment mainComment)
        {
            Post newPost = new Post()
            {
                Title = title,
                MainComment = mainComment,
                MainCommentId = mainComment.Id,
                Hub = hub
            };
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return newPost;
        }

        public async Task<Post?> GetPost(int id)
        {
            if (IsContextNull()) return null;
            return await _context.Posts.FindAsync(id);
        }

        public async Task<Post> DeletePost(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return post;
        }

        private bool IsContextNull() => _context.Posts == null;
    }
}
