using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HiSign.Application.Features.ContractType.Commands.CreateContractType
{
    public class CreateContractTypeCommand : IRequest<Response<bool>>
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class CreateContractTypeCommandHandler : IRequestHandler<CreateContractTypeCommand, Response<bool>>
    {
        private readonly IContractTypeRepository _contractTypeRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public CreateContractTypeCommandHandler(IContractTypeRepository contractTypeRepository,
            IAuthenticatedUserService authenticatedUserService)
        {
            _contractTypeRepository = contractTypeRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<bool>> Handle(CreateContractTypeCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();
            var contractType = new HiSign.Domain.Entities.ContractType
            {
                CompanyId = currentUser.CompanyId.GetValueOrDefault(),
                Content = request.Content,
                Name = request.Name,
                CreatedBy = currentUser.UserName,
                LastModified = DateTime.Now
            };

            await _contractTypeRepository.AddAsync(contractType);

            return new Response<bool>(true);
        }
    }
}
