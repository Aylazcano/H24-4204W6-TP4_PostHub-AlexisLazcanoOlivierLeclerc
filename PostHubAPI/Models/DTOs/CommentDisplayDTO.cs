namespace PostHubAPI.Models.DTOs
{
    public class CommentDisplayDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime? Date { get; set; }
        public string? Username { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public bool Upvoted { get; set; }
        public bool Downvoted { get; set; }
        public int SubCommentTotal { get; set; }
        public List<CommentDisplayDTO>? SubComments { get; set; }
        public List<int>? PictureIds { get; set; }
        public CommentDisplayDTO() { }
        public CommentDisplayDTO(Comment comment, bool withSubComments, User? user)
        {
            List<CommentDisplayDTO>? subComments = null;
            if (withSubComments) subComments = comment.SubComments?.Select(c => new CommentDisplayDTO(c, true, user)).ToList();

            Id = comment.Id;
            Text = comment.Text;
            Date = comment.Date;
            Username = comment.User?.UserName;
            Upvotes = comment.Upvoters?.Count ?? 0;
            Downvotes = comment.Downvoters?.Count ?? 0;
            Upvoted = user != null && (comment.Upvoters?.Contains(user) ?? false);
            Downvoted = user != null && (comment.Downvoters?.Contains(user) ?? false);
            SubCommentTotal = comment.GetSubCommentTotal();
            SubComments = subComments;
            PictureIds = comment.Pictures?.Select(x => x.Id).ToList();
        }
    }
}
