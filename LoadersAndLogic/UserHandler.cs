using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace LoadersAndLogic
{
    using ErrorLoggerModel;
    using System.Data.Entity.Infrastructure;
    public class UserHandler
    {
        public bool AddAdmin(User newUser)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                db.Users.Add(newUser);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public bool AddUser(User newUser)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                newUser.access = User.ROLE.USER;
                db.Users.Add(newUser);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool IsAdmin(string mail)
        {
            if (string.IsNullOrEmpty(mail)) return false;
            using (ErrorLogModel db = new ErrorLogModel())
            {
                if (db.Users.Any(x => x.mailID == mail))
                    return db.Users.First(x => x.mailID == mail).access == User.ROLE.ADMIN;
                return false;
            }
        }

        public static string GetName(string id)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                if (db.Users.Any(x => x.mailID == id))
                {
                    User user = db.Users.First(x => x.mailID == id);
                    return user.firstname + " " + user.lastname;
                }
                return " ";
            }
        }

        public static bool IsActive(string mail)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                if (db.Users.Any(x => x.mailID == mail))
                    return db.Users.First(x => x.mailID == mail).Status;
                return false;
            }
        }

        public static void LastLogin(string mail)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                User user = db.Users.First(x => x.mailID == mail);
                user.firstname = user.firstname;
                user.lastname = user.lastname;
                user.mailID = user.mailID;
                user.access = user.access;
                user.LastLogin = DateTime.Now;
                db.Users.Attach(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
        
        public bool AddApplication(int _user,string app)
        {
            bool output = false;
            using (ErrorLogModel db = new ErrorLogModel())
            {
                if (db.Users.Where(x => x.UserId == _user).Count() > 0)
                {
                    if (db.Users.First(x => x.UserId == _user).apps.Where(x => x.AppName == app).Count() == 0)
                    {
                        Application ap = db.Applications.First(x => x.AppName == app);
                        db.Users.First(x => x.UserId == _user).apps.Add(ap);
                        db.SaveChanges();
                        output = true;
                    }
                    else output = false;
                }
                else output = false;
            }
            return output;
        }

        public bool RemoveApplication(int _user, string app)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                if (db.Users.Where(x => x.UserId == _user).Count() > 0)
                {
                    Application ap = db.Applications.First(x => x.AppName == app);
                    db.Users.First(x => x.UserId == _user).apps.Remove(ap);
                    db.SaveChanges();
                }
                else return false;
            }
            return true;
        }
        
        public List<User> GetUserByApplication(int appid)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Applications.First(x => x.AppId == appid).users.Where(x => x.access == User.ROLE.USER).ToList();
            }
        }

        public List<User> GetUsersNotByApplication(int appid)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                try
                {
                    List<User> mappedUsers = db.Applications.First(x => x.AppId == appid).users.ToList();
                    List<User> unmappedUsers = new List<User>();
                    foreach (User user in db.Users.Where(x => x.access == User.ROLE.USER))
                    {
                        if (!mappedUsers.Exists(x => x.UserId == user.UserId))
                            unmappedUsers.Add(user);
                    }
                    return unmappedUsers;
                }
                catch(Exception ex)
                {
                    var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "logErrors.txt";
                    var sw = new System.IO.StreamWriter(filename, true);
                    sw.WriteLine(DateTime.Now.ToString() + " " + ex.Message + " " + ex.InnerException);
                    sw.WriteLine(ex.StackTrace);
                    sw.Close();
                    return null;
                }
            }
        }

        public List<User> GetUsers(int status)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                List<User> userlist = new List<User>();
                foreach (User user in db.Users.Where(x => x.access == User.ROLE.USER))
                {
                    if (user.Status == Convert.ToBoolean(status))
                        userlist.Add(user);
                }
                return userlist;
            }
        }

        public User GetUserByMailId(string mail)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                return db.Users.First(x => x.mailID == mail);
            }
        }

        public void UpdateUser(string mail, string fname,string lname, string phone)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                try {
                    User user = db.Users.First(x => x.mailID == mail);
                    user.firstname = fname;
                    user.lastname = lname;
                    user.phone = phone;
                    user.access = user.access;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Display or log error messages

                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            Console.WriteLine(message);
                        }
                    }
                }
            }
        }

        public void ToggleUser(int userid)
        {
            using (ErrorLogModel db = new ErrorLogModel())
            {
                try
                {
                    User user = db.Users.First(x => x.UserId == userid);
                    user.Status = !user.Status;
                    user.access = user.access;
                    db.Users.Attach(user);
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
            }
        }
    }
}
