using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Models.HRMS;


public partial class StrReqMstr
{
    [Key]
    [Display(Name = "Request No.")]
    public int ReqNo { get; set; }
  
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Request Date")]
    public DateTime? ReqDate { get; set; }= DateTime.Now.Date;
   
    [ForeignKey("VempDtl")]
    [Display(Name = "Employee ID")]
    public int EmpNo { get; set; }
    //public virtual VempDtl? VempDtl { get; private set; }

    [Display(Name = "Manager Approval")]
    [Required(ErrorMessage = "The Manager Approval Field is required.")]

    public int? ManagrAppr { get; set; }
    
    public int? ManagrApprFlag { get; set; }

    [DataType(DataType.Date)]
   [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Manager Approval Date")]
    public DateTime? ManagrApprDate { get; set; }
    
    [Display(Name = "Store Manager Approval")]
    public int? StrManagrAppr { get; set; }

    public int? StrManagrApprFlag { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Store Manager Approval Date")]
    public DateTime? StrManagrApprDate { get; set; }
  
    public int?  DirectorApproval  { get; set; }
  
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? DirectorApprovalDate { get; set; }
  
    public int? CancelFlage { get; set; }=0;
    
    public int? UserId { get; set; }
   
    public int? DeliveryAppr { get; set; }
    
    public virtual List<StrRequestDetails> StrRequestDetails { get; set; } = new List<StrRequestDetails>();




}
