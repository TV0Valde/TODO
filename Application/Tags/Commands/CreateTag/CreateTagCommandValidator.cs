using TODO.Persistence.Interfaces;
using FluentValidation;


namespace TODO.Application.Tags.Commands.CreateTagCommand
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        public CreateTagCommandValidator(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название обязательно!");
        }
    }
}
