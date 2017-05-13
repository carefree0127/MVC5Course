using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ViewModel
{
    public class ProductSearchVM: IValidatableObject
    {
        //給預設值
        public ProductSearchVM(){
            this.Stock_S = 0;
            this.Stock_E = 9999;
        }
        /*
         原本弱型別帶的參數 -> 轉換為Prop
         string q,int Stock_S=0,int Stock_E=9999
        */
        public string q { get; set; }
        public int Stock_S { get; set; }
        public int Stock_E { get; set; }


        #region 針對Stock_S、Stock_E欄位做驗證
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Stock_E < this.Stock_S) {
                yield return new ValidationResult("庫存資料篩選條件錯誤", new string[] { "Stock_S", "Stock_E" });
            }
        }
        #endregion
    }
}