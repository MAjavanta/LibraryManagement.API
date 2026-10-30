using LibraryManagement.API.DataAccess;
using LibraryManagement.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Endpoints;

public static class CategoriesEndpoint
{

    private const string GET_BY_ID = "GetCategoryById";

    public static void MapCategories(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/categories")
            .WithTags("Categories");

        group.MapGet("/{id:int}", GetById)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName(GET_BY_ID);

        group.MapPut("/{id:int}", Update)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapDelete("/{id:int}", Delete)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/", GetAll);

        group.MapPost("/", Create)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict);

    }
    private static async Task<Ok<List<Category>>> GetAll(LibraryDbContext dbContext)
    {
        var categories = await dbContext.Categories.ToListAsync();
        return TypedResults.Ok(categories);
    }

    private static async Task<Results<Ok<Category>, ProblemHttpResult>> GetById(
        int id,
        LibraryDbContext dbContext)
    {
        var category = await dbContext.Categories.FindAsync(id);

        if (category is null)
        {
            return TypedResults.Problem(
                detail: $"Category with Id: {id} not found",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return TypedResults.Ok(category);
    }

    private static async Task<Results<CreatedAtRoute<Category>, ProblemHttpResult>> Create(
        Category category,
        LibraryDbContext dbContext)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            return TypedResults.Problem(
                detail: "Category name cannot be empty",
                statusCode: StatusCodes.Status400BadRequest
            );

        if (await dbContext.Categories.AnyAsync(cat => cat.Name == category.Name))
            return TypedResults.Problem(
                detail: $"Category Name: {category.Name} already exists",
                statusCode: StatusCodes.Status409Conflict
            );

        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        return TypedResults.CreatedAtRoute(
            category,
            GET_BY_ID,
            new { Id = category.Id }
        );
    }

    private static async Task<Results<Ok<Category>, ProblemHttpResult>> Update(
        int id,
        Category updatedCategory,
        LibraryDbContext dbContext
    )
    {
        if (string.IsNullOrWhiteSpace(updatedCategory.Name))
            return TypedResults.Problem(
                detail: "Category name cannot be empty",
                statusCode: StatusCodes.Status400BadRequest
            );

        var ogCategory = await dbContext.Categories.FindAsync(id);

        if (ogCategory is null)
            return TypedResults.Problem(
                detail: $"Category with Id {id} cannot be found",
                statusCode: StatusCodes.Status404NotFound
            );

        if (await dbContext.Categories.AnyAsync(cat =>
            cat.Id != id &&
            cat.Name == updatedCategory.Name))
        {
            return TypedResults.Problem(
                detail: $"Category with Name: {updatedCategory.Name} already exists",
                statusCode: StatusCodes.Status409Conflict
            );
        }

        ogCategory.Name = updatedCategory.Name;
        await dbContext.SaveChangesAsync();

        return TypedResults.Ok(ogCategory);
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> Delete(
        int id,
        LibraryDbContext dbContext)
    {
        var category = await dbContext.Categories.FindAsync(id);

        if (category is null)
            return TypedResults.Problem(
                detail: $"Category with Id {id} cannot be found",
                statusCode: StatusCodes.Status404NotFound
            );

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}
