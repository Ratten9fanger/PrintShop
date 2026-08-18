namespace PrintShop.DataAccess.Entities
{
    public class UserEntity //описываем уровень хранения, никакой валидации
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
