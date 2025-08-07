namespace LinkShortener.Data
{
    public class Link
    {
        public int LinkId { get; set; }
        public string LongLink { get; set; }
        public string ShortLink { get; set; }
        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}