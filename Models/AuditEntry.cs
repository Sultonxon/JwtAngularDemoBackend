using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProductAPi.Models;


public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        EntityEntry = entry;
    }

    public EntityEntry EntityEntry { get; }
    public string UserId { get; set; }
    public string TableName { get; set; }
    public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
    public AuditType AuditType { get; set; }
    public List<string> ChangedColumns { get; } = new List<string>();
    public Audit ToAudit()
    {
        var audit = new Audit();
        audit.UserId = UserId;
        audit.TableName = TableName;
        audit.Type = AuditType.ToString();
        audit.DateTime = DateTime.Now;
        audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
        audit.OldValues = OldValues.Count == 0 ? "" : JsonConvert.SerializeObject(OldValues);
        audit.NewValues = NewValues.Count == 0 ? "" : JsonConvert.SerializeObject(NewValues);
        audit.AffectedColumns = ChangedColumns.Count == 0 ? "" : JsonConvert.SerializeObject(ChangedColumns);
        return audit;
    }
}
