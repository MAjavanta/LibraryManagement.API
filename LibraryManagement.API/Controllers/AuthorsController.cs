using LibraryManagement.API.DataAccess;
using LibraryManagement.API.DTOs.Authors;
using LibraryManagement.API.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthorsController(
        LibraryDbContext dbContext,
        AuthorMapper mapper) : ControllerBase
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

        return Ok(mapper.ToReadDto(author));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReadAuthorDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReadAuthorDTO>>> GetAll()
    {
        var authors = await dbContext.Authors.ToListAsync();
        return Ok(authors.Select(mapper.ToReadDto).ToList());
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

        var author = mapper.ToEntity(newAuthor);

        dbContext.Authors.Add(author);
        await dbContext.SaveChangesAsync();

        var readAuthorDto = mapper.ToReadDto(author);

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

        mapper.UpdateEntity(updatedAuthor, author);
        await dbContext.SaveChangesAsync();

        return Ok(mapper.ToReadDto(author));
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
