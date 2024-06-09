using System.ComponentModel.DataAnnotations;

namespace CSForums.Models.Reply
{
    public class PostReplyModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorImageUrl { get; set; }
        [Required]
        public bool IsAuthorAdmin { get; set; }

        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string ReplyContent { get; set; }

        [Required]
        public int PostId { get; set; }
        [Required]
        public string PostTitle { get; set; }
        [Required]
        public string PostContent { get; set; }

        [Required]
        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }
        [Required]
        public int ForumId { get; set; }
    }
}
