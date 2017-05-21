using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ValidationAttribute
{
    public class 商品名稱必須包含Ruby字串Attribute:DataTypeAttribute
    {
        public 商品名稱必須包含Ruby字串Attribute():base(DataType.Text)
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null) {
                return true;
            }
            var str = (string)value;
            return str.Contains("Ruby");
        }
    }

}