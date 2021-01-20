using HiSign.Domain.Common;
using System;

namespace HiSign.Domain.Entities
{
    public class DigitalSignature : BaseEntity
    {
        public string SerialNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
