using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.Models
{
    public class DeviceContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<DeviceContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);

            /*
            modelBuilder.Entity<DBDevice>()
                .HasKey(p => p.deviceID);
            modelBuilder.Entity<DBDevice>().Property(p => p.deviceID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            */
            base.OnModelCreating(modelBuilder);
        }

        
        public DbSet<NexaDevice> NexaDeviceObject { get; set; }
        public DbSet<NexaTimeSchema> NexaTimeSchema { get; set; }
    }
}
