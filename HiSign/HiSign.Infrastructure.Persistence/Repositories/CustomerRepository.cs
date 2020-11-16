using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HiSign.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> Customers;
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            Customers = dbContext.Set<Customer>();
        }

        public List<Customer> GetAllCustomers(int companyId)
        {
            return Customers.Where(x => x.BelongToCompanyId == companyId).ToList();
        }

        public async Task<bool> Exist(int belongToCompanyId, int companyId)
        {
            return await Customers.AnyAsync(x => x.CompanyId == companyId && x.BelongToCompanyId == belongToCompanyId);
        }
    }
}
