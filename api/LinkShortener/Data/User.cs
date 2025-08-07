using LinkShortener.Constants;

namespace LinkShortener.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public ICollection<Link> Links { get; set; }
    }
}