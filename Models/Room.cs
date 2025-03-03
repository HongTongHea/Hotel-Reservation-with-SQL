using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Reservation.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoomNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomType { get; set; }

        [Required]
        [StringLength(100)]
        public string AvailabilityStatus { get; set; }

        public int? Floor { get; set; }
        public int? BedCount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? RoomPrice { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(255)]
        public string? Pictures { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
