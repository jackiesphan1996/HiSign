using System.Threading;
using System.Threading.Tasks;
using Hisign.Application.Features.Company.Queries.GetAllCompany;
using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;

namespace HiSign.Application.Features.Company.Queries.GetCompanyInfo
{
    public class GetCompanyinfoQuery : IRequest<Response<CompanyViewModel>>
    {
    }

    public class GetCompanyinfoQueryHanlder : IRequestHandler<GetCompanyinfoQuery, Response<CompanyViewModel>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetCompanyinfoQueryHanlder(IAuthenticatedUserService authenticatedUserService,
            ICompanyRepository companyRepository)
        {
            _authenticatedUserService = authenticatedUserService;
            _companyRepository = companyRepository;
        }

        public async Task<Response<CompanyViewModel>> Handle(GetCompanyinfoQuery request,
            CancellationToken cancellationToken)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var company = await _companyRepository.GetByIdAsync(currentUser.CompanyId.Value);

            var result = new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                Address = company.Address,
                PhoneNumber = company.PhoneNumber,
                TaxCode = company.TaxCode,
                BankAccount = company.BankAccount,
                BusinessLicense = company.BusinessLicense,
                Email = company.Email
            };

            return new Response<CompanyViewModel>(result);
        }
    }
}
