using System.ComponentModel.DataAnnotations.Schema;

namespace PostHubAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual Hub? Hub { get; set; }

        [InverseProperty("MainCommentOf")]
        public virtual Comment? MainComment { get; set; } // Commentaire principal de l'auteur qui a créé le post
        public int MainCommentId { get; set; } // Id du commentaire principal de l'auteur qui a créé le post
        
    }
}
