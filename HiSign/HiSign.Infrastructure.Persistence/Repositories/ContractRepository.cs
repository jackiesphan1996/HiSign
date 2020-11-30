using System.Collections.Generic;
using System.Linq;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HiSign.Infrastructure.Persistence.Repositories
{
    public class ContractRepository : GenericRepository<Contract>,IContractRepository
    {
        private readonly DbSet<Contract> _dbSet;
        public ContractRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Contract>();
        }

        public List<Contract> GetAllContracts(int companyId)
        {
            return _dbSet.Where(x => x.CompanyId == companyId).Include(x => x.Customer).ToList();
        }
    }
}
