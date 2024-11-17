using Commersant.Models;

namespace Commersant.Data
{
    public class DatabaseFiller
    {
        public static void SeedData(CommersantDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Categories.Any())
            {
                var categories = Enumerable.Range(1, 100).Select(i => new Category
                {
                    Name = $"Category {i}"
                }).ToList();

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Items.Any())
            {
                var random = new Random();
                var items = new List<Item>();

                for (int i = 1; i <= 1_000_000; i++)
                {
                    items.Add(new Item
                    {
                        Name = $"Item {i}",
                        Description = $"Description for item {i}",
                      
                    });


                    if (i % 10_000 == 0)
                    {
                        context.Items.AddRange(items);
                        context.SaveChanges();
                        items.Clear();
                        Console.WriteLine($"{i} items added...");
                    }
                }

                if (items.Any())
                {
                    context.Items.AddRange(items);
                    context.SaveChanges();
                }

                Console.WriteLine("Items seeded successfully.");
            }

            if (!context.ItemCategories.Any())
            {
                var items = context.Items.AsQueryable().Take(1_000_000).ToList();
                var categories = context.Categories.ToList();
                var random = new Random();
                var itemCategories = new List<ItemCategory>();

                for (int i = 0; i < items.Count; i++)
                {
                    // 90% объектов связываем с одной или несколькими категориями
                    if (i < items.Count * 0.9)
                    {
                        var linkedCategories = categories.OrderBy(c => random.Next()).Take(random.Next(1, 5)).ToList();

                        foreach (var category in linkedCategories)
                        {
                            itemCategories.Add(new ItemCategory
                            {
                                ItemId = items[i].Id,
                                CategoryId = category.Id
                            });
                        }
                    }

                    // Сохраняем партиями каждые 10,000 записей
                    if (i % 10_000 == 0)
                    {
                        context.ItemCategories.AddRange(itemCategories);
                        context.SaveChanges();
                        itemCategories.Clear();
                        Console.WriteLine($"{i} item-category links added...");
                    }
                }

                // Сохраняем остатки
                if (itemCategories.Any())
                {
                    context.ItemCategories.AddRange(itemCategories);
                    context.SaveChanges();
                }

                Console.WriteLine("Item-Category links seeded successfully.");
            }
        }
    }
}
