using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservation.Models
{
    public class AppUser
    {
        [Key]
        public int AppUserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(255)]
        public string? Pictures { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        public List<AppUserPermission> Permissions { get; set; } = new List<AppUserPermission>();
    }
}
