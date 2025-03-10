using System.ComponentModel.DataAnnotations;

namespace Karakatsiya.Models.PageSettings
{
    public class NewsArticle
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        private DateTime _creationDate;

        [Required]
        public DateTime CreationDate
        {
            get => _creationDate;
            set => _creationDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        public string? ImageUrl { get; set; }
    }
}
