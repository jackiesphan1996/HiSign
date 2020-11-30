using System.Collections.Generic;
using HiSign.Domain.Entities;

namespace HiSign.Application.Interfaces.Repositories
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        List<Contract> GetAllContracts(int companyId);
    }
}
