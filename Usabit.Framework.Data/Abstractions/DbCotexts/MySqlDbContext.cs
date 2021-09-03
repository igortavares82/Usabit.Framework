using Microsoft.EntityFrameworkCore;
using System;

namespace Usabit.Framework.Data.Abstractions.DbCotexts
{
    public abstract class MySqlDbContext : DbContext, IDisposable
    {
        private bool disposed = false;

        public MySqlDbContext(DbContextOptions options) : base(options) { }

        public void AutoDetectChanges(bool value) => base.ChangeTracker.AutoDetectChangesEnabled = value;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
                return;

            optionsBuilder.UseLazyLoadingProxies(false);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override void Dispose()
        {
            base.Dispose();
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            disposed = true;
            GC.Collect();
        }
    }
}
