using System.ComponentModel.DataAnnotations;


namespace PrintShop.Application.Dtos
{
    public record LoginRequest(
        [EmailAddress]
        [Required]
        string Email,
        [Required]
        string Password
        );
}
