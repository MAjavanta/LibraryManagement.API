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
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName(GET_BY_ID);
        group.MapGet("/", GetAll);
        group.MapPost("/", Create)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
    private static async Task<Ok<List<Category>>> GetAll()
    {
        var categories = _categories;
        return TypedResults.Ok(categories);
    }

    private static async Task<Results<Ok<Category>, ProblemHttpResult>> GetById(int id)
    {
        var category = _categories.FirstOrDefault(cat => cat.Id == id);

        if (category is null)
        {
            return TypedResults.Problem(
                detail: $"Category with Id: {id} not found",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return TypedResults.Ok(category);
    }

    private static async Task<Results<CreatedAtRoute<Category>, ProblemHttpResult>> Create(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return TypedResults.Problem(
                detail: "Category name cannot be empty",
                statusCode: StatusCodes.Status400BadRequest
            );

        if (_categories.Any(cat => cat.Id == category.Id))
            return TypedResults.Problem(
                detail: $"Id {category.Id} is already taken",
                statusCode: StatusCodes.Status409Conflict
            );

        if (_categories.Any(cat => cat.Name == category.Name))
            return TypedResults.Problem(
                detail: $"Category Name: {category.Name} already exists",
                statusCode: StatusCodes.Status409Conflict
            );

        _categories.Add(category);

        return TypedResults.CreatedAtRoute(
            category,
            GET_BY_ID,
            new { Id = category.Id }
        );
    }

}
