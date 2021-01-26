using HiSign.Application.Features.ContractType.Commands.CreateContractType;
using HiSign.Application.Features.ContractType.Commands.UpdateContractType;
using HiSign.Application.Features.ContractType.Queries.GetAllContractTypes;
using HiSign.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Application.Features.Company.Commands.UpdateCompanyByCompany;
using Microsoft.EntityFrameworkCore;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context; 

        public ContractTypeController(IMediator mediator, ApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] GetAllContractTypesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        public async Task<IActionResult> Post(CreateContractTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        [Route("{id}")]
        public async Task<IActionResult> Pust([FromRoute] int id, UpdateContractTypeCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "CompanyAdmin,CEO")]
        [Route("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] UpdateyStatus status)
        {
            var company = await _context.ContractTypes.SingleOrDefaultAsync(x => x.Id == id);

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
