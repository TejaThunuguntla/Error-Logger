using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ErrorLoggerModel;
using System.Threading;

namespace LoggerService.Controllers
{
    using LoadersAndLogic;
    using Utilities;

    public class ValuesController : ApiController
    {
        private BlockingQueue<Log> blkQ = new BlockingQueue<Log>();

        [HttpPost]
        public void PostException(LogMV log)
        {
            LogHandler logHandler = new LogHandler();
            logHandler.SaveLog(log);
        }

        private void SaveToDB()
        {
            while(true)
            {
                if(blkQ.Size() > 0)
                {
                    var toreq = blkQ.Dequeue();

                    Thread thread = new Thread(x =>
                    {
                        using (var db = new ErrorLogModel())
                        {
                            db.Logs.Add(toreq);
                            db.SaveChanges();
                        }
                    });
                    thread.Start();
                    thread.Join();
                }
            }
        }
    }
}
