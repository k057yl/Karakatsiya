using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Karakatsiya.Models.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        private DateTime _creationDate;

        [Required]
        public DateTime CreationDate
        {
            get => _creationDate;
            set => _creationDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime? _expirationDate;

        public DateTime? ExpirationDate
        {
            get => _expirationDate;
            set => _expirationDate = value.HasValue
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                : (DateTime?)null;
        }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        [Required]
        public bool IsSold { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public ICollection<Sale>? Sales { get; set; }
    }
}
