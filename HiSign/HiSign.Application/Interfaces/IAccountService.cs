using System.Collections.Generic;
using HiSign.Application.DTOs.Account;
using HiSign.Application.Wrappers;
using System.Threading.Tasks;

namespace HiSign.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> RegisterAsync(int companyId, RegisterEmployeeRequest request);
        Task<Response<List<EmployeeResponse>>> GetAllEmployeeAsync(int companyId);
        Task<Response<bool>> UpdateEmployeeAsync(UpdateEmployeeRequest request);
    }
}
