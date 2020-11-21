using HiSign.Application.Interfaces;
using HiSign.Application.Interfaces.Repositories;
using HiSign.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HiSign.Application.Features.ContractType.Queries.GetAllContractTypes
{
    public class GetAllContractTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class GetAllContractTypesQuery : IRequest<Response<IEnumerable<GetAllContractTypeViewModel>>>
    {
    }

    public class GetAllContractTypesQueryHandler : IRequestHandler<GetAllContractTypesQuery, Response<IEnumerable<GetAllContractTypeViewModel>>>
    {
        private readonly IContractTypeRepository _contractTypeRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetAllContractTypesQueryHandler(IContractTypeRepository contractTypeRepository, IAuthenticatedUserService authenticatedUserService)
        {
            _contractTypeRepository = contractTypeRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<IEnumerable<GetAllContractTypeViewModel>>> Handle(GetAllContractTypesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _authenticatedUserService.GetCurentApplicationUser();

            var contracTypes = await _contractTypeRepository.GetAllByCompanyIdAsync(currentUser.CompanyId.GetValueOrDefault());

            var res = contracTypes.Select(x => new GetAllContractTypeViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Content = x.Content,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.Created
            });

            return new Response<IEnumerable<GetAllContractTypeViewModel>>(res);
        }
    }
}
