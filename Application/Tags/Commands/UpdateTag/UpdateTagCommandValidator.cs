using TODO.Application.Interfaces;
using FluentValidation;


namespace TODO.Application.Tags.Commands.UpdateTagCommand
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        public UpdateTagCommandValidator(ITagRepository tagRepository) {
            _tagRepository = tagRepository;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название обязательно!");
        }
    }
}
