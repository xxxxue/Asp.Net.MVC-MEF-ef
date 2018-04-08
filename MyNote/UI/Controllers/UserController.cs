using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Dal;
using DAL.IDal;

namespace UI.Controllers
{
    [Export]
    public class UserController : BaseController
    {     

        /// <summary>
        /// UserDal
        /// </summary>
        [Import]
        public IUserDal UserDal { get; set; }

        /// <summary>
        /// 登录页面Page 
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginPage()
        {
            return View();
        }

        /// <summary>
        /// 登录       
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public ActionResult Login(string userName, string userPwd)
        {
            var res = UserDal.UserLogin(userName, userPwd);
            if (res != null)
            {
                Response.Redirect("/MyNote/MyNotePage");
            }

            return Content("<script>alert('帐号密码错误。');location.href='/User/LoginPage';</script>");
        }

    }
}