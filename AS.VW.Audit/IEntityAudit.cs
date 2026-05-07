using AS.VW.Audit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Audit
{
    public interface IEntityAudit<T>
    {
        List<AuditEntityModel> CompareEntities(T oldEntity, T newEntity, string primaryKey, string[] propertyNames);
    }
}
