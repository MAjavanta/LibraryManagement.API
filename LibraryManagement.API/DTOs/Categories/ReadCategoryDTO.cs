namespace LibraryManagement.API.DTOs.Categories;

public record ReadCategoryDTO
{
    public int Id { get; init; }
    public required string Name { get; init; }
}
