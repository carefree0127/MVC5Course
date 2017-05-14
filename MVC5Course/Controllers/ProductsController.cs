using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModel;

namespace MVC5Course.Controllers
{
    //[Authorize]//不要再用Session執行檢查有沒有登入
    public class ProductsController : Controller
    {
        //ADO.NET Data Provider
        //private FabricsEntities db = new FabricsEntities();  

        //加入Repository
        ProductRepository repo = RepositoryHelper.GetProductRepository();

        #region 20170507 用ViewModel建立資料
        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(
            [Bind(Include = "ProductName,Price,Stock")]//只接受這些欄位的model binding，避免前端手動新增欄位
            ProductLiteVM data)//指定Bind欄位model不要加驗證 -> 維護性需要考量
        {
            if (ModelState.IsValid) {//下中斷點偵錯，可查看data接到什麼資料

                TempData["CP_Result"] = "商品資料新增成功";

                //TODO: 儲存資料進資料庫
                return RedirectToAction("ListProducts");
            }

            //驗證失敗，繼續顯示原本的表單
            return View();
        }
        #endregion

        #region 20170506: 建立時預設的
        // GET: Products
        [OutputCache(Duration = 300,Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        //Cache使用要小心，適當使用可以提升效能
        public ActionResult Index(bool Active = true)//model binding
        {
            #region EF取資料語法 篩選條件
            //return View(db.Product.ToList());
            //return View(db.Product.Take(10));//取10筆資料

            //var data = db.Product
            //    .Where(p => p.Active.HasValue && p.Active.Value == Active)
            //    .OrderByDescending(p => p.ProductId).Take(10);
            #endregion
            
            //repo.All() //取得所有資料
            //var data = repo.All()
            //           .Where(p => p.Active.HasValue && p.Active.Value == Active)
            //           .OrderByDescending(p => p.ProductId).Take(10);
            //將邏輯移到ProductRepository
            var data = repo.getProduct列表頁所有資料(Active);//showAll: false 具名參數的使用
            //return View(data);
            
            #region 把資料傳到View裡使用的方式有四種            
            //強型別
            ViewData.Model = data;
            //弱型別 以下三種寫法      
            ViewData["ppp"] = data;
            ViewBag.qqq = data;
            //-----上面兩個弱型別骨子裡一樣
            TempData["zzz"] = data;
            //被讀過一次就會被刪除，通常用於表單新增成功，POST導頁，骨子裡是Session
            #endregion


            return View(); //View中拿到的 @Model即是 data，資料型別會一致
        }

        // GET: Products/Details/5
        // GET: Products/Details?id=5 -> QueryString 
        public ActionResult Details(int? id)//Form Name叫id也接的到 -> model binding模型繫結(把資料寫入模型的參數裡)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Product product = db.Product.Find(id);
            //找到 ProductRepository 把邏輯移進去
            Product product = repo.get單筆資料ByProductID(id.Value);


            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        //顯示表單
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        //接表單資料
        [HttpPost]//區分執行哪一個Action
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)//用表單的Name屬性
        {
            if (ModelState.IsValid)//資料驗證 -> model
            {
                // 與Entity FrameWork相關的語法
                //db.Product.Add(product);
                //db.SaveChanges();
                //
                //改用Repository
                repo.Add(product);
                repo.UnitOfWork.Commit();
                //

                //新增成功訊息在Index頁顯示
                TempData["CP_Result"] = "商品資料新增成功";
                return RedirectToAction("ListProducts");
            }

            ViewBag.product = product;//弱型別，動態，沒有提示可用

            return View(product); //強型別，只能傳一個model，有多個model要傳遞的話要用ViewBag或者定義一個class去包
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Product product = db.Product.Find(id);
            //改用Repository
            Product product = repo.get單筆資料ByProductID(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        public ActionResult Edit(int id, FormCollection form)
        {
            #region ModelBinding 延遲驗證 public ActionResult Edit(int id, FormCollection form)
            var product = repo.get單筆資料ByProductID(id);
            if (TryUpdateModel<Product>(product,new string[] { "ProductId", "ProductName", "Price", "Active", "Stock" }))
                //注意Model Binding欄位要隔開，不能全部當一個字串
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            #endregion
            #region ModelBinding 強型別 public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
            //若需求改變，部分資料沒有要更新的話，那些資料會變為預設值，造成意外更新

            //if (ModelState.IsValid)
            //{
            //    //db.Entry(product).State = EntityState.Modified;
            //    //db.SaveChanges();
            //    //改用Repository 擴充ProductRepository Update方法
            //    repo.Update(product);
            //    repo.UnitOfWork.Commit();
            //    return RedirectToAction("Index");
            //}
            #endregion

            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Product product = db.Product.Find(id);
            //改用Repository
            Product product = repo.get單筆資料ByProductID(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Product product = db.Product.Find(id);
            //db.Product.Remove(product);
            //db.SaveChanges();
            //改用Repository
            Product product = repo.get單筆資料ByProductID(id);

            //關閉資料驗證
            repo.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            repo.Delete(product);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        #endregion

        #region 20170506: 使用ViewModel，並透過Entity Framework查詢資料
        //ViewModel的使用，注意ViewModel新增時資料內容類別要清空
        //FormCollection要用HTTP POST
        //LINQ TO ENTITY轉型失敗 -> 先用區域變數存資料
        public ActionResult ListProducts(ProductSearchVM Mysearch)//表單送出，只要有ModelBinding就會有ModelState -> 才會套用到HTML
                                          //弱型別 string q,int Stock_S=0,int Stock_E=9999
        {
            #region 使用EF搭配LINQ取得資料
            //var data = db.Product
            //    .Where(p => p.Active.Value == true)
            //    .Select(p => new ProductLiteVM() { 
            //        ProductId = p.ProductId,
            //        ProductName = p.ProductName,
            //        Price = p.Price,
            //        Stock = p.Stock
            //    })
            //    .Take(10);
            #endregion

            #region 改用Repository、在列表頁新增篩選條件(單一欄位搜尋、多欄位搜尋)
            var data = repo.getProduct列表頁所有資料(true);

            #region 弱型別與強型別的寫法
            //if (!String.IsNullOrEmpty(q))
            //{
            //    data = data.Where(p => p.ProductName.Contains(q));
            //}
            //data = data.Where(p => p.Stock > Stock_S && p.Stock < Stock_E);

            //if (ModelState.IsValid)//強型別判斷ModelState，如果有問題就不判斷
            //{
                if (!String.IsNullOrEmpty(Mysearch.q))
                {
                    data = data.Where(p => p.ProductName.Contains(Mysearch.q));
                }
                data = data.Where(p => p.Stock > Mysearch.Stock_S && p.Stock < Mysearch.Stock_E);

            //}

            #endregion

            ViewData.Model = data
                .Select(p => new ProductLiteVM()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock
                });//Select 可以寫在Repository

            #endregion

            return View();

        }
        #endregion
    }
}
