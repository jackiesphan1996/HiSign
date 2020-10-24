using FluentValidation;

namespace Hisign.Application.Features.Company.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(x => x.Address).NotNull().WithMessage("Required");
        }
    }
}
