namespace Karakatsiya.Models.DTOs
{
    public class ItemFilterDto
    {
        public string? Name { get; set; }
        public string? UserId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }

        public DateTime? SoldAfter { get; set; } // через Sale
        public DateTime? SoldBefore { get; set; }

        public string? SortBy { get; set; } // "name", "price", "creationDate", "saleDate"
        public bool Ascending { get; set; } = true;
        public bool IncludeSold { get; set; } = true;
    }
}
