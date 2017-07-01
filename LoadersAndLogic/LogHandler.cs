using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadersAndLogic
{
    using ErrorLoggerModel;
    using Utilities;
    public class LogHandler
    {
        public LogHandler()  {  }

        public List<Log> GetLogs()
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Logs.Include("App").ToList();
            }
        }

        public IQueryable<Log> GetLogss()
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Logs.Include("App");
            }
        }

        public List<Log> GetLogsByAppID(int appId)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Logs.Include("App").Where(x => x.AppId == appId).ToList();
            }
        }

        public Log GetLogByLogID(int logID)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Logs.Where(x => x.LogId == logID).ToList().First();
            }
        }

        public bool ChangeApplication(int oldId,int newId)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                db.Logs.Where(x => x.LogId == oldId).ToList()
                       .ForEach(x => x.LogId = newId);
                db.SaveChanges();
            }
            return true;
        }

        public void SaveLog(Log log)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                db.Logs.Add(log);
                db.SaveChanges();
            }
        }

        public void SaveLog(LogMV logMV)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                if (db.Applications.Any(x => x.AppId == logMV.AppId))
                {
                    Log log = new Log()
                    {
                        AppId = logMV.AppId,
                        description = logMV.description,
                        exception = logMV.exception,
                        timestamp = logMV.timestamp,
                        type = (Log.Type)logMV.type
                    };
                    db.Logs.Add(log);
                    db.SaveChanges();
                }
            }
        }
    }
}
