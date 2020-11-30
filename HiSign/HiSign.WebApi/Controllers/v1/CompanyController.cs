using System.Threading.Tasks;
using Hisign.Application.Features.Company.Commands.CreateCompany;
using HiSign.Application.Features.Company.Commands.UpdateCompanyByCompany;
using Hisign.Application.Features.Company.Queries.GetAllCompany;
using HiSign.Application.Features.Company.Queries.GetCompanyInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCompanyQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Post(CreateCompanyCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize("CEO,CompanyAdmin")]
        [Route("{id}")]
        public async Task<IActionResult> PutCompany([FromRoute]int id, UpdateCompanyByCompanyCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize("CEO,CompanyAdmin,Secretary")]
        [Route("{id}")]
        public async Task<IActionResult> GetInfo([FromQuery] GetCompanyinfoQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
