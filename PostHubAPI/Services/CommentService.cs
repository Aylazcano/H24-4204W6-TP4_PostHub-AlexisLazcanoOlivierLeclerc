using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PostHubAPI.Services
{
    public class CommentService
    {
        private readonly PostHubAPIContext _context;

        public CommentService(PostHubAPIContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetComment(int id)
        {
            if (IsContextNull()) return null;

            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment?> CreateComment(User user, string text, Comment? parentComment, List<Picture>? picList)
        {
            if (IsContextNull()) return null;

            Comment newComment = new Comment()
            {
                Id = 0,
                Text = text,
                Date = DateTime.UtcNow,
                User = user, // Auteur
                ParentComment = parentComment, // null si commentaire principal du post
                Pictures = picList,
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return newComment;
        }

        public async Task<Comment?> EditComment(Comment comment, string text, List<Picture> picList)
        {
            comment.Text = text;
            comment.Pictures = picList;
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> SoftDeleteComment(Comment deletedComment)
        {
            deletedComment.Text = "Commentaire supprimé.";
            deletedComment.User = null;
            deletedComment.Pictures ??= new List<Picture>();
            deletedComment.Upvoters ??= new List<User>();
            deletedComment.Downvoters ??= new List<User>();
            foreach(User u in deletedComment.Upvoters)
            {
                u.Upvotes?.Remove(deletedComment);
            }
            foreach(User u in deletedComment.Downvoters)
            {
                u.Downvotes?.Remove(deletedComment);
            }
            deletedComment.Upvoters = new List<User>();
            deletedComment.Downvoters = new List<User>();
            await _context.SaveChangesAsync();
            return deletedComment;
        }

        public async Task<Comment?> HardDeleteComment(Comment deletedComment)
        {
            deletedComment.SubComments ??= new List<Comment>();

            for(int i = deletedComment.SubComments.Count - 1; i >= 0; i--)
            {
                Comment? deletedSubComment = await HardDeleteComment(deletedComment.SubComments[i]);
                if (deletedSubComment == null) return null;
            }

            _context.Comments.Remove(deletedComment);
            await _context.SaveChangesAsync();
            return deletedComment;
        }

        public async Task<bool> UpvoteComment(int id, User user)
        {
            if (IsContextNull()) return false;

            Comment? comment = await _context.Comments.FindAsync(id);
            if (comment == null || comment.User == null) return false;

            comment.Upvoters ??= new List<User>();
            comment.Downvoters ??= new List<User>();

            if (comment.Upvoters.Contains(user)) comment.Upvoters.Remove(user);
            else
            {
                comment.Upvoters.Add(user);
                comment.Downvoters.Remove(user);
            }
            await _context.SaveChangesAsync();

            return true; // Basculement du upvote réussi
        }

        public async Task<bool> DownvoteComment(int id, User user)
        {
            if (IsContextNull()) return false;

            Comment? comment = await _context.Comments.FindAsync(id);
            if (comment == null || comment.User == null) return false;

            comment.Upvoters ??= new List<User>();
            comment.Downvoters ??= new List<User>();

            if (comment.Downvoters.Contains(user)) comment.Downvoters.Remove(user);
            else
            {
                comment.Downvoters.Add(user);
                comment.Upvoters.Remove(user);
            }

            await _context.SaveChangesAsync();

            return true; // Basculement du downvote réussi
        }

        private bool IsContextNull() => _context.Comments == null;
    }
}
