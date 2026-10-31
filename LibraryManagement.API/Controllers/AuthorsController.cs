using LibraryManagement.API.DataAccess;
using LibraryManagement.API.DTOs.Authors;
using LibraryManagement.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthorsController(LibraryDbContext dbContext) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReadAuthorDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadAuthorDTO>> GetById(int id)
    {
        var author = await dbContext.Authors.FindAsync(id);

        if (author is null)
            return Problem(
                detail: $"Author with Id: {id} not found",
                statusCode: StatusCodes.Status404NotFound
            );

        var readAuthorDTO = new ReadAuthorDTO
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            YearOfBirth = author.YearOfBirth
        };

        return Ok(readAuthorDTO);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReadAuthorDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReadAuthorDTO>>> GetAll()
    {
        var authors = await dbContext.Authors.ToListAsync();
        var readAuthorDTOs = authors.Select(author =>
            new ReadAuthorDTO
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                YearOfBirth = author.YearOfBirth
            }).ToList();
        return Ok(readAuthorDTOs);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReadAuthorDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReadAuthorDTO>> Create(CreateAuthorDTO newAuthor)
    {
        if (string.IsNullOrWhiteSpace(newAuthor.FirstName)
            || string.IsNullOrWhiteSpace(newAuthor.LastName))
        {
            return Problem(
                detail: "Author must have a first and last name",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var author = new Author
        {
            FirstName = newAuthor.FirstName,
            LastName = newAuthor.LastName,
            YearOfBirth = newAuthor.YearOfBirth
        };

        dbContext.Authors.Add(author);
        await dbContext.SaveChangesAsync();

        var readAuthorDto = new ReadAuthorDTO
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            YearOfBirth = author.YearOfBirth
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = readAuthorDto.Id },
            readAuthorDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReadAuthorDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadAuthorDTO>> Update(int id, UpdateAuthorDTO updatedAuthor)
    {
        if (string.IsNullOrWhiteSpace(updatedAuthor.FirstName)
            || string.IsNullOrWhiteSpace(updatedAuthor.LastName))
        {
            return Problem(
                detail: "Author must have a first and last name",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var author = await dbContext.Authors.FindAsync(id);

        if (author is null)
            return Problem(
                detail: $"Author with Id: {id} cannot be found",
                statusCode: StatusCodes.Status404NotFound
            );

        author.FirstName = updatedAuthor.FirstName;
        author.LastName = updatedAuthor.LastName;
        author.YearOfBirth = updatedAuthor.YearOfBirth;

        await dbContext.SaveChangesAsync();

        var readAuthorDto = new ReadAuthorDTO
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            YearOfBirth = author.YearOfBirth
        };

        return Ok(readAuthorDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var author = await dbContext.Authors.FindAsync(id);

        if (author is null)
            return Problem(
                detail: $"Author with Id: {id} cannot be found",
                statusCode: StatusCodes.Status404NotFound
            );

        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

}
