using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ValidationAttribute
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class MaxWordLenAttrbute: DataTypeAttribute //繼承 :DataTypeAttribute
    {
        public MaxWordLenAttrbute(int maxWords) : base("{0} has too much words.")
        {
            _maxWords = maxWords;
        }

        //ValidationResult
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var strValue = (string)value;
            if(strValue.Split(' ').Length > _maxWords)
            {
                var errMsg = "has too much words";
                return new ValidationResult(errMsg);
            }
            return ValidationResult.Success;
        }

        private readonly int _maxWords;
    }
}