using System.Collections.Generic;
using HiSign.Domain.Entities;

namespace HiSign.Application.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        List<Customer> GetAllCustomers(int companyId);
    }
}
