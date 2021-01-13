using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using HiSign.Domain.Entities;
using MediatR;

namespace HiSign.Application.Features.Contract.Queries.GetAllContracts
{
    public class GetAllContractsViewModel
    {
        public int Id { get; set; }
        public string ContractTitle { get; set; }
        public string ContractNum { get; set; }
        public string ContractName { get; set; }
        public string ContractPlace { get; set; }
        public DateTime ContractExpiredDate { get; set; }
        public decimal ContractValue { get; set; }
        public string ContractContent { get; set; }
        public int ContractTypeId { get; set; }
        public int CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }
        public ContractStatus Status { get; set; }
        public string StatusAsString => Status.ToString();
        public string FileUrl { get; set; }
        public string Header { get; set; }
        public string AInformation { get; set; }
        public string BInformation { get; set; }
        public string ContractStringValue { get; set; }
        public string ContractLaw { get; set; }
        public string Footer { get; set; }
        public bool IsMainContract { get; set; }
        public int? BelongToContractId { get; set; }
    }

    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
    }

    public class GetAllContracsQuery : IRequest<Response<List<GetAllContractsViewModel>>>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class GetAllContracsQueryHandler : IRequestHandler<GetAllContracsQuery, Response<List<GetAllContractsViewModel>>>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        private readonly IContractRepository _contractRepository;

        public GetAllContracsQueryHandler(IAuthenticatedUserService authenticatedUserService,
            IContractRepository contractRepository
            )
        {
            _authenticatedUserService = authenticatedUserService;
            _contractRepository = contractRepository;
        }

        public async Task<Response<List<GetAllContractsViewModel>>> Handle(GetAllContracsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var contracts = _contractRepository.GetAllContracts(currentUser.CompanyId.Value);

            if (request.FromDate.HasValue)
            {
                contracts = contracts.Where(x => x.Created >= request.FromDate.Value).ToList();
            }

            if (request.ToDate.HasValue)
            {
                contracts = contracts.Where(x => x.Created <= request.ToDate.Value.Date).ToList();
            }

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
                FileUrl = x.FileUrl,
                Header = x.Header,
                AInformation = x.AInformation,
                BInformation = x.BInformation,
                ContractLaw = x.ContractLaw,
                Footer = x.Footer,
                ContractStringValue = x.ContractValue,
                CustomerId = x.CustomerId,
                IsMainContract = !x.BelongToContractId.HasValue,
                BelongToContractId = x.BelongToContractId
            }).ToList();

            return new Response<List<GetAllContractsViewModel>>(result);
        }
    }
}
