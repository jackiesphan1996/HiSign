using System.Linq;
using HiSign.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Application.Interfaces;
using HiSign.Domain.Entities;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigitalSignatureController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public DigitalSignatureController(ApplicationDbContext dbContext, IAuthenticatedUserService authenticatedUserService)
        {
            _dbContext = dbContext;
            _authenticatedUserService = authenticatedUserService;
        }

        [HttpGet]
        [Authorize(Roles = "CompanyAdmin,CEO,Secretary")]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();
            var allData = _dbContext.Set<DigitalSignature>().Where(x => x.CompanyId == currentUser.CompanyId).ToList();
            return Ok(allData);
        }

        [HttpPost]
        [Authorize(Roles = "CompanyAdmin,CEO,Secretary")]
        public async Task<IActionResult> Create([FromBody] DigitalSignature digitalSignature)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var signature = _dbContext.Set<DigitalSignature>().FirstOrDefault(x => x.SerialNumber == digitalSignature.SerialNumber);

            if (signature != null)
            {
                throw new ApiException("Serial Number already exists.");
            }

            _dbContext.Set<DigitalSignature>().Add(digitalSignature);

            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "CompanyAdmin,CEO,Secretary")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DigitalSignature digitalSignature)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var signature = _dbContext.Set<DigitalSignature>().FirstOrDefault(x => x.Id == id);

            if (signature == null)
            {
                throw new ApiException("Id does not  exist.");
            }

            signature.SerialNumber = digitalSignature.SerialNumber;
            signature.ExpirationDate = digitalSignature.ExpirationDate;

            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "CompanyAdmin,CEO,Secretary")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var signature = _dbContext.Set<DigitalSignature>().FirstOrDefault(x => x.Id == id);

            if (signature == null)
            {
                throw new ApiException("Id does not  exist.");
            }

            _dbContext.Set<DigitalSignature>().Remove(signature);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
