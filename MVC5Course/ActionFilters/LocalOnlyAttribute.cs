using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    internal class LocalOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsLocal)
            {
                //如果不是本機執行，就導回首頁
                filterContext.Result = new RedirectResult("/");
            }
        }
    }
}