namespace PrintShop.Domain.Models
{
    public class Category
    {
        public const int CATEGORY_NAME_MAX_LENGTH = 100;

        public Guid Id { get; }
        public string Name { get; }

        private Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public (string? error, Category? category) Create(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ("The name is null", null);

            if (name.Length > CATEGORY_NAME_MAX_LENGTH)
                return ($"Category's name can't be more than {CATEGORY_NAME_MAX_LENGTH} chars", null);

            var category = new Category(id, name);

            return (null, category);
        }

    }
}
