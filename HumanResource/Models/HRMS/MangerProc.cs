using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResource.Models.HRMS
{
    [NotMapped]
    public class MangerProc
    {
        [NotMapped]
        public int inEMPNO { get; set; }
        [NotMapped]
        public int inDG { get; set; }
        [NotMapped]
        public int inDEPT { get; set; }
        [NotMapped]
        public int inDESGN { get; set; }

        [NotMapped]
        public int? Outl_id { get; set; }
        
        [StringLength(200)]
        [NotMapped]
        public string? Outl_name { get; set; }

    }
}
