using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Karakatsiya.Models.Entities
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId { get; set; }

        public int? ItemId { get; set; }

        [MaxLength(200)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        private DateTime _saleDate;

        [Required]
        public DateTime SaleDate
        {
            get => _saleDate;
            set => _saleDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [NotMapped]
        public DateTime? ItemCreationDate => Item?.CreationDate;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

        [MaxLength(10)]
        public string? Currency { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Profit { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }

        [NotMapped]
        public string? ItemName => Item != null ? Item.Name : Name;

        public bool ItemIsDeleted { get; set; }

        [MaxLength(500)]
        public string? ItemImagePath { get; set; }
    }
}
