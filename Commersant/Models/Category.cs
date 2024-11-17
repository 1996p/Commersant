namespace Commersant.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>(); // Навигационное свойство
    }
}
