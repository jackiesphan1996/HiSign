using System;
using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using HiSign.Domain.Entities;
using MediatR;

namespace HiSign.Application.Features.Contract.Commands.CreateNewContract
{
    public class CreateNewContractCommand : IRequest<Response<int>>
    {
        public string ContractTitle { get; set; }
        public string ContractNum { get; set; }
        public string ContractName { get; set; }
        public string ContractPlace { get; set; }
        public DateTime ContractExpiredDate { get; set; }
        public decimal ContractValue { get; set; }
        public string ContractContent { get; set; }
        public int ContractTypeId { get; set; }
        public int CustomerId { get; set; }
    }

    public class CreateNewContractCommandHanlder : IRequestHandler<CreateNewContractCommand, Response<int>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public CreateNewContractCommandHanlder(IContractRepository contractRepository, IAuthenticatedUserService authenticatedUserService)
        {
            _contractRepository = contractRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<int>> Handle(CreateNewContractCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var contract = new Domain.Entities.Contract
            {
                CompanyId = currentUser.CompanyId.Value,
                ContractTypeId = request.ContractTypeId,
                ExpiredDate = request.ContractExpiredDate,
                Status = ContractStatus.Draft,
                TotalValue = request.ContractValue,
                CreatedBy = currentUser.UserName,
                Created = DateTime.Now,
                ContractPlace = request.ContractPlace,
                Content =  request.ContractContent,
                Name = request.ContractName,
                ContractNum = request.ContractNum,
                Title = request.ContractTitle,
                CustomerId = request.CustomerId
            };

            await _contractRepository.AddAsync(contract);

            return new Response<int>(contract.Id);
        }
    }
}
