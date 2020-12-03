using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;

namespace HiSign.Application.Features.Customer.Queries.GetAllCustomers
{
    public class GetAllCustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string BusinessLicense { get; set; }
        public int? CompanyId { get; set; } }

    public class GetAllCustomerQuery : IRequest<Response<List<GetAllCustomerViewModel>>>
    {
    }

    public class GetAllCustomerQueryHanlder : IRequestHandler<GetAllCustomerQuery, Response<List<GetAllCustomerViewModel>>>
    {
        public readonly ICompanyRepository _companyRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomerQueryHanlder(ICompanyRepository companyRepository, ICustomerRepository customerRepository, IAuthenticatedUserService authenticatedUserService)
        {
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<List<GetAllCustomerViewModel>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            var currentUserLogin = await _authenticatedUserService.GetCurentApplicationUser();

            var customers = _customerRepository.GetAllCustomers(currentUserLogin.CompanyId.Value);

            var result = customers.Select(x => new GetAllCustomerViewModel
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                BankAccount = x.BankAccount,
                Address = x.Address,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                TaxCode = x.TaxCode,
                BusinessLicense = x.BusinessLicense
            }).ToList();

            return new Response<List<GetAllCustomerViewModel>>(result);

        }
    }


}
