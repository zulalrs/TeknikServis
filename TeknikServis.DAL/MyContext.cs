using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.DAL
{
    public class MyContext : IdentityDbContext<User>
    {
        public MyContext() : base("name=MyCon")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
