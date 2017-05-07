using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Services.Description;

namespace MVC5Course.Models.ViewModel
{
    public class ProductLiteVM
    {
        /// <summary>
        /// 這是一個精簡版的 Prodoct 資料，主要用於建立商品資料
        /// </summary>
        public int ProductId { get; set; }

        //自訂驗證
        //[FabricsRequired(_Type=1,ErrorMessage="欄位為必填")]
        [Required]
        [MinLength(5)]
        public string ProductName { get; set; }

        [Required]
        public Nullable<decimal> Price { get; set; }

        [Required]
        public Nullable<decimal> Stock { get; set; }
    }
}