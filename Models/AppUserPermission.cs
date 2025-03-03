using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Reservation.Models
{
    public class AppUserPermission
    {
        [Key]
        public int PermissionID { get; set; }

        [Required]
        public int AppUserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Permission { get; set; }

        [ForeignKey("AppUserID")]
        public AppUser AppUser { get; set; }
    }
}
