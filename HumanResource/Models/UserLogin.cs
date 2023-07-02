using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace HumanResource.Models
{
    [NotMapped]
    public class UserLogin
    {
        [Required(ErrorMessage = "UserName  Field is required!")]
        [MaxLength(30)]
        public string UserName { get; set; } = "";


        [Required(ErrorMessage = "Password  Field is required!")]
        public int Password { get; set; } = 0;

      
    }
}
