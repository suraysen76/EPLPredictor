using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string? UserName { get; set; }

        public string? Name { get; set; }

        [Required] 
        public string? Password { get; set; }

        [DisplayName("Joined Date")]
        public DateTime JoinedDate { get; set; }
        public string? Role { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
    }
}
