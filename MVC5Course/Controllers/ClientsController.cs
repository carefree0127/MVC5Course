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
    public class ClientsController : Controller
    {
        #region 建立步驟
        /*
        1. Controller建立，調整
        2. View調整
        3. ViewModel建立，調整
        */
        #endregion
        private FabricsEntities db = new FabricsEntities();

        // GET: Clients
        public ActionResult BatchUpdate()
        {
            GetClients();
            return View();
        }

        private void GetClients()
        {
            var client = db.Client.Include(c => c.Occupation).Take(10);
            ViewData.Model = client;
        }

        [HttpPost]
        public ActionResult BatchUpdate(ClientBatchUpdateVM[] items)
        {
            if (ModelState.IsValid) {
                foreach (var item in items)
                {
                    var c = db.Client.Find(item.ClientId);
                    c.FirstName = item.FirstName;
                    c.MiddleName = item.MiddleName;
                    c.LastName = item.LastName;

                }

                db.SaveChanges();

                return RedirectToAction("BatchUpdate");
            }


            GetClients();

            return View();
        }
        
    }
}
