using FluentValidation;
using FluentValidation.Results;
using NOV.ES.TAT.Job.API.Application.Commands;
using NOV.ES.TAT.Job.DTOs;

namespace NOV.ES.TAT.Job.API.Validators
{
    public class NovJobValidator : AbstractValidator<NovJobDto>
    {
        public NovJobValidator()
        {
            RuleFor(o => o.ModuleKey).NotEmpty().WithMessage(string.Format("message{0}", "Name"));
        }
        public new ValidationResult Validate(NovJobDto instance)
        {
            return instance == null
                ? new ValidationResult(new[] { new ValidationFailure("NovJobDto", "NovJobDto cannot be null") })
                : base.Validate(instance);
        }

        public ValidationResult Validate(CreateNovJobCommand instance)
        {
            return instance == null
                ? new ValidationResult(new[] { new ValidationFailure("CreateNovJobCommand", "CreateNovJobCommand cannot be null") })
                : Validate(instance.NovJobDto);
        }
    }
}
