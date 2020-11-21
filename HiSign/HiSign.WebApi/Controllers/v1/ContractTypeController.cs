using HiSign.Application.Features.ContractType.Commands.CreateContractType;
using HiSign.Application.Features.ContractType.Queries.GetAllContractTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractTypeController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
