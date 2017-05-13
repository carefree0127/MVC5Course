using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using MVC5Course.Models.ViewModel;

namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{
        //加入一個欄位篩選資料
        public override IQueryable<Product> All()
        {
            return base.All().Where(p=>!p.is刪除);
        }

        //判斷是否要使用顯示所有資料
        public IQueryable<Product> All(bool ShowAll) {
            if (ShowAll)
            {
                return base.All();
            }
            else {
                return this.All();
            }
        }

        //API擴充
        public Product get單筆資料ByProductID(int id) {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public IQueryable<Product> getProduct列表頁所有資料(bool Active, bool ShowAll=false)
        {
            IQueryable<Product> all = this.All();//預設ShowAll = false
            if (ShowAll) { // 如果有要ShowAll的需求
                all = base.All();
            }
            /*
             .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10)
             */
            return all
                .Where(p => p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10); 
        }

        public void Update(Product product) {
            //db.Entry(product).State = EntityState.Modified;
            this.UnitOfWork.Context.Entry(product).State = EntityState.Modified;
        }

        public override void Delete(Product entity)
        {
            //關閉驗證
            //建議寫在Controller
            //this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;
            entity.is刪除 = true;
            //this.UnitOfWork.Commit(); //交由Controller處理
        }
    }

	public  interface IProductRepository : IRepository<Product>
	{

	}
}