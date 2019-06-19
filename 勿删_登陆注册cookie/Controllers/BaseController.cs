
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 勿删_登陆注册cookie.Controllers
{
    public class UserInfo
    {
        public string Name { get; set; }
    }
    public class BaseController : Controller
    {
        public bool IsCheckUserLogin = true;

        // 这里本来应该是一个对象的, 我这里简化成字符串了
        public UserInfo LoginUser { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {      


            // mvc 请求来了以后, 根据请求地址, 创建控制工厂, 控制器工厂创建控制器, 执行方法

            //Spring.Net 就是重写了创建控制器工程的类, 

            // 我们也可以自己写 控制器工厂, 有一个文章
            //  Custom Controller Factory in ASP.NET MVC
            base.OnActionExecuting(filterContext);

            //TODO 测试之后, 删除 return
            // return;
            if (IsCheckUserLogin)
            {
                #region 登录校验
                if (Request.Cookies["userLoginId"] == null)
                {

                    // 用户未登录
                    //  filterContext.HttpContext.Response.Redirect("/UserLogin/Index");
                 
                    filterContext.HttpContext.Response.Write(LitJson.JsonMapper.ToJson(new { code = -2, msg = "Not logged in from server" }));
                    filterContext.HttpContext.Response.End();

                    return;
                }
                string userGUID = Request.Cookies["userLoginId"].Value;


                UserInfo userInfo = Common.Cache.CacheHelper.GetCache(userGUID) as UserInfo;

                //从缓存中拿到当天用户的登录信息
                if (userInfo == null)
                {
                    // 用户超时了
                
                    filterContext.HttpContext.Response.Write(LitJson.JsonMapper.ToJson(new { code = -2, msg = "Not logged in from server" }));

                    filterContext.HttpContext.Response.End();
                    return;
                }

                //LoginUser = /*filterContext.HttpContext.Session["loginUser"] as UserInfo;*/
                LoginUser = userInfo;
                // 滑动窗口机制, 将登录过期时间 延长一段时间
                Common.Cache.CacheHelper.SetCache(userGUID, userInfo, DateTime.Now.AddMinutes(20));
                #endregion



                #region 权限校验
                //// 把当前其你去对应的权限数据拿到
                //if (LoginUser.UName == "admin")
                //{
                //    return;  // 后门
                //}

                //string url = Request.Url.AbsolutePath.ToLower();
                //string httpMethod = Request.HttpMethod.ToLower();

                //// 通过spring容器, 创建一个实例
                //IApplicationContext ctx = ContextRegistry.GetContext();
                //IActionInfoService actionInfoService = ctx.GetObject("ActionInfoService") as IActionInfoService;
                //IR_User_ActionService r_User_ActionService = ctx.GetObject("R_User_ActionService") as IR_User_ActionService;
                //IUserInfoService userInfoService = ctx.GetObject("UserInfoService") as IUserInfoService;
                //IRoleInfoService roleInfoService = ctx.GetObject("RoleInfoService") as IRoleInfoService;
                //var actionInfo = actionInfoService.GetEntities(a => a.URL.ToLower() == url && a.HttpMethod.ToLower() == httpMethod).FirstOrDefault();
                //if (actionInfo == null)
                //{
                //    //Response.
                //    Response.Redirect("/Error.html");
                //    return;
                //}
                //var rUAs = r_User_ActionService.GetEntities(u => u.UserID == LoginUser.ID);

                //#region 线路一  特殊权限
                //var item = (from a in rUAs
                //            where a.ActionID == actionInfo.ID
                //            select a).FirstOrDefault();
                //if (item != null)
                //{
                //    if (item.HasPermission == true)
                //    {
                //        return;
                //    }
                //    else
                //    {
                //        Response.Redirect("/Error.html");
                //        return;
                //    }
                //}
                //#endregion


                //#region 线路二 常规role 路线

                //var user = userInfoService.GetEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
                //var allRoles = user.R_User_Role.Select(u => u.RoleInfo);
                //var ations = allRoles.SelectMany(u => u.R_Role_Action.Select(t => t.ActionInfo));
                //// 只要
                //var temp = ations.Where(u => u.ID == actionInfo.ID).Count();

                //if (temp <= 0)
                //{
                //    Response.Redirect("/Error.html");
                //    return;
                //}



                //#endregion

                #endregion

            }



        }
        public class AllowOriginAttribute
        {
            public static void onExcute(ControllerContext context, string[] AllowSites)
            {
                var origin = context.HttpContext.Request.Headers["Origin"];
                Action action = () =>
                {
                    //当withCredentials设置为true时，后台跨域处理，头部“Access-Control-Allow-Origin” 不可以设置为 "*", 必须为确定的域名，
                    context.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", origin);
                    context.HttpContext.Response.AppendHeader("Access-Control-Allow-Credentials", "true");

                };
                if (AllowSites != null && AllowSites.Any())
                {
                    if (AllowSites.Contains(origin))
                    {
                        action();
                    }
                }
            }
        }
        public class ActionAllowOriginAttribute : ActionFilterAttribute
        {
            public string[] AllowSites { get; set; }
            public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
            {
                AllowOriginAttribute.onExcute(filterContext, AllowSites);
                base.OnActionExecuting(filterContext);
            }
        }
        public class ControllerAllowOriginAttribute : AuthorizeAttribute
        {
            public string[] AllowSites { get; set; }
            public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
            {
                AllowOriginAttribute.onExcute(filterContext, AllowSites);
            }

        }

    }
}