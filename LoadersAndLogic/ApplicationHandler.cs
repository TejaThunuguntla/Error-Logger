using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadersAndLogic
{
    using ErrorLoggerModel;

    public class ApplicationHandler
    {
        public void AddApplication(string Appname)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                Application app = new Application()
                {
                    AppName = Appname,
                    Enabled = true
                };
                db.Applications.Add(app);
                db.SaveChanges();
            }
        }

        public static string GetAppName(int appId)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                if (db.Applications.Any(x => x.AppId == appId))
                {
                    return db.Applications.FirstOrDefault(x => x.AppId == appId).AppName;
                }
                return "";
            }
        }
        
        public void EditName(int oldId, string newName)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                db.Applications.First(x => x.AppId == oldId).AppName = newName;
                db.SaveChanges();
            }
        }

        public bool AddUser(string app, int user)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                foreach (Application application in db.Applications.Where(x => x.AppName == app).ToList())
                {
                    if (application.users.Where(x => x.UserId == user).Count() == 0)
                    {
                        User us = db.Users.First(x => x.UserId == user);
                        application.users.Add(us);
                        db.SaveChanges();
                    }
                    else return false;
                }
            }
            return true;
        }

        public bool AddUser(int app, int user)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                Application application = db.Applications.First(x => x.AppId == app);

                if (application.users.Where(x => x.UserId == user).Count() == 0)
                {
                    User us = db.Users.First(x => x.UserId == user);
                    application.users.Add(us);
                    db.SaveChanges();
                }
                else return false;
            }
            return true;
        }

        public bool RemoveUser(string app, int user)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                foreach (Application application in db.Applications.Where(x => x.AppName == app).ToList())
                {
                    if (application.users.Where(x => x.UserId == user).Count() > 0)
                    {
                        User us = db.Users.First(x => x.UserId == user);
                        application.users.Remove(us);
                        db.SaveChanges();
                    }
                    else return false;
                }
            }
            return true;
        }

        public bool RemoveUser(int app, int user)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                Application application = db.Applications.First(x => x.AppId == app);

                if (application.users.Where(x => x.UserId == user).Count() > 0)
                {
                    User us = db.Users.First(x => x.UserId == user);
                    application.users.Remove(us);
                    db.SaveChanges();
                }
                else return false;
            }
            return true;
        }

        public Application GetApplicationById(int appid)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Applications.Single(x => x.AppId == appid);
            }
        }

        public List<Application> GetApplicationByUser(int userid)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Users.Single(x => x.UserId == userid).apps
                                .Where(y=>y.Enabled == true).ToList();
            }
        }

        public List<Application> GetApplicationByUser(string username)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Users.Single(x => x.mailID == username).apps
                                .Where(y => y.Enabled == true).ToList();
            }
        }

        public List<Application> GetApplications(int? status)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                List<Application> applist = new List<Application>();
                foreach(Application app in db.Applications)
                {
                    if (app.Enabled == Convert.ToBoolean(status))
                        applist.Add(app);
                }
                return applist;
            }
        }

        public void DisableApplication(int appid)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                Application app = db.Applications.First(x => x.AppId == appid);
                bool temp = !app.Enabled;
                app.Enabled = temp;
                db.SaveChanges();
            }
        }
    }
}
