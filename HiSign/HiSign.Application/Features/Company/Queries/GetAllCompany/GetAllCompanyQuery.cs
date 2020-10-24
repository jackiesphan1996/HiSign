using HiSign.Application.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Interfaces.Repositories;

namespace Hisign.Application.Features.Company.Queries.GetAllCompany
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string BusinessLicense { get; set; }
    }
    public class GetAllCompanyQuery : IRequest<Response<List<CompanyViewModel>>>
    {
        public string Name { get; set; }
    }

    public class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQuery, Response<List<CompanyViewModel>>>
    {
        private readonly ICompanyRepository _companyRepository;
        public GetAllCompanyQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<Response<List<CompanyViewModel>>> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
        {
            var companies = await _companyRepository.GetAllAsync();

            var res = companies.Select(x => new CompanyViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                TaxCode = x.TaxCode,
                BankAccount = x.BankAccount,
                BusinessLicense = x.BusinessLicense,
                Email = x.Email
            }).ToList();

            return new Response<List<CompanyViewModel>>(res);
        }
    }

}
