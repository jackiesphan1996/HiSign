using System;
using System.Collections;
using System.Collections.Generic;
using HiSign.Application.Features.Contract.Commands.CreateNewContract;
using HiSign.Application.Features.Contract.Queries.GetAllContracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.VariantTypes;
using GemBox.Document;
using HiSign.Application.Exceptions;
using HiSign.Application.Wrappers;
using HiSign.Domain.Entities;
using HiSign.Infrastructure.Persistence.Contexts;
using HiSign.WebApi.Services;
using MariGold.OpenXHTML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ContractController(
            IMediator mediator , 
            ApplicationDbContext dbContext,
            AzureBlobHelper azureBlobHelper,
            AzureBlobSavingService azureBlobSavingService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _azureBlobHelper = azureBlobHelper;
            _azureBlobSavingService = azureBlobSavingService;
            _configuration = configuration;
            _userManager = userManager;
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

            contract.FileUrl = url;

            _dbContext.SaveChanges();

            return Ok(url);
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("{id}/sign")]
        public async Task<IActionResult> Sign([FromRoute] int id, [FromBody] SignData model)
        {
            var contract = _dbContext.Contracts.SingleOrDefault(x => x.Id == id);

            X509Certificate2 clientCertificate = new X509Certificate2(model.RawData);

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");


            var document = DocumentModel.Load(contract.FileUrl);

            // Create visual representation of digital signature
            var options = new DocxSaveOptions()
            {
                DigitalSignatures =
                {
                    new DocxDigitalSignatureSaveOptions
                    {
                        Certificate = clientCertificate
                    }
                }
            };


            var fileName = $"{contract.Title}_{DateTime.Now.ToString("dd-MM-yyyy HH:mm")}.docx";

            var container = "";

            container = "contracts";
            var blobContainer = await _azureBlobHelper.GetBlobContainer(container);

            using (var stream = new MemoryStream())
            {
                document.Save(stream, options);

                var uploadFileName = await _azureBlobSavingService.SavingFileToAzureBlobAsync(stream.ToArray(), fileName, options.ContentType, blobContainer);
                var url = $"{_configuration["AzureBlob:ServerImage"]}/{container}/{uploadFileName}";

                contract.FileUrl = url;

                _dbContext.SaveChanges();
            }

            return Ok();
        }

        [HttpGet]
        [Route("a-side-info")]
        public async Task<IActionResult> GetCreationContractInfo(int id)
        {
            var contract = await _dbContext.Contracts.SingleOrDefaultAsync(x => x.Id == id);

            if (contract is null)
            {
                throw new ApiException("Contract does not exist");
            }

            return Ok(new
            {
                companyId = contract.CompanyId
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-by-taxcode")]
        public async Task<IActionResult> GetAllContractsByTaxCode(string taxCode)
        {
            var contracts = await _dbContext.Contracts.Include(x => x.Customer).Where(x => x.Customer.TaxCode == taxCode).ToListAsync();

            var result = contracts.Select(x => new GetAllContractsViewModel
            {
                Id = x.Id,
                ContractName = x.Name,
                ContractNum = x.ContractNum,
                ContractPlace = x.ContractPlace,
                ContractContent = x.Content,
                ContractTypeId = x.ContractTypeId,
                ContractExpiredDate = x.ExpiredDate.Value,
                ContractValue = x.TotalValue,
                ContractTitle = x.Title,
                Customer = new CustomerViewModel
                {
                    Id = x.CustomerId,
                    CompanyName = x.Customer.Name
                },
                Status = x.Status,
                FileUrl = x.FileUrl
            }).ToList();

            return Ok(new Response<List<GetAllContractsViewModel>>(result));
        }
    }

    public class SignData
    {
        public string RawData { get; set; }
    }
}
