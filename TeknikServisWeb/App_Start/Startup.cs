using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Admin.Web.UI.App_Start.Startup))]

namespace Admin.Web.UI.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            // Sistemi çalıştırdığımızda owinleri kurduğumuz için bir owinstartup dosyası isteyecek bunun için bir Web Projesi içerisinde yer alan app_start klasörüne new item ile bir owinstartup class ı ekledik ve içerisine gerekli kodları yazdık.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account")  // Login işlemleri için bu sayfayı kullanacagımızı belirttik. Yetkisiz olunan bir yere girmeye çalıştıgımızda ya da authorize ile kullanıcı uyuşmadıgı zaman Account Index e yonlendiriliyoruz.
            });
        }
    }
}