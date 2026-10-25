using LibraryManagement.API.Entities;

namespace LibraryManagement.API.Endpoints;

public static class CategoriesEndpoint
{
    private static List<Category> _categories = [

        new Category { Id = 1, Name = "Horror"},
        new Category { Id = 2, Name = "Biography"}
    ];

    public static void MapCategories(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/categories")
            .WithTags("Categories");

        group.MapGet("/", GetAll);
    }
    private static async Task<IResult> GetAll()
    {
        var categories = _categories;
        return Results.Ok(categories);
    }

}
