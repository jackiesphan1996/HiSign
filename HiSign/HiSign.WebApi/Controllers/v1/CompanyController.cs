using System.Linq;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using Hisign.Application.Features.Company.Commands.CreateCompany;
using HiSign.Application.Features.Company.Commands.UpdateCompanyByCompany;
using Hisign.Application.Features.Company.Queries.GetAllCompany;
using HiSign.Application.Features.Company.Queries.GetCompanyInfo;
using HiSign.Application.Wrappers;
using HiSign.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private IMediator _mediator;
        private readonly ApplicationDbContext _dbContext;
        public CompanyController(IMediator mediator, ApplicationDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
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
        [Authorize(Roles = "CEO,CompanyAdmin")]
        [Route("{id}")]
        public async Task<IActionResult> PutCompany([FromRoute]int id, UpdateCompanyByCompanyCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "CEO,CompanyAdmin,Secretary")]
        [Route("info")]
        public async Task<IActionResult> GetInfo([FromQuery] GetCompanyinfoQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("info/guest")]
        public async Task<IActionResult> GetInfo(int id)
        {
            var company = await _dbContext.Companies.SingleOrDefaultAsync(x => x.Id == id);

            if (company == null)
            {
                throw new ApiException("Id does not exist.");
            }

            var result = new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                Address = company.Address,
                PhoneNumber = company.PhoneNumber,
                TaxCode = company.TaxCode,
                BankAccount = company.BankAccount,
                BusinessLicense = company.BusinessLicense,
                Email = company.Email
            };

            return Ok(new Response<CompanyViewModel>(result));
        }
    }
}
