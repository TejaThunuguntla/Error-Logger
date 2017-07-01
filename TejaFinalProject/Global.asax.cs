using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TejaFinalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    HttpContext httpContext = HttpContext.Current;
        //    if (httpContext != null)
        //    {
        //        RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
        //        /* when the request is ajax the system can automatically handle a mistake with a JSON response. then overwrites the default response */
        //        if (requestContext.HttpContext.Request.IsAjaxRequest())
        //        {
        //            httpContext.Response.Clear();
        //            string controllerName = requestContext.RouteData.GetRequiredString("controller");
        //            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
        //            IController controller = factory.CreateController(requestContext, controllerName);
        //            ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

        //            JsonResult jsonResult = new JsonResult();
        //            jsonResult.Data = new { success = false, serverError = "500" };
        //            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //            jsonResult.ExecuteResult(controllerContext);
        //            httpContext.Response.End();
        //        }
        //        else
        //        {
        //            string status = httpContext.Response.Status;

        //            var filename = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["logfile"].ToString(); //"App_Data\\" + "logErrors.txt";
        //            var sw = new System.IO.StreamWriter(filename, true);                   
        //            Exception exc = Server.GetLastError();
        //            sw.Write(DateTime.Now + " ");
        //            if (exc.InnerException != null)
        //            {
        //                sw.Write("Inner Exception Type: ");
        //                sw.WriteLine(exc.InnerException.GetType().ToString());
        //                sw.Write("Inner Exception: ");
        //                sw.WriteLine(exc.InnerException.Message);
        //                sw.Write("Inner Source: ");
        //                sw.WriteLine(exc.InnerException.Source);
        //                if (exc.InnerException.StackTrace != null)
        //                {
        //                    sw.WriteLine("Inner Stack Trace: ");
        //                    sw.WriteLine(exc.InnerException.StackTrace);
        //                }
        //            }
        //            sw.Write("Exception Type: ");
        //            sw.WriteLine(exc.GetType().ToString());
        //            sw.WriteLine("Exception: " + exc.Message);
                    
        //            sw.Close();
        //            string BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority);
        //            httpContext.Response.Redirect(BASE_URL + "/Error/Error");
        //        }
        //    }
        //}

        protected void Application_Error(object sender, EventArgs e)
        {
            //Response.Redirect("~/Error/Error");
            try {
                HttpContext httpContext = HttpContext.Current;
                string status = httpContext.Response.Status;
                var filename = HttpContext.Current.Server.MapPath("~/Logs/log.txt");
                //var filename = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["logfile"].ToString(); //"App_Data\\" + "logErrors.txt";
                var sw = new System.IO.StreamWriter(filename, true);
                Exception exc = Server.GetLastError();
                sw.Write(DateTime.Now + " ");
                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }
                }
                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);

                sw.Close();
                //string BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath.TrimEnd('/');
                Response.Redirect("~/Error/Error");
            }
            catch(Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                    Response.Redirect("~/Error/Error");
                //string BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority);
                Response.Redirect("~/Error/Error");
            }
        }
    }
}
