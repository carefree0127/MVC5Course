using System.Web;
using System.Web.Mvc;

namespace MVC5Course
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //可以套用在Controller、Action，不需要做額外的設定
            filters.Add(new HandleErrorAttribute());
        }
    }
}
