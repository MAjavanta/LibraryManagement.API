using LibraryManagement.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.DataAccess;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options)
    : DbContext(options)
{

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
}
