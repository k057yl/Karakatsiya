using Karakatsiya.Models.Entities;

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

        public string? SortOrder { get; set; } = "az"; // "az", "za", "price_asc", "price_desc"

        public bool IncludeSold { get; set; } = false;

        public Category? Category { get; set; }

        public void Normalize()
        {
            if (CreatedAfter.HasValue)
                CreatedAfter = DateTime.SpecifyKind(CreatedAfter.Value, DateTimeKind.Utc);

            if (CreatedBefore.HasValue)
                CreatedBefore = DateTime.SpecifyKind(CreatedBefore.Value, DateTimeKind.Utc);
        }
    }
}
