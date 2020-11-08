using System.Threading.Tasks;
using HiSign.Domain.Entities;

namespace HiSign.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        Task<ApplicationUser> GetCurentApplicationUser();
    }
}
