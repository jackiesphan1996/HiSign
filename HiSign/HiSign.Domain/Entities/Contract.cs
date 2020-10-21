using System;
using System.ComponentModel;
using HiSign.Domain.Common;

namespace HiSign.Domain.Entities
{
    public class Contract : AuditableBaseEntity
    {
        public int ContractTypeId { get; set; }
        public ContractType ContractType { get; set; }
        public int CompanyId { get; set; }
        public int CustomerCompanyId { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public ContractStatus Status { get; set; }
        public Company Company { get; set; }
    }

    public enum ContractStatus
    {
        [Description("Status là AAAA")]
        AAAA = 0,
        [Description("Status là BBBB")]
        BBBB = 1,
        [Description("Status là CCCC")]
        CCCC = 2,
        [Description("Status là DDDD")]
        DDDD = 3
    }
}
