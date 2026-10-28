using LibraryManagement.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagement.API.Endpoints;

public static class CategoriesEndpoint
{

    private const string GET_BY_ID = "GetCategoryById";
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
            .WithName(GET_BY_ID)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        group.MapGet("/", GetAll)
            .Produces(StatusCodes.Status200OK);
        group.MapPost("/", Create);
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

    private static async Task<Results<CreatedAtRoute<Category>, Conflict, BadRequest>> Create(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return TypedResults.BadRequest();

        if (_categories.Any(cat => cat.Id == category.Id))
            return TypedResults.Conflict();

        if (_categories.Any(cat => cat.Name == category.Name))
            return TypedResults.Conflict();

        _categories.Add(category);

        return TypedResults.CreatedAtRoute(
            category,
            GET_BY_ID,
            new { Id = category.Id }
        );
    }

}
