using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Models.HRMS;

[PrimaryKey("EmpNo", "PageId")]
public class RoleMenuPermission
{

    public int EmpNo { get; set; }
   
    [ForeignKey("Page")]
    public int? PageId { get; set; }
    public virtual NavigationMenu Page { get; set; }
    public int? IsActivated { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? FromDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? ToDate { get; set; }

  
}