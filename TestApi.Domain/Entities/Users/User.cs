using TestApi.Domain.Entities.Posts;

namespace TestApi.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string? Preferences { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
