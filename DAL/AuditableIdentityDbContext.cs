using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductApi.Models.Enums;
using ProductAPi.Models;

namespace ProductApi.DAL;

public abstract class AuditableIdentityDbContext: IdentityDbContext<AppUser>
{
    public AuditableIdentityDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }

    public DbSet<Audit> AuditLogs { get; set; }

    public virtual async Task<int> SaveChangesAsync(string userId)
    {
        OnBeforeChanges(userId);
        var result = await SaveChangesAsync();
        return result;
    }

    private void OnBeforeChanges(string userId)
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Unchanged
                || entry.State == EntityState.Detached)
                    continue;
            var auditEntry = new AuditEntry(entry);
            auditEntry.TableName = entry.Entity.GetType().Name;
            auditEntry.UserId = userId;
            foreach (PropertyEntry property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[property.Metadata.Name] = property.CurrentValue;
                    continue;
                }
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[property.Metadata.Name] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(property.Metadata.Name);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[property.Metadata.Name] = property.OriginalValue;
                            auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                        }
                        break;
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                        break;
                }

            }
            auditEntries.Add(auditEntry);
        }
        foreach (var entry in auditEntries)
        {
            AuditLogs.Add(entry.ToAudit());
        }
    }
}
