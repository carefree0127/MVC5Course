using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities(); //全類別共用

        #region 20170507: 商品CRUD

        #region 商品列表
        // GET: EF
        public ActionResult Index()
        {
            var all = db.Product.AsQueryable(); // 選用IEnumerable，當資料量大時會有效能問題

            var data = all.Where(p => p.isDeleted==false && p.Active == true && p.ProductName.Contains("Black"));

            //注意: 下面三種回傳型別
            //var data1 = all.Where(p => p.ProductId == 1);
            //var data2 = all.FirstOrDefault(p => p.ProductId == 1);
            //var data3 = db.Product.Find(1);

            return View(data);
        }
        #endregion

        #region 新增資料
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }
        #endregion

        #region 編輯資料
        public ActionResult Edit(int id)
        {
            var data = db.Product.Find(id);
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {

                var item = db.Product.Find(id);

                item.ProductName = product.ProductName;
                item.Price = product.Price;
                item.Active = product.Active;
                item.Stock = product.Stock;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        #endregion

        #region 刪除資料
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            var data = db.Product.Find(id);

            //因為會有一個orderLine關聯 (導覽屬性)
            //foreach (var item in data.OrderLine.ToList())
            //{
            //    db.OrderLine.Remove(item);
            //}

            //等於上面的foreach
            //db.OrderLine.RemoveRange(data.OrderLine);

            //if (data != null) {
            //db.Product.Remove(data);

            //改為修改isDeleted欄位
            data.isDeleted = true;

            //偵錯技巧，執行時頁面出現錯誤訊息可把Exception Catch下來
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

                throw;
            }

            
            return RedirectToAction("Index");
            //}

            //return View();
        }
        #endregion

        #region 單一商品列表
        public ActionResult Details(int id)
        {
            var data = db.Product.Find(id);
            return View(data);
        }
        #endregion

        #endregion



    }
}