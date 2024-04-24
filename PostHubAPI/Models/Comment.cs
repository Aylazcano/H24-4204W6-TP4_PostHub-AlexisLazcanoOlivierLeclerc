using System.ComponentModel.DataAnnotations.Schema;

namespace PostHubAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime? Date { get; set; }

        [InverseProperty("ParentComment")]
        public virtual List<Comment>? SubComments { get; set; }

        [InverseProperty("SubComments")]
        public virtual Comment? ParentComment { get; set; } // Si ceci est un sous-commentaire

        [InverseProperty("MainComment")]
        public virtual Post? MainCommentOf {  get; set; } // Si ce commentaire est le commentaire principal du post

        [InverseProperty("Comments")]
        public virtual User? User { get; set; }

        [InverseProperty("Upvotes")]
        public virtual List<User>? Upvoters { get; set; } = new List<User>();

        [InverseProperty("Downvotes")]
        public virtual List<User>? Downvoters { get; set; } = new List<User>();

        public int GetSubCommentTotal()
        {
            SubComments ??= new List<Comment>();
            int total = SubComments.Where(c => c.User != null).Count();
            foreach(Comment c in SubComments)
            {
                total += c.GetSubCommentTotal();
            }
            return total;
        }
    }
}
