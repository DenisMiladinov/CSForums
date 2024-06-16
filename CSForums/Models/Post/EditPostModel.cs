namespace CSForums.Models.Post
{
    public class EditPostModel
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
    }
}
