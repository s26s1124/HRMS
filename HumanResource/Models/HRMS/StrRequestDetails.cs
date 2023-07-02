using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using HumanResource.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace HumanResource.Models.HRMS;
[PrimaryKey("ReqNo", "ReqDetailNo")]
public partial class StrRequestDetails
{
    public int ReqDetailNo { get; set; }

    [ForeignKey("ReqNo")]
    public int ReqNo { get; set; }
    public virtual StrReqMstr StrReqMstr { get; set; } 

    [ForeignKey("StrCategory")]
    [DisplayName("Category Code")]
    [Required(ErrorMessage = "This Category Code Field is required.")]
    public int CategoryCode { get; set; }

    public virtual StrCategoryCode StrCategory { get; private set; } 
    public int? DirectToPur { get; set; } = 0;

    [ForeignKey("StrItem")]
    [DisplayName("Item Code")]
    [Required(ErrorMessage = "This Item Code Field is required.")]
    public int ItemCode { get; set; }

    public virtual StrItemCode StrItem { get; private set; } 

    [Required(ErrorMessage = "The Item Quantity Field is required.")]
    [Range(1, 100, ErrorMessage = "The Item Quantity must be More than 1 and Less than 100.")]
    [LessThanDbValueAttribute(nameof(ItemCode), ErrorMessage = "Minumm qunitity must be less tahn Max value")]
    public int ItemQuantity { get; set; } = 1;


    [StringLength(200)]
    public string? ItemRemark { get; set; } = null;

    public int? ItemDelivery { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? ItemDeliveryDate { get; set; }

    public int? CancelFlage { get; set; } = 0;

    [Required(ErrorMessage = "This Field is required.")]

    public int? StrManagerAppr { get; set; } = 0;
   
    [StringLength(150)]
    public string? StrManagerApprReson { get; set; } = null;
    
    [Required(ErrorMessage = "This Field is required.")]
    public int? ItemQutReceived { get; set; } = 0;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? StrApprDate { get; set; }

    public int? IsItemRetrieve { get; set; } = 0;

   // [Required(ErrorMessage = "This Qutntity Retrieve Field is required.")]
    public int? QutRetrieve { get; set; } = 0;
  
    [StringLength(200)]
    //[Required(ErrorMessage = "This Retrieve Reason Field is required.")]
    public string? RetrieveReason { get; set; } = null;

    [NotMapped]
    public bool IsDeleted { get; set; }= false;




    public class LessThanDbValueAttribute : ValidationAttribute
    {
        private readonly hrmsContext _context; // Your database context
        public string _dbItemCode { get; }
        public string _dbItemQuantity { get; }

        public LessThanDbValueAttribute(string dbItemCode)//, string dbItemQuantity)
        {

            _dbItemCode = dbItemCode;
            //_dbItemQuantity = Convert.ToInt32(dbItemQuantity);
        }

        public string GetErrorMessage() =>
            $"The value must be less than {_dbItemQuantity}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Quantity = (StrRequestDetails)validationContext.ObjectInstance;


            if (Quantity.ItemCode.ToString() == _dbItemCode && Quantity.StrItem.TotalQuaAva > Convert.ToInt32(value.ToString()))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

    }

}
