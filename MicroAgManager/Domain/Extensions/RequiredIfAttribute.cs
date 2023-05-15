using System.ComponentModel.DataAnnotations;

namespace Domain.Extensions;
//create data annotations for required if field is true
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _otherProperty;
    private readonly object _otherPropertyValue;
    public RequiredIfAttribute(string otherProperty, object otherPropertyValue)
    {
        _otherProperty = otherProperty;
        _otherPropertyValue = otherPropertyValue;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);
        if (otherPropertyInfo == null)
        {
            return new ValidationResult($"Unknown property {_otherProperty}");
        }
        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
        if (Equals(otherPropertyValue, _otherPropertyValue))
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        return ValidationResult.Success;
    }
}