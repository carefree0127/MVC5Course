using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : Controller
    {
        private FabricsEntities db = new FabricsEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        #region 練習 FileResult 使用方式
        public ActionResult GetFile()
        {
            ////讀取圖片
            //return File(Server.MapPath("~/Content/20170513.png"), "image/png");
            //強迫下載圖片，並改名
            return File(Server.MapPath("~/Content/20170513.png"), "image/png", "NewName");
        }
        #endregion

        #region 練習 JsonResult 使用方式
        public ActionResult GetJson()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Json(db.Product.Take(5), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}