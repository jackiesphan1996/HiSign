using HiSign.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HiSign.Application.Interfaces.Repositories
{
    public interface IContractTypeRepository : IGenericRepository<ContractType>
    {
        Task<IEnumerable<ContractType>> GetAllByCompanyIdAsync(int companyId);
    }
}
