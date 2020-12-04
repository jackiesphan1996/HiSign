using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using HiSign.Application.Exceptions;

namespace HiSign.Application.Features.ContractType.Commands.UpdateContractType
{
    public class UpdateContractTypeCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class UpdateContractTypeCommandHandler : IRequestHandler<UpdateContractTypeCommand, Response<bool>>
    {
        private readonly IContractTypeRepository _contractTypeRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UpdateContractTypeCommandHandler(IContractTypeRepository contractTypeRepository,
            IAuthenticatedUserService authenticatedUserService)
        {
            _contractTypeRepository = contractTypeRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<bool>> Handle(UpdateContractTypeCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var contractType = await _contractTypeRepository.GetByIdAsync(request.Id);

            if (contractType == null)
            {
                throw new ApiException($"ContractType Id {request.Id} does not exist.");
            }

            contractType.Content = request.Content;
            contractType.Name = request.Name;
            contractType.LastModified = DateTime.Now;
            contractType.LastModifiedBy = currentUser.UserName;

            await _contractTypeRepository.UpdateAsync(contractType);

            return new Response<bool>(true);
        }
    }
}
