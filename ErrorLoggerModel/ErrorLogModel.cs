namespace ErrorLoggerModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ErrorLogModel : DbContext
    {
        public ErrorLogModel()
            : base("name=ErrorLogModel")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ErrorLogModel, ErrorLoggerModel.Migrations.Configuration>("ErrorLogModel"));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //.HasMany(e => e.apps)
            //.WithMany(e => e.users)
            //.Map(m => m.ToTable("ApplicationUsers"));
        }
    }
}
