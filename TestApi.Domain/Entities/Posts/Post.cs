using TestApi.Domain.Entities.Users;

namespace TestApi.Domain.Entities.Posts
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
