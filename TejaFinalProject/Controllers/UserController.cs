using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Helpers;

namespace TejaFinalProject.Controllers
{
    using ErrorLoggerModel;
    using LoadersAndLogic;
    using PagedList;

    [Authorize]
    public class UserController : Controller
    {
        LogHandler data = new LogHandler();
        static private int currentApp;
        static List<Application> currentList = new List<Application>();

        [AllowAnonymous]
        public ActionResult Create(string usermail)
        {
            User user = new User();
            user.mailID = usermail;
            user.LastLogin = DateTime.Now;
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            User user = new User()
            {
                mailID = Convert.ToString(form["mailID"]),
                firstname = Convert.ToString(form["firstname"]),
                lastname = Convert.ToString(form["lastname"]),
                phone = Convert.ToString(form["phone"]),
                Status = true,
                LastLogin = DateTime.Now,
            };
            UserHandler handler = new UserHandler();
            handler.AddUser(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ViewApplications(string sortOrder, int? page)
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            UserHandler userhandler = new UserHandler();
            User user = userhandler.GetUserByMailId(User.Identity.GetUserName());
            List<Application> applist = appHandler.GetApplicationByUser(user.UserId);
            ViewBag.AppSort = String.IsNullOrEmpty(sortOrder) ? "app_desc" : "";

            switch (sortOrder)
            {
                case "app_desc":
                    applist = applist.OrderByDescending(x => x.AppName).ToList();
                    break;
                default:
                    applist = applist.OrderBy(x => x.AppName).ToList();
                    break;
            }
            currentList = applist;
            int pagenum = (page ?? 1);
            return View(applist.ToPagedList(pagenum, 5));
        }

        public ActionResult ViewLogs(int appid, string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (!currentList.Exists(x => x.AppId == appid))
            {
                ViewBag.accessdenied = "Access Denied.";
                var temp = new List<Log>();
                return View(temp.ToPagedList(1, 5));
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TypeSort = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewBag.DateSort = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.ExpSort = sortOrder == "exp" ? "exp_desc" : "exp";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            LogHandler loghandler = new LogHandler();
            List<Log> logs = loghandler.GetLogsByAppID(appid);
            if(!String.IsNullOrEmpty(searchString))
            {
                logs = logs.Where(x => x.description.ToLower().Contains(searchString.ToLower())).ToList();
            }
            switch (sortOrder)
            {
                case "type_desc":
                    logs = logs.OrderByDescending(x => x.type).ToList();
                    break;
                case "date":
                    logs = logs.OrderBy(x => x.timestamp).ToList();
                    break;
                case "date_desc":
                    logs = logs.OrderByDescending(x => x.timestamp).ToList();
                    break;
                case "exp":
                    logs = logs.OrderBy(x => x.exception).ToList();
                    break;
                case "exp_desc":
                    logs = logs.OrderByDescending(x => x.exception).ToList();
                    break;
                default:
                    logs = logs.OrderBy(x => x.type).ToList();
                    break;
            }
            ViewBag.application = appid;
            ViewBag.appname = ApplicationHandler.GetAppName(appid);
            currentApp = appid;
            int pagesize = 5;
            int pagenum = (page ?? 1);
            ViewBag.currentpage = pagenum;
            return View(logs.ToPagedList(pagenum, pagesize));
        }

        public ActionResult DispChart()
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            int appid = currentApp;
            using (var db = new ErrorLogModel())
            {
                var data = db.Logs.Where(r => r.AppId == appid).GroupBy(x => x.type).Select(y => new { Name = y.Key, Data = y.Count() });
                Chart ch = new Chart(300, 300);
                string[] xval = new string[3];
                int[] yval = new int[3];
                int i = 0;
                foreach (var d in data)
                {
                    xval[i] = d.Name.ToString();
                    yval[i] = d.Data;
                    i++;
                }

                ch.AddSeries("Default", chartType: "Pie",
                    xValue: xval, yValues: yval).Write("png");

                ch.Save("~/Content/chart", "png");
                return File("~/Content/chart", "png");
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("ViewApplications");
        }
    }
}