using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        private const string sessionKey = "myFormKey";

        public ActionResult Index()
        {
            Club club = null;
            if (Session[sessionKey] != null)
            {
                club = (Club) Session[sessionKey];
            }
            else
            {
                club = new Club();
            }
            return View(club);
        }

        //提交表单
        [HttpPost]
        public ActionResult Index(Club club)
        {
            if (ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(club.Name);

                if (club.Players != null && club.Players.Count > 0)
                {
                    foreach (var item in club.Players)
                    {
                        sb.AppendFormat("--{0}", item.Name);
                    }
                }
                //删除Session
                //Session.Abandon();
                //Session.Clear();
                Session.Remove(sessionKey);
                return Content(sb.ToString());
            }
            else
            {
                return View(club);
            }
        }

        //添加新行
        public ActionResult NewPlayerRow(Player player)
        {
            return PartialView("_NewPlayer", player ?? new Player());
        }

        //跳转之前把表单保存到Session中
        [HttpPost]
        public ActionResult BeforeGoToMustSave(Club club)
        {
            Session[sessionKey] = club;
            return Json(new { msg = true });
        }

        //保存完Club的Session后真正跳转到的页面
        public ActionResult RealGoTo()
        {
            return View();
        }

    }
}
