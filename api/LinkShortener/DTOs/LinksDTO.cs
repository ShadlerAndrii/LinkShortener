using LinkShortener.Data;

namespace LinkShortener.DTOs
{
    public class UnAuthorizedLinkDTO
    {
        public string ShortLink { get; set; }
        public string LongLink { get; set; }
    }

    public class LinkUserDTO
    {
        public int LinkId { get; set; }
        public string LongLink { get; set; }
        public string ShortLink { get; set; }
        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }
        public string UserLogin { get; set; }
    }
}
