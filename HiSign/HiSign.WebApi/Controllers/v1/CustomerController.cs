using System.Threading.Tasks;
using Hisign.Application.Features.Company.Commands.CreateCompany;
using Hisign.Application.Features.Company.Queries.GetAllCompany;
using HiSign.Application.Features.Customer.Commands.CreateCustomer;
using HiSign.Application.Features.Customer.Queries.GetAllCustomers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
