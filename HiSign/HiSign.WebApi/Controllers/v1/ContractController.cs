using System;
using HiSign.Application.Features.Contract.Commands.CreateNewContract;
using HiSign.Application.Features.Contract.Queries.GetAllContracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Infrastructure.Persistence.Contexts;
using HiSign.WebApi.Services;
using MariGold.OpenXHTML;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _dbContext;
        private readonly AzureBlobHelper _azureBlobHelper;
        private readonly AzureBlobSavingService _azureBlobSavingService;
        private readonly IConfiguration _configuration;

        public ContractController(
            IMediator mediator , 
            ApplicationDbContext dbContext,
            AzureBlobHelper azureBlobHelper,
            AzureBlobSavingService azureBlobSavingService,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _azureBlobHelper = azureBlobHelper;
            _azureBlobSavingService = azureBlobSavingService;
            _configuration = configuration;
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

        [HttpGet]
        [Authorize(Roles = "CompanyAdmin,CEO,Secretary")]
        [Route("export-docx/{id}")]
        public IActionResult GenerateContractFile([FromRoute] int id)
        {
            var contract = _dbContext.Contracts.SingleOrDefault(x => x.Id == id);

            if (contract == null)
            {
                throw new ApiException($"Contract with ID {id} does not exist.");
            }

            using (MemoryStream mem = new MemoryStream())
            {
                WordDocument doc = new WordDocument(mem);
                doc.Process(new HtmlParser(contract.Content));
                doc.Save();

                return File(mem.ToArray(), "application/msword", $"{contract.Name}_{contract.Name}.docx");
            }
        }

        [HttpPost]
        [Route("{id}/upload-contract")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadContract([FromRoute] int id, IFormFile file)
        {
            var contract = _dbContext.Contracts.SingleOrDefault(x => x.Id == id);
            if (contract == null)
            {
                throw new ApiException("Contract Id does not exist.");
            }

            byte[] data = null;
            var fileName = DateTime.Now.ToString("yy-MM-dd HH:mm:ss")  + "_" +  file.FileName;
            using (var reader = new BinaryReader(file.OpenReadStream()))
            {
                data = reader.ReadBytes((int)file.Length);
            }
            var contenType = file.ContentType;
            var uploadFileName = "";
            CloudBlobContainer blobContainer = null;
            var container = "";

            container = "contracts";
            blobContainer = await _azureBlobHelper.GetBlobContainer(container);


            uploadFileName = await _azureBlobSavingService.SavingFileToAzureBlobAsync(data, fileName, contenType, blobContainer);
            var url = $"{_configuration["AzureBlob:ServerImage"]}/{container}/{uploadFileName}";

            contract.FileUrl = uploadFileName;

            _dbContext.SaveChanges();

            return Ok(url);
        }
    }
}
