using HiSign.Domain.Common;
using System.Collections.Generic;

namespace HiSign.Domain.Entities
{
    public class ContractType : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string Content { get; set; }
        public Company Company { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public Status Status { get; set; }
    }
}
