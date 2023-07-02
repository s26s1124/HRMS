using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResource.Models.HRMS;

public class NavigationMenu
{
    [Key]
    public int PageId { get; set; }

    [StringLength(100)]
    public string? PageName { get; set; }

    [StringLength(100)]  
    public string? PagePath { get; set; }
    public int? ParentMenuId { get; set; }

    public int? IsActivated { get; set; }

    [StringLength(100)]
    public string? ControllerName { get; set; }

    [StringLength(100)]
    public string? ActionName { get; set; }
    public virtual ICollection<RoleMenuPermission> RoleMenuPermissions { get; set; } = new List<RoleMenuPermission>();


}
