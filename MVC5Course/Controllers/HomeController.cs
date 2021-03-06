﻿using MVC5Course.Models;
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

        #region 自訂ActionFilter，可把共用的資料抽取出來，讓Controller更輕量
        [SharedViewBag(MyProperty = "SharedViewBag MyProperty Test")]
        [LocalOnly]//判斷是否在本機執行，若不是就導回首頁，可加在BaseController的Debug() <-練習待補
        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            //web.config
            throw new ArgumentException("Error Handle!!!");

            return View();
        }

        [SharedViewBag]
        public ActionResult PartialAbout()
        {
            //練習32待補
            //ViewBag.Message = "Your application description page.";
            if (Request.IsAjaxRequest())
            {
                return PartialView("About");
            }
            else
            {
                return View("About");
            }

        }
        #endregion

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

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

        #region 練習Razor
        public ActionResult VT()
        {
            ViewBag.IsEnabled = true;
            return View();
        }
        public ActionResult RazorTest()
        {
            int[] data = new int[] { 1, 2, 3, 4, 5 };

            return PartialView(data);
        }
        #endregion


    }
}