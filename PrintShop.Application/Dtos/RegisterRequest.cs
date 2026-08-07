using System.ComponentModel.DataAnnotations;


namespace PrintShop.Application.Dtos
{
    public record RegisterRequest(
        [EmailAddress]
        [Required]
        string Email,
        [Required]
        string Password,
        [Required]
        string RepeatedPassword
        );
}
