using LibraryManagement.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.DataAccess;

public static class DataSeeder
{

    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

        if (!await dbContext.Authors.AnyAsync())
        {
            var authors = new List<Author>
            {
                new Author { FirstName = "Jane", LastName = "Austen", YearOfBirth = 1775 },
                new Author { FirstName = "Charlotte", LastName = "Bronte", YearOfBirth = 1816 },
                new Author { FirstName = "Emily", LastName = "Bronte", YearOfBirth = 1818 },
                new Author { FirstName = "George", LastName = "Orwell", YearOfBirth = 1903 },
                new Author { FirstName = "Agatha", LastName = "Christie", YearOfBirth = 1890 }
            };
            await dbContext.Authors.AddRangeAsync(authors);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Categories.AnyAsync())
        {
            var cats = new List<Category>
            {
                new Category { Name = "Mystery" },
                new Category { Name = "Romance" },
                new Category { Name = "Drama" },
            };

            await dbContext.Categories.AddRangeAsync(cats);
            await dbContext.SaveChangesAsync();
        }
    }
}
