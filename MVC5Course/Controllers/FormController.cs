using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class FormController : Controller
    {
        private FabricsEntities db = new FabricsEntities();

        // GET: Form
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            //有傳參數就有ModelBinding
            return View(db.Product.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(int id,FormCollection form)
        {
            //ModelBinding ModelState 優先權較高
            var product = db.Product.Find(id);

            if (TryUpdateModel(product, includeProperties: new string[] { "ProductName" }))
            {//只針對ProductName作ModelBinding
                db.SaveChanges();
                return View("Index");
            }

            product = db.Product.Find(id);
            return View(product);
        }
    }
}