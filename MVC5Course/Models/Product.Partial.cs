namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using ValidationAttribute;

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
        //擴充屬性
        public int 訂單數量 {
            get {
                //效能問題
                return this.OrderLine.Count; //需注意當延遲載入功能被關閉時會有問題
                //return this.OrderLine.Where(p => p.Qty > 400).Count;
                //return this.OrderLine.Where(p => p.Qty > 400).ToList().Count;
                //return this.OrderLine.Count(p => p.Qty > 400);
            }
        }

        public DateTime 建立時間
        {
            get
            {
                return this.CreatedOn;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Price > 100 && this.Stock > 5)
            {
                yield return new ValidationResult("價格與庫存數量不合理",
                    new string[] { "Price", "Stock" });
            }

            using (var db = new FabricsEntities())
            {
                var prod = db.Product.FirstOrDefault(p => p.ProductId == this.ProductId);
                if (prod.OrderLine.Count() > 5 && this.Stock == 0)
                {
                    yield return new ValidationResult("Stock 與訂單數量不匹配",
                        new string[] { "Stock" });
                }
            }

            yield break;
        }
    }
    
    public partial class ProductMetaData //放驗證屬性
    {
        [Required]
        public int ProductId { get; set; }

        //自訂驗證屬性，後端驗證
        [商品名稱必須包含Ruby字串(ErrorMessage ="商品名稱必須包含Ruby字串")]
        [MaxWordLenAttrbute(10)]
        [Required(ErrorMessage = "請輸入商品名稱")]
        [DisplayName("商品名稱")]
        //[MinLength(3), MaxLength(30)]
        //[RegularExpression("(.+)-(.+)", ErrorMessage = "商品名稱格式錯誤")]
        public string ProductName { get; set; }
        [Required]
        [Range(0, 999999, ErrorMessage = "請設定正確的商品價格範圍")]
        public Nullable<decimal> Price { get; set; }
        [Required]
        public Nullable<bool> Active { get; set; }
        [Required]
        //[Range(0, 100, ErrorMessage = "請設定正確的商品庫存數量")]
        public Nullable<decimal> Stock { get; set; }


        //[StringLength(80, ErrorMessage="欄位長度不得大於 80 個字元")]
        //[DisplayName("商品名稱")]
        //public string ProductName { get; set; }

        ////整個網站都不顯示小數點的時候，可在此自訂顯示的格式
        //[DisplayFormat(DataFormatString="{0:N0}",ApplyFormatInEditMode=true)]
        //[DisplayName("商品價格")]
        //public Nullable<decimal> Price { get; set; }
        //[DisplayName("是否上架")]
        //public Nullable<bool> Active { get; set; }
        //[DisplayName("商品庫存")]
        //public Nullable<decimal> Stock { get; set; }

        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
