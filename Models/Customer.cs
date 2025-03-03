using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservation.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Gender { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string PassportNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string Country { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
