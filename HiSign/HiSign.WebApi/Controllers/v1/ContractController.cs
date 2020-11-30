using HiSign.Application.Features.ContractType.Commands.CreateContractType;
using HiSign.Application.Features.ContractType.Queries.GetAllContractTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HiSign.Application.Features.Contract.Commands.CreateNewContract;
using HiSign.Application.Features.Contract.Queries.GetAllContracts;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "CompanyAdmin,CEO,Secretary")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllContracsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [Authorize(Roles = "CompanyAdmin,CEO,Secretary")]
        public async Task<IActionResult> Post(CreateNewContractCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
