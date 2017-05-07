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
    public class ProductsController : Controller
    {
        private FabricsEntities db = new FabricsEntities();

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
                //TODO: 儲存資料進資料庫
                return RedirectToAction("ListProducts");
            }

            //驗證失敗，繼續顯示原本的表單
            return View();
        }
        #endregion

        #region 20170506: 建立時預設的
        // GET: Products
        public ActionResult Index(bool Active = true)//model binding
        {
            //return View(db.Product.ToList());
            //return View(db.Product.Take(10));//取10筆資料
            var data = db.Product
                .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10);
            return View(data); //View中拿到的 @Model即是 data，資料型別會一致
        }

        // GET: Products/Details/5
        // GET: Products/Details?id=5 -> QueryString 
        public ActionResult Details(int? id)//Form Name叫id也接的到 -> model binding模型繫結(把資料寫入模型的參數裡)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
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
                db.Product.Add(product);
                db.SaveChanges();
                //新增成功訊息在Index頁顯示
                TempData["Msg"] = "新增成功!";
                return RedirectToAction("Index");
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
            Product product = db.Product.Find(id);
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
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
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
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 20170506: 使用ViewModel，並透過Entity Framework查詢資料
        //ViewModel的使用，注意ViewModel新增時資料內容類別要清空
        public ActionResult ListProducts()
        {
            var data = db.Product
                .Where(p => p.Active.Value == true)
                .Select(p => new ProductLiteVM() { 
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock
                })
                .Take(10);
            return View(data);

        }
        #endregion
    }
}
