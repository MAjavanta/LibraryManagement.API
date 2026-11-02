using FluentValidation;
using LibraryManagement.API.DataAccess;
using LibraryManagement.API.DTOs.Categories;
using LibraryManagement.API.Endpoints;
using LibraryManagement.API.Mapping;
using LibraryManagement.API.Middleware;
using LibraryManagement.API.Validator.Categories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("LibraryDb")
    ));
builder.Services.AddScoped<AuthorMapper>();
builder.Services.AddScoped<CategoryMapper>();
builder.Services.AddScoped<IValidator<CreateCategoryDTO>, CreateCategoryValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryDTO>, UpdateCategoryValidator>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await DataSeeder.SeedData(app.Services);
}

app.MapCategories();
app.MapControllers();

app.Run();