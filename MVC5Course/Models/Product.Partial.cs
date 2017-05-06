namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
    }
    
    public partial class ProductMetaData
    {
        [Required]
        public int ProductId { get; set; }
        
        [StringLength(80, ErrorMessage="欄位長度不得大於 80 個字元")]
        [DisplayName("商品名稱")]
        public string ProductName { get; set; }

        //整個網站都不顯示小數點的時候，可在此自訂顯示的格式
        [DisplayFormat(DataFormatString="{0:N0}",ApplyFormatInEditMode=true)]
        [DisplayName("商品價格")]
        public Nullable<decimal> Price { get; set; }
        [DisplayName("是否上架")]
        public Nullable<bool> Active { get; set; }
        [DisplayName("商品庫存")]
        public Nullable<decimal> Stock { get; set; }
    
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
