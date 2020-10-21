using HiSign.Application.Interfaces.Repositories;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;

namespace HiSign.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
