using System.Linq;
using HiSign.Application.Features.Customer.Commands.CreateCustomer;
using HiSign.Application.Features.Customer.Commands.UpdateCusstomer;
using HiSign.Application.Features.Customer.Queries.GetAllCustomers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Application.Features.Company.Commands.UpdateCompanyByCompany;
using HiSign.Application.Wrappers;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IMediator _mediator;
        private ApplicationDbContext _context;

        public CustomerController(IMediator mediator, ApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCustomerQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        public async Task<IActionResult> Post(CreateCustomerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, UpdateCustomerCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var customer = _context.Set<Customer>().Find(id);

            if (customer == null)
            {
                throw new ApiException("Id does not exist.");
            }

            var result = new GetAllCustomerViewModel
            {
                Id = customer.Id,
                CompanyId = customer.CompanyId,
                BankAccount = customer.BankAccount,
                Address = customer.Address,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                TaxCode = customer.TaxCode,
                BusinessLicense = customer.BusinessLicense
            };

            return Ok(new Response<GetAllCustomerViewModel>(result));
        }

        [HttpPut]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        [Route("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] UpdateyStatus status)
        {
            var company = await _context.Set<Customer>().SingleOrDefaultAsync(x => x.Id == id);

            if (company == null)
            {
                throw new ApiException("Id does not exist.");
            }

            company.Status = status.Status;

            _context.SaveChanges();

            return Ok();
        }
    }
}
