using System.Threading.Tasks;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HiSign.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly DbSet<Company> DbSet;
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            DbSet = dbContext.Set<Company>();
        }
    }
}
