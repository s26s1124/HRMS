using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Models.HRMS;


public partial class StrCategoryCode
{
    [Key]
    public int CategoryCode { get; set; }
   
    [DisplayName("Category Name")]
    [Required(ErrorMessage = "This Category Name Field is required.")]
    [StringLength(50)]
    public string? CategoryName { get; set; }

    [Display(Name = "Category Deactivated")]
    public int? CategoryDeactivated { get; set; }

    public virtual List<StrRequestDetails>? StrRequestDetails { get; set; }
    public virtual List<StrItemCode>? StrItemCodes { get; set; }

}
