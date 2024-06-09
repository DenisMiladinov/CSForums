using System.ComponentModel.DataAnnotations;

namespace CSForums.Models.Forum
{
    public class AddForumModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
