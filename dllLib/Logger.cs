using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dllLib
{
    using ErrorLoggerModel;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Utilities;

    public class Logger
    {
        public static int ApplicationId = 1;
        // public static int SERVICE_PORT = 2379;
        // public static string SERVICE_URL = "http://localhost:{0}/";
        public static string SERVICE_URL = "http://localhost/LoggerService/";
        public static string PARAM_URL = "Api/Values/PostException?log=";

        public void Initialize(int AppId)
        {
            ApplicationId = AppId;
        }

        public void Error(Exception ex)
        {
            LogMV log = new LogMV()
            {
                type = LogMV.Type.ERROR,
                AppId = ApplicationId,
                timestamp = DateTime.Now,
                description = ex == null ? "" : ex.Message,
                exception = ex == null ? "" : ex.GetType().Name
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(log);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            HttpClient client = new HttpClient();
            //  client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));
            client.BaseAddress = new Uri(String.Format(SERVICE_URL));
            var task = client.PostAsync(PARAM_URL, content).Result;
        }

        public void Debug(Exception ex)
        {
            LogMV log = new LogMV()
            {
                type = LogMV.Type.DEBUG,
                AppId = ApplicationId,
                timestamp = DateTime.Now,
                description = ex == null ? "" : ex.Message,
                exception = ex == null ? "" : ex.GetType().Name
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(log);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));
            client.BaseAddress = new Uri(String.Format(SERVICE_URL));
            var task = client.PostAsync(PARAM_URL, content).Result;
        }

        public void Warning(Exception ex)
        {
            LogMV log = new LogMV()
            {
                type = LogMV.Type.WARNING,
                AppId = ApplicationId,
                timestamp = DateTime.Now,
                description = ex == null ? "" : ex.Message,
                exception = ex == null ? "" : ex.GetType().Name
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(log);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));
            client.BaseAddress = new Uri(String.Format(SERVICE_URL));
            var task = client.PostAsync(PARAM_URL, content).Result;
        }
    }
}
