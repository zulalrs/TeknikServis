
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using TeknikServis.DAL;
using TeknikServis.Models.Entities;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.BLL.Identity
{
    public static class MembershipTools
    {
        private static MyContext _db;

        public static UserStore<User> NewUserStore() => new UserStore<User>(_db ?? new MyContext());
        public static UserManager<User> NewUserManager() => new UserManager<User>(NewUserStore());

        public static RoleStore<Role> NewRoleStore() => new RoleStore<Role>(_db ?? new MyContext());
        public static RoleManager<Role> NewRoleManager() => new RoleManager<Role>(NewRoleStore());




        public static string GetNameSurname(string userId)
        {
            User user;
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "";

                user = NewUserManager().FindById(id);
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                    return null;
            }

            return $"{user.Name} {user.Surname}";
        }
        public static string GetRole(string userId)
        {
            User user;
            string role="";
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "";

                user = NewUserManager().FindById(id);

            }
            else
            {
                user = NewUserManager().FindById(userId);
                foreach (var item in user.Roles)
                {
                    role = NewRoleManager().FindById(item.RoleId).Name;
                }
            }

            return $"{role}";
        }
        public static string GetAvatarPath(string userId)
        {
            User user;
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "../../dist/img/ZGlogo.jpg";

                user = NewUserManager().FindById(id);
                if(user.AvatarPath==null)
                    return "../../dist/img/ZGlogo.jpg";
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                    return "../../dist/img/ZGlogo.jpg";
                if (user.AvatarPath == null)
                    return "../../dist/img/ZGlogo.jpg";
            }

            return $"{user.AvatarPath}";
        }

        public static string GetUserName(string userId)
        {
            User user;
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "";

                user = NewUserManager().FindById(id);
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                    return null;
            }

            return $"{user.UserName}";
        }
    }
}
