namespace Karakatsiya.Models.PageSettings
{
    public class HomePageSettings
    {
        public int Id { get; set; }

        // Контакты
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        // Новости
        public string? NewsTitle1 { get; set; }
        public string? NewsSummary1 { get; set; }
        public DateTime NewsDate1 { get; set; }

        public string? NewsTitle2 { get; set; }
        public string? NewsSummary2 { get; set; }
        public DateTime NewsDate2 { get; set; }

        // Фото (пути к картинкам)
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
    }
}
