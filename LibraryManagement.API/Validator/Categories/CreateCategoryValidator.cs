using FluentValidation;
using LibraryManagement.API.DTOs.Categories;

namespace LibraryManagement.API.Validator.Categories;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDTO>
{
    public CreateCategoryValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
