using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErrorLoggerModel;
using LoadersAndLogic;

namespace TejaFinalProject.Controllers
{
    using Microsoft.AspNet.Identity;
    using PagedList;
    using System.Web.Helpers;

    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private static int currentApp;
        private static IPagedList<Log> currentLogs;

        public ActionResult Index()
        {
            return View(); 
        }

        [HttpGet]
        public ActionResult AddApplication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddApplication(string AppName)
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            appHandler.AddApplication(AppName);
            TempData["AddApp"] = AppName;
            return RedirectToAction("ViewApplications", new { status = 1 });
        }
        
        public ActionResult ViewApplications(int? status, string sortOrder, int? page)
        {
            status = status ?? 1;
            ViewBag.Status = status;
            ApplicationHandler appHandler = new ApplicationHandler();
            ICollection<Application> apps = appHandler.GetApplications(status);
            ViewBag.AppSort = String.IsNullOrEmpty(sortOrder) ? "app_desc" : "";
            switch(sortOrder)
            {
                case "app_desc":
                    apps = apps.OrderByDescending(x => x.AppName).ToList();
                    break;
                default:
                    apps = apps.OrderBy(x => x.AppName).ToList();
                    break;
            }
            int pagenum = (page ?? 1);
            return View(apps.ToPagedList(pagenum, 5));
        }

        [HttpGet]
        public ActionResult EditApplication(int appid)
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            Application app = appHandler.GetApplicationById(appid);
            return View(app);
        }

        [HttpPost]
        public ActionResult EditApplication(int appid, string appname)
        {
            ErrorLogModel db = new ErrorLogModel();
            ApplicationHandler appHandler = new ApplicationHandler();
            Application app = db.Applications.Single(x => x.AppId == appid);
            appHandler.EditName(appid, appname);

            List<SelectListItem> list = new List<SelectListItem>();
            foreach(User us in app.users)
            {
                var sl = new SelectListItem()
                {
                    Text = us.firstname,
                    Value = us.UserId.ToString()
                };
                list.Add(sl);
            }
            ViewBag.Users = list;
            return RedirectToAction("ViewApplications");
        }

        [HttpGet]
        public ActionResult RemoveUser(int appid,int? page)
        {
            UserHandler userhandler = new UserHandler();
            List<User> users = userhandler.GetUserByApplication(appid);
            int pagenum = (page ?? 1);
            ViewBag.Application = appid;
            return View(users.ToPagedList(pagenum, 5));
        }

        public ActionResult Remove(int userid)
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            var queryCollection = System.Web.HttpUtility.ParseQueryString(HttpContext.Request.UrlReferrer.Query);
            string param = queryCollection.Get("appid");
            appHandler.RemoveUser(int.Parse(param), userid);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public ActionResult AddUser(int appid, int? page)
        {
            UserHandler userhandler = new UserHandler();
            List<User> users = userhandler.GetUsersNotByApplication(appid);
            int pagenum = (page ?? 1);
            ViewBag.Application = appid;
            return View(users.ToPagedList(pagenum, 5));
        }

        public ActionResult Add(int userid)
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            var queryCollection = System.Web.HttpUtility.ParseQueryString(HttpContext.Request.UrlReferrer.Query);
            string param = queryCollection.Get("appid");
            appHandler.AddUser(int.Parse(param), userid);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Disable(int appid)
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            appHandler.DisableApplication(appid);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ViewUsers(int status,int? page)
        {
            UserHandler userHandler = new UserHandler();
            ICollection<User> users = userHandler.GetUsers(status);
            int pagenum = (page ?? 1);
            ViewBag.Status = status;
            return View(users.ToPagedList(pagenum, 3));
        }

        [Authorize]
        public ActionResult ToggleUser(int id)
        {
            UserHandler userHandler = new UserHandler();
            userHandler.ToggleUser(id);
            return Redirect(Request.UrlReferrer.ToString());
        }
        
        public ActionResult ViewLogs(int appid, string sortOrder, string currentFilter, string searchString, int? page)
        {
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
            if (!String.IsNullOrEmpty(searchString))
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
            currentLogs = logs.ToPagedList(pagenum, pagesize);
            return View(logs.ToPagedList(pagenum, pagesize));
        }

        [Authorize]
        public ActionResult DispChart()
        {
            ApplicationHandler appHandler = new ApplicationHandler();
            int appid = currentApp;
            var data = currentLogs.GroupBy(x => x.type).Select(y => new { Name = y.Key, Data = y.Count() });
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
}