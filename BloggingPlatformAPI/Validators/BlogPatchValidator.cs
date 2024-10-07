using BloggingPlatformAPI.DTOs;
using FluentValidation;

namespace BloggingPlatformAPI.Validators
{
    public class BlogPatchValidator : AbstractValidator<BlogUpdateDTO>
    {
        public BlogPatchValidator() 
        {
            RuleFor(x => x.Title)
                .Length(5, 50)
                .WithMessage("Title length must be between 5 and 50.")
                .When(x => !string.IsNullOrWhiteSpace(x.Title));
            RuleFor(x => x.Content)
                .MinimumLength(10)
                .WithMessage("Content must have a minimum length of 10.")
                .When(x => !string.IsNullOrWhiteSpace(x.Content));
        }
    }
}
