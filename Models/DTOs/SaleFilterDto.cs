namespace Karakatsiya.Models.DTOs
{
    public class SaleFilterDto
    {
        public string? UserId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public decimal? MinProfit { get; set; }
        public decimal? MaxProfit { get; set; }

        public string SortOrder { get; set; } = "date_desc";

        public void Normalize()
        {
            if (StartDate.HasValue)
                StartDate = DateTime.SpecifyKind(StartDate.Value, DateTimeKind.Utc);

            if (EndDate.HasValue)
                EndDate = DateTime.SpecifyKind(EndDate.Value, DateTimeKind.Utc);
        }

        public string? ItemName { get; set; }

        public string? Currency { get; set; }
    }
}
