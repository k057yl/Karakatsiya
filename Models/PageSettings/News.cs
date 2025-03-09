namespace Karakatsiya.Models.PageSettings
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? ImagePath { get; set; }
    }
}
