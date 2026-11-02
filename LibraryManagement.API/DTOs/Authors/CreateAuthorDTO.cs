using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Authors;

public record CreateAuthorDTO
{
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; init; }

    [Required]
    [MaxLength(75)]
    public required string LastName { get; init; }

    [Range(-5000, 2026)]
    public int? YearOfBirth { get; init; }
}
