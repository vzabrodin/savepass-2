using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DevExpress.Mvvm;

namespace Zabrodin.SavePass.Validation
{
    public abstract class ValidationBase : BindableBase, IDataErrorInfo
    {
        string IDataErrorInfo.Error => throw new NotSupportedException(
            "IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead");

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                if (string.IsNullOrEmpty(propertyName))
                    throw new ArgumentException("Invalid property name", propertyName);

                string error = string.Empty;
                object value = GetValue(propertyName);
                List<ValidationResult> results = new List<ValidationResult>(1);
                bool result = Validator.TryValidateProperty(value,
                    new ValidationContext(this, null, null) { MemberName = propertyName },
                    results);

                if (result)
                    return error;

                ValidationResult validationResult = results.First();
                error = validationResult.ErrorMessage;
                return error;
            }
        }

        private object GetValue(string propertyName) => GetType().GetProperty(propertyName)?.GetValue(this);
    }
}