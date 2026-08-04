using System.ComponentModel.DataAnnotations;


namespace PrintShop.Application.Dtos
{
    public record LoginRequest(
        [EmailAddress]
        [Required]
        string Email,
        [Required]
        [Range(8, 30, ErrorMessage = "Пароль от 8 до 30 символов")]
        string Password
        );
}
