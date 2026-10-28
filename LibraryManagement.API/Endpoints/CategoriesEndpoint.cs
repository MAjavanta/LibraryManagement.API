using LibraryManagement.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

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

        group.MapGet("/{id:int}", GetById)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        group.MapGet("/", GetAll)
            .Produces(StatusCodes.Status200OK);
    }
    private static async Task<Ok<List<Category>>> GetAll()
    {
        var categories = _categories;
        return TypedResults.Ok(categories);
    }

    private static async Task<Results<Ok<Category>, NotFound>> GetById(int id)
    {
        var category = _categories.FirstOrDefault(cat => cat.Id == id);

        if (category is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(category);
    }

}
