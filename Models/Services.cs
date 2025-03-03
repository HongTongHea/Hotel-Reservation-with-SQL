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
    }
}
