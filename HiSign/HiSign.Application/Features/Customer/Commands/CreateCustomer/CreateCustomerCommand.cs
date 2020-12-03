using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;

namespace HiSign.Application.Features.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string BusinessLicense { get; set; }
        public int? CompanyId { get; set; }
        public string Representaive { get; set; }
    }

    public class CreateCustomerCommandHanlder : IRequestHandler<CreateCustomerCommand, Response<int>>
    {
        public readonly ICompanyRepository _companyRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHanlder(ICompanyRepository companyRepository, ICustomerRepository customerRepository, IAuthenticatedUserService authenticatedUserService)
        {
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var currentUserLogin = await _authenticatedUserService.GetCurentApplicationUser();

            Domain.Entities.Customer customer = null;
            
            if (request.CompanyId.HasValue)
            {
                if (request.CompanyId.Value == currentUserLogin.CompanyId)
                {
                    throw new ApiException("Can not add your company to customer list");
                }

                var company = await _companyRepository.GetByIdAsync(request.CompanyId.Value);

                if (company is null)
                {
                    throw new ApiException("Company ID does not exist.");
                }

                bool isExist = await _customerRepository.Exist(currentUserLogin.CompanyId.Value, company.Id);

                if (isExist)
                {
                    throw new ApiException($"{company.Name} already added to customer list.");
                }

                customer = new Domain.Entities.Customer
                {
                    CompanyId = company.Id,
                    BelongToCompanyId = currentUserLogin.CompanyId.GetValueOrDefault(),
                    Address = company.Address,
                    BankAccount = company.BankAccount,
                    BusinessLicense = company.BusinessLicense,
                    Email = company.Email,
                    Name = company.Name,
                    PhoneNumber = company.PhoneNumber,
                    TaxCode = company.TaxCode,
                    Representaive = company.Representaive
                };
            }
            else
            {
                customer = new Domain.Entities.Customer
                {
                    BelongToCompanyId = currentUserLogin.CompanyId.GetValueOrDefault(),
                    Address = request.Address,
                    BankAccount = request.BankAccount,
                    BusinessLicense = request.BusinessLicense,
                    Email = request.Email,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    TaxCode = request.TaxCode,
                    Representaive = request.Representaive
                };
            }

            await _customerRepository.AddAsync(customer);

           return new Response<int>(customer.Id);
        }
    }
}
