
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 勿删_登陆注册cookie.Controllers
{
    // 标记这个页面不需要校验 , 不做登录校验的第一种方式
    //[LoginCheckFilter(IsChecked = false)]
    [ControllerAllowOrigin(AllowSites = new string[] { "http://localhost:14709" })]
    public class UserLoginController : BaseController
    {

        //   public IUserInfoService UserInfoService { get; set; }

        public UserLoginController()
        {
            this.IsCheckUserLogin = false;// 不做登录校验的 第二种方式
                                          //    UserInfoService = new UserInfoService();
        }


        //public UserInfoService

        // GET: UserLogin
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ShowVCode()
        //{

        //    YZMHelper vCode = new YZMHelper();
        //    string vCodeText = vCode.Text;
        //    byte[] imageArray = vCode.ImageByteArray;
        //    Session["VCode"] = vCodeText;

        //    return File(imageArray, @"image/jpeg");

        //}
        //   /UserLogin/ProcessLogin

        public ActionResult ProcessLogin(/*string VCode,*/ string loginName, string Password)
        {

            // 1.  处理验证码
            // 拿到表单中的验证码

            //string sessionCode = Session["VCode"] as string;
            //Session["VCode"] = null;
            //if (string.IsNullOrEmpty(sessionCode) || VCode != sessionCode)
            //{
            //    return Json("验证码错误!");
            //}
            // 2.  处理验证用户名, 密码

            //var userInfo = UserInfoService.GetEntities(u => u.UName == loginName && u.Password == Password).FirstOrDefault();
            var userInfo = loginName == "hello" ? new UserInfo() { Name = "hello" } : null;

            if (userInfo == null)
            {
                return Json(new { code = -1, msg = "用户名密码错误! from server" });
            }

            //   Session["loginUser"] = loginName;
            //用memcache + cookie 代替session
            // 立即分配一个标志, GUID. 把标志 作为 mm存储数据的key, 把对象放到mm, 把guid 写到客户端cookie 里面取
            string userLoginId = Guid.NewGuid().ToString();

            // 把 userLoginId 写到mm里面, 怎么解决变化: 写到不同的地方, 可能同时写入多个地方
            Common.Cache.CacheHelper.AddCache(userLoginId, userInfo, DateTime.Now.AddMinutes(20));

            // 给浏览器 设置cookie, 没有设置会话时间, 默认一次会话(浏览器关闭就没了)
            //Response.Cookies["userLoginId"].Value = userLoginId;
            HttpContext.Response.Cookies.Add(new HttpCookie("userLoginId", userLoginId)
            {
                HttpOnly = false
            });


            // 3. 如果正确, 跳转到 首页
            return Json(new { code = 1, msg = "登陆成功 from server!" }, JsonRequestBehavior.AllowGet);
        }
        //[ControllerAllowOrigin(AllowSites = new string[] { "http://localhost:14709" })]
        public ActionResult ProcessLogout()
        {
            string userGUID = Request.Cookies["userLoginId"].Value;

            Common.Cache.CacheHelper.RemoveCache(userGUID);
            return Json(new { code = 0, msg = "ok" }, JsonRequestBehavior.AllowGet);
        }

    }
}