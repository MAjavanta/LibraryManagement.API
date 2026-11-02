using FluentValidation;
using LibraryManagement.API.DTOs.Categories;

namespace LibraryManagement.API.Validator.Categories;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
{
    public UpdateCategoryValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}


