using Karakatsiya.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Karakatsiya.Models.DTOs
{
    public class CreateItemDto
    {
        public int ItemId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        private DateTime? _expirationDate;

        public DateTime? ExpirationDate
        {
            get => _expirationDate;
            set => _expirationDate = value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                : null;
        }

        public IFormFile? ImageFile { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        public string? ExistingImagePath { get; set; }

        [Required]
        public bool IsSold { get; set; }
    }
}
