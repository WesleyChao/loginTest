using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 勿删_登陆注册cookie.Controllers
{
    public class HomeController : BaseController
    {
        [ControllerAllowOrigin(AllowSites = new string[] { "http://localhost:14709" })]
        public ActionResult Index()
        {





            return Json(new { code = 0, msg = "I am home from server", data = "hello" }, JsonRequestBehavior.AllowGet);
        }

        [ControllerAllowOrigin(AllowSites = new string[] { "http://localhost:14709" })]
        public ActionResult GetCookie()
        {
            string userGUID = Request.Cookies["userLoginId"].Value;
            return Json(new { code = 0, msg = "ok", data = userGUID }, JsonRequestBehavior.AllowGet);
        }

    }
}