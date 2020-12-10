using HiSign.Domain.Common;
using System.Collections.Generic;

namespace HiSign.Domain.Entities
{
    public class Permission : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }

    public class UserPermission : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
        public bool Enabled { get; set; }
    }
}
