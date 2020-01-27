using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contacts.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Contacts.Infrastructure.DAL
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        /// <summary>
        /// DbContext pooling doesn't allow injection of additional dependencies.
        /// </summary>
        private ILogger<DatabaseContext> Logger => this.GetService<ILogger<DatabaseContext>>();

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task<int> SaveChangesAsync() => await SaveChangesAsync(true);

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {   
            foreach (var entry in ChangeTracker.Entries())
            {
                var created = entry.State == EntityState.Added;
                var deleted = entry.State == EntityState.Deleted;
                var modified = entry.State == EntityState.Modified;

                if (entry.Entity is BaseEntity baseEntity)
                {
                    if (created || deleted || modified)
                    {
                        LogTransaction(entry, deleted, baseEntity.Id);
                    }
                }

            }
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        private void LogTransaction(EntityEntry entry, bool deleted, int id)
        {
            var table = entry.Metadata.GetTableName();
            var state = deleted ? nameof(EntityState.Deleted) : entry.State.ToString();
            var propertyTransactionLog = deleted ? string.Empty : GetPropertyTransactionLog(entry);

            Logger?.LogInformation("Transaction: {table}:{id} {state} {changedFields}", table, id, state, propertyTransactionLog);
        }

        private static string GetPropertyTransactionLog(EntityEntry entry)
        {
            StringBuilder changedFields = new StringBuilder();

            foreach (var propertyEntry in entry.Properties)
            {
                switch (entry.State)
                {
                    case EntityState.Modified when propertyEntry.IsModified:
                        changedFields.Append($"{propertyEntry.Metadata.Name} ({propertyEntry.OriginalValue}->{propertyEntry.CurrentValue}) ");
                        break;
                    case EntityState.Added when propertyEntry.CurrentValue != null:
                        changedFields.Append($"{propertyEntry.Metadata.Name} ({propertyEntry.CurrentValue}) ");
                        break;
                }
            }

            return changedFields.ToString();
        }
    }
}
