using HiSign.Application.Interfaces.Repositories;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSign.Infrastructure.Persistence.Repositories
{
    public class ContractTypeRepository : GenericRepository<ContractType>, IContractTypeRepository
    {
        private readonly DbSet<ContractType> _contractTypes;
        public ContractTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _contractTypes = dbContext.Set<ContractType>();
        }

        public async Task<IEnumerable<ContractType>> GetAllByCompanyIdAsync(int companyId)
        {
            return await _contractTypes.Where(x => x.CompanyId == companyId).ToListAsync();
        }
    }
}
