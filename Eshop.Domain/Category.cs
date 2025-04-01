using System.Diagnostics.CodeAnalysis;

namespace Eshop.Domain
{
    public class Category
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [ExcludeFromCodeCoverage]
        private Category() { } // private ctor needed for a persistence - Entity Framework
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public Category(int id, string name)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(id);

            ValidateParameters(name);

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; private set; }

        public void Udpate(string name)
        {
            ValidateParameters(name);

            Name = name;
        }

        private static void ValidateParameters(string name)
        {
            if (string.IsNullOrEmpty(name?.Trim()) || name.Length > 50)
                throw new ArgumentNullException(nameof(name));
        }
    }
}