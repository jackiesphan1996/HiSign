using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Application.Features.Customer.Commands.CreateCustomer;
using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;

namespace HiSign.Application.Features.Customer.Commands.UpdateCusstomer
{
    public class UpdateCustomerCommand : IRequest<Response<bool>>
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

    public class UpdateCustomerCommandHanlder : IRequestHandler<UpdateCustomerCommand, Response<bool>>
    {
        public readonly ICompanyRepository _companyRepository;
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHanlder(
            ICompanyRepository companyRepository,
            ICustomerRepository customerRepository)
        {
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Response<bool>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer is null)
            {
                throw new ApiException($"CustomerId {request.Id} does not exist");
            }

            customer.Address = request.Address;
            customer.BankAccount = request.BankAccount;
            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;
            customer.Representaive = request.Representaive;
            customer.TaxCode = request.TaxCode;

            await _customerRepository.UpdateAsync(customer);

            return new Response<bool>(true);
        }
    }
}
