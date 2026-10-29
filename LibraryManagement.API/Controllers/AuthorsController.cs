using System;
using System.Linq.Expressions;
using LibraryManagement.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthorsController : ControllerBase
{
    private static List<Author> _authors = [
        new Author { Id = 1, FirstName = "Jane", LastName = "Austen", YearOfBirth = 1775 },
        new Author { Id = 2, FirstName = "Charlotte", LastName = "Bronte", YearOfBirth = 1816 },
        new Author { Id = 3, FirstName = "Emily", LastName = "Bronte", YearOfBirth = 1818 },
        new Author { Id = 4, FirstName = "Veronica", LastName = "Roth", YearOfBirth = 1988 }
    ];

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Author>> GetById(int id)
    {
        var author = _authors.FirstOrDefault(author =>
            author.Id == id);

        if (author is null)
            return Problem(
                detail: $"Author with Id: {id} not found",
                statusCode: StatusCodes.Status404NotFound
            );

        return Ok(author);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Author>>> GetAll()
    {
        return Ok(_authors);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Author), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Author>> Create(Author newAuthor)
    {
        if (string.IsNullOrWhiteSpace(newAuthor.FirstName)
            || string.IsNullOrWhiteSpace(newAuthor.LastName))
        {
            return Problem(
                detail: "Author must have a first and last name",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        if (_authors.Any(author => author.Id == newAuthor.Id))
        {
            return Problem(
                detail: $"Author with Id: {newAuthor.Id} already exists",
                statusCode: StatusCodes.Status409Conflict
            );
        }

        Console.WriteLine("Id valid and names valid");

        _authors.Add(newAuthor);

        return CreatedAtAction(
            nameof(GetById),
            new { id = newAuthor.Id },
            newAuthor);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Author>> Update(int id, Author updatedAuthor)
    {
        if (string.IsNullOrWhiteSpace(updatedAuthor.FirstName)
            || string.IsNullOrWhiteSpace(updatedAuthor.LastName))
        {
            return Problem(
                detail: "Author must have a first and last name",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var author = _authors.FirstOrDefault(author =>
            author.Id == id);

        if (author is null)
            return Problem(
                detail: $"Author with Id: {id} cannot be found",
                statusCode: StatusCodes.Status404NotFound
            );

        author.FirstName = updatedAuthor.FirstName;
        author.LastName = updatedAuthor.LastName;
        author.YearOfBirth = updatedAuthor.YearOfBirth;

        return Ok(author);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var author = _authors.FirstOrDefault(author =>
            author.Id == id);

        if (author is null)
            return Problem(
                detail: $"Author with Id: {id} cannot be found",
                statusCode: StatusCodes.Status404NotFound
            );

        _authors.Remove(author);

        return NoContent();
    }

}
