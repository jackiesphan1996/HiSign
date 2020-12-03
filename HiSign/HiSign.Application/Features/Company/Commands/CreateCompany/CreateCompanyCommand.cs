using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hisign.Application.Features.Company.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<Response<bool>>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string BusinessLicense { get; set; }
        public string Representaive { get; set; }
    }

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Response<bool>>
    {
        private readonly ICompanyRepository _companyRepository;
        public CreateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<Response<bool>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new HiSign.Domain.Entities.Company
            {
                Name = request.Name,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                BankAccount = request.BankAccount,
                BusinessLicense = request.BusinessLicense,
                Email = request.Email,
                TaxCode = request.TaxCode,
                Created = DateTime.Now,
                Representaive = request.Representaive
            };

            await _companyRepository.AddAsync(company);

            return new Response<bool>(true);
        }
    }

}
