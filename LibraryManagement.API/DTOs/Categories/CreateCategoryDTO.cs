namespace LibraryManagement.API.DTOs.Categories;

public record CreateCategoryDTO
{
    public required string Name { get; init; }
}
