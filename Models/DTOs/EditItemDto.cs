using Karakatsiya.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Karakatsiya.Models.DTOs
{
    public class EditItemDto
    {
        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public List<IFormFile>? ImageFiles { get; set; }

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
