
namespace PrintShop.Infrastructure
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;

        public int ExpireMins { get; set; }
    }
}
