using HiSign.Application.Features.Contract.Commands.CreateNewContract;
using HiSign.Application.Features.Contract.Queries.GetAllContracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;
using HiSign.Infrastructure.Persistence.Contexts;

namespace HiSign.WebApi.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ContractController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _dbContext;

        public ContractController(IMediator mediator , ApplicationDbContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
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

            using (WordDocument document = new WordDocument())
            {
                //Adds a section and a paragraph to the document
                document.EnsureMinimal();
                //Appends text to the last paragraph of the document
                document.LastParagraph.AppendText(contract.Content);
                MemoryStream stream = new MemoryStream();
                //Saves the Word document to  MemoryStream
                document.Save(stream, FormatType.Docx);
                stream.Position = 0;
                //Download Word document in the browser
                return File(stream, "application/msword", $"{contract.Name}_{contract.Name}.docx");
            }
        }
    }
}
