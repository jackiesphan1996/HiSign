using System.Collections.Generic;
using System.Threading.Tasks;
using HiSign.Domain.Entities;

namespace HiSign.Application.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        List<Customer> GetAllCustomers(int companyId);
        Task<bool> Exist(int belongToCompanyId, int companyId);
    }
}
