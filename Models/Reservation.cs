using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Reservation.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int RoomID { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int NumberOfChildren { get; set; }

        [Required]
        public int NumberOfAdults { get; set; }

        [Column(TypeName = "decimal(20, 2)")]
        public decimal? TotalPrice { get; set; }

        [Required]
        public string? PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("CustomerID")]
        public Customer? Customer { get; set; }

        [ForeignKey("RoomID")]
        public Room? Room { get; set; }

        public ICollection<Billing> Billings { get; set; } = new List<Billing>();

    }


}
