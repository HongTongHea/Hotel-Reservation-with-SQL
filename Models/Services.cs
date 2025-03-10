using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Reservation.Models
{
    public class Services
    {
        [Key]
        public int ServiceID { get; set; }
        [Required]
        [MaxLength(100)]
        public string? ServiceName { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ServiceRate { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [MaxLength(50)]
        public string? Category { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int? DurationInMinutes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
