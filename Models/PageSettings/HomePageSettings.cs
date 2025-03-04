namespace Karakatsiya.Models.PageSettings
{
    public class HomePageSettings
    {
        public int Id { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? NewsTitle1 { get; set; }
        public string? NewsSummary1 { get; set; }
        public DateTime NewsDate1 { get; set; }

        public string? NewsTitle2 { get; set; }
        public string? NewsSummary2 { get; set; }
        public DateTime NewsDate2 { get; set; }
        /*
        public string? NewsTitle3 { get; set; }
        public string? NewsSummary3 { get; set; }
        public DateTime NewsDate3 { get; set; }
        */
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
    }
}
