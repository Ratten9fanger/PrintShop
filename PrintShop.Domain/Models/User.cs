using System.ComponentModel.DataAnnotations;

namespace PrintShop.Domain.Models
{
    public class User
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Role { get; }
        public string PasswordHash { get; }

        private User(Guid id, string email, string role, string password)
        {
            Id = id;
            Email = email;
            Role = role;
            PasswordHash = password;
        }

        public static (string? error, User? user) Create(
            Guid id,
            string email,
            string role,
            string passwordHash)
        { 
            if (string.IsNullOrWhiteSpace(email))
                return("The email is empty", null);

            var user = new User(id, email, role, passwordHash);

            return (null, user);
        }

}
}
