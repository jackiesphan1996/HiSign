using HiSign.Domain.Common;
using System;

namespace HiSign.Domain.Entities
{
    public class Contract : AuditableBaseEntity
    {
        public int ContractTypeId { get; set; }
        public ContractType ContractType { get; set; }
        public int CompanyId { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public ContractStatus Status { get; set; }
        public Customer Customer { get; set; }
        public Company Company { get; set; }
        public string Content { get; set; }
        public string ContractPlace { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string ContractNum { get; set; }
        public string FileUrl { get; set; }
    }

    public enum ContractStatus
    {
        Draft = 0,
        Open = 1,
        Completed = 2
    }
}
