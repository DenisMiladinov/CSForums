using System.ComponentModel.DataAnnotations;

namespace CSForums.Models.Post
{
    public class NewPostModel
    {
        [Required]
        public string ForumName { get; set; }
        [Required]
        public int ForumId { get; set; }
        [Required]
        public string AuthormName { get; set; }
        [Required]
        public string ForumImageUrl { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
