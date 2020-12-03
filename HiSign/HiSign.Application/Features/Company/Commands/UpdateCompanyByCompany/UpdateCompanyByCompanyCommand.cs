using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;

namespace HiSign.Application.Features.Company.Commands.UpdateCompanyByCompany
{
    public class UpdateCompanyByCompanyCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string BusinessLicense { get; set; }
        public string Representaive { get; set; }
    }

    public class UpdateCompanyByCompanyCommandHandler : IRequestHandler<UpdateCompanyByCompanyCommand, Response<bool>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UpdateCompanyByCompanyCommandHandler(ICompanyRepository companyRepository, IAuthenticatedUserService authenticatedUserService)
        {
            _companyRepository = companyRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<bool>> Handle(UpdateCompanyByCompanyCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();
            var company = await _companyRepository.GetByIdAsync(request.Id);

            if (company == null)
            {
                throw new ApiException("Company does not exist. ");
            }

            if (currentUser.CompanyId != company.Id)
            {
                throw new ApiException("Can not update other company.");
            }

            company.Name = request.Name;
            company.Address = request.Address;
            company.PhoneNumber = request.PhoneNumber;
            company.Email = request.Email;
            company.TaxCode = request.TaxCode;
            company.BankAccount = request.BankAccount;
            company.BusinessLicense = request.BusinessLicense;
            company.Representaive = request.Representaive;

            await _companyRepository.UpdateAsync(company);

            return new Response<bool>(true);
        }
    }
}
