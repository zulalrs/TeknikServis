using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using TeknikServis.Models.Entities;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.DAL
{
    public class MyContext : IdentityDbContext<User>
    {
        public MyContext() : base("name=MyCon")
        {
            this.InstanceDate = DateTime.Now;
        }
        public DateTime InstanceDate { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Ariza> Arizalar { get; set; }
        public virtual DbSet<Fotograf> Fotograflar { get; set; }
        public virtual DbSet<Anket> Anketler { get; set; }
        public virtual DbSet<ArizaLog> ArizaLoglar { get; set; }
    }
}
