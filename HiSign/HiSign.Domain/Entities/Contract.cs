using HiSign.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;

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
        public string Header { get; set; }
        public string AInformation { get; set; }
        public string BInformation { get; set; }
        public string ContractValue { get; set; }
        public string ContractLaw { get; set; }
        public string Footer { get; set; }
        public string Note { get; set; }
        public int? BelongToContractId { get; set; }
        public virtual Contract Parent { get; set; }
        public virtual ICollection<Contract> Children { get; set; }
        public DateTime? ActivedDate { get; set; }
    }

    public enum ContractStatus
    {
        Draft = 0,
        Waiting = 1,
        Active = 2
    }
}
