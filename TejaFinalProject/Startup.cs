using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using TejaFinalProject.Models;

[assembly: OwinStartupAttribute(typeof(TejaFinalProject.Startup))]
namespace TejaFinalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("ADMIN"))
            {
                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "ADMIN";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "tthunugu@syr.edu";//"Thunuguntla_Teja";
                user.Email = "tthunugu@syr.edu";

                string userPWD = "Tjpk?1991";

                var chkUser = UserManager.Create(user, userPWD);
                LoadersAndLogic.UserHandler us = new LoadersAndLogic.UserHandler();
                ErrorLoggerModel.User tuser = new ErrorLoggerModel.User()
                {
                    mailID = "tthunugu@syr.edu",
                    access = ErrorLoggerModel.User.ROLE.ADMIN,
                    firstname = "Teja",
                    lastname = "Thunuguntla",
                    Status = true,
                    LastLogin = DateTime.Now
                };
                us.AddAdmin(tuser);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "ADMIN");

                }
            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("USER"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "USER";
                roleManager.Create(role);

            }
        }
    }
}
