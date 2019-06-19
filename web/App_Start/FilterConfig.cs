using System.Web;
using System.Web.Mvc;

namespace 勿删_登陆注册cookie
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
