using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Reservation.Models
{
    public class Billing
    {
        [Key]
        public int BillingID { get; set; }

        [ForeignKey("Reservation")]
        [Required]
        public int ReservationID { get; set; }

        [ForeignKey("Customer")]
        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("Room")]
        [Required]
        public int RoomId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime .Now;

        public string? Status { get; set; }

        public virtual Customer? Customer { get; set; }  
        public virtual Room? Room { get; set; }


    }
}
