using HumanResource.Models.HRMS;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HumanResource.Models
{
    public class CustomModelValidatorProvider : IModelValidatorProvider
    {

        public void CreateValidators(ModelValidatorProviderContext context)
        {
            if (context.Results.Any(v => v.Validator.GetType() == typeof(MyCustomModelValidator)) == true)
            {
                return;
            }

            if (context.ModelMetadata.ContainerType == typeof(StrCategoryCode))
            {
                context.Results.Add(new ValidatorItem
                {
                    Validator = new MyCustomModelValidator(),
                    IsReusable = true
                });
            }
        }
    }

    public class MyCustomModelValidator : IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var model = context.Container as StrItemCode;


            if (context.ModelMetadata.ModelType == typeof(int)
                && context.ModelMetadata.Name == nameof(model.MinQau))
            {

                if (model.MinQau > model.MaxQau)
                {
                    return new List<ModelValidationResult>
                {
                   new ModelValidationResult("","Minimum quantity should be less than Max quantity")
                };
                }
            }
            //if (context.ModelMetadata.ModelType == typeof(int)
            //    && context.ModelMetadata.Name == nameof(model.MaxQau))
            //{

            //    if (model.MaxQau < model.ClosureDate)
            //    {
            //        return new List<ModelValidationResult>
            //    {
            //       new ModelValidationResult("","End Date should be grater than Closure date")
            //    };
            //    }
            //}

            return Enumerable.Empty<ModelValidationResult>();
        }
    }
}

