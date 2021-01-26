using HiSign.Domain.Common;
using System.Collections.Generic;

namespace HiSign.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string BusinessLicense { get; set; }
        public int? CompanyId { get; set; }
        public int BelongToCompanyId { get; set; }
        public string Representaive { get; set; }
        public virtual Company BelongToCompany { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public Status Status { get; set; }
    }
}
