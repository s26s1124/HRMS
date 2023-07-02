using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using HumanResource.Models;

namespace HumanResource.Models.HRMS;


public partial class StrItemCode
{
    [Key]
    public int ItemCode { get; set; }


    [StringLength(100)]
    [DisplayName("Item Name")]
    [Required(ErrorMessage = "This Item Name Field is required.")]
    public string? ItemName { get; set; }

  
    [StringLength(300)]
    [DisplayName("Item Description")]
    [Required(ErrorMessage = "This Item Description Field is required.")]
    public string? ItemDesc { get; set; }

    
    [StringLength(100)]
    [DisplayName("Item Brand")]
    public string? ItemBrand { get; set; }

    [StringLength(100)]
    [DisplayName("Item Model")] 
    public string? ItemModel { get; set; }

    [ForeignKey("StrCategory")]
    [DisplayName("Category Name")]
    [Required(ErrorMessage = "This Category Name Field is required.")]
    public int CategoryCode { get; set; }
    public virtual StrCategoryCode? StrCategory { get; set; }

    public int TotalQuaAva { get; set; }

    [DisplayName("Maximum Quantity")]
    [Required( ErrorMessage = "This Maximum Quantity Field is required.")]
    public int MaxQau { get; set; }

    [DisplayName("Minimum  Quantity")]
    [Required(ErrorMessage = "This Minimum Quantity Field is required.")]
    [QauLessThanAttribute("MaxQau", ErrorMessage = "Minumm qunitity must be less tahn Max value")]
    public int MinQau { get; set; }

    [DisplayName("Number to Request")]
    [Required(ErrorMessage = "This Number to Request Field is required.")]
    public int? NoToReq { get; set; }

    public int? ItemDetDeactivated { get; set; }


    public class QauLessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public QauLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            if (value == null)
                return new ValidationResult("This Minimum Quantity Field is required.");
     
            var currentValue = (int)value!;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                return new ValidationResult("Property with this name not found");

            var comparisonValue = (int)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }


}
