using BloggingPlatformAPI.DTOs;
using FluentValidation;
using System.Collections.Immutable;

namespace BloggingPlatformAPI.Validators
{
    public class BlogUpdateValidator : AbstractValidator<BlogUpdateDTO>
    {
        public BlogUpdateValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title must have a value");
            RuleFor(x => x.Title).Length(5, 50).WithMessage("Title length must be between 5 and 50.");
            RuleFor(x => x.Content).MinimumLength(10).WithMessage("Content must have a minimum length of 10.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Post must have a category.");
            RuleFor(x => x.Tags).NotNull().WithMessage("Blog must have at least one tag.");
        }
    }
}
