using Commersant.Data;
using Commersant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Commersant.Controllers
{
    public class ItemsController : ControllerBase
    {
        private readonly CommersantDbContext _context;

        public ItemsController(CommersantDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все объекты по заданной категории
        /// </summary>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <returns>Список объектов</returns>
        [HttpGet("byCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByCategory(int categoryId)
        {
            
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == categoryId);
            if (!categoryExists)
            {
                return NotFound(new { message = "Category not found." });
            }

            var items = await _context.ItemCategories
                .AsNoTracking()
                .Where(ic => ic.CategoryId == categoryId)
                .Select(ic => ic.Item)
                .ToListAsync();

            return Ok(items);
        }

    }
}

