using FluentValidation;
using WorkHive.Common.ErrorCodes;

namespace WorkHive.Api.Models.Validators
{
    public class RegistrationValidator
        : AbstractValidator<RegistrationModel>
    {
        public RegistrationValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithErrorCode(RegistrationErrorCodes.NameRequired.ToString());

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithErrorCode(RegistrationErrorCodes.EmailRequired.ToString())
                .EmailAddress()
                .WithErrorCode(RegistrationErrorCodes.EmailInvalid.ToString());

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode(RegistrationErrorCodes.PasswordRequired.ToString());
        }
    }
}
