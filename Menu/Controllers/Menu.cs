using Microsoft.AspNetCore.Mvc;
using Menu.Models;
using Menu.Data;
using Microsoft.EntityFrameworkCore;

namespace Menu.Controllers
{
    public class Menu : Controller
    {
        private readonly MenuContext _context;
        public Menu(MenuContext context)
        {
            _context= context;
        }
        public async Task<IActionResult> Index(string searchstring)
        {
            var dishes =from d in _context.Dishes select d;
            if (!string.IsNullOrEmpty(searchstring))
            {
                dishes=dishes.Where(d => d.Name.Contains(searchstring));
                return View(await dishes.ToListAsync());
            }
            return View(await dishes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var dish=await _context.Dishes.Include(di => di.DishIngredients)
                .ThenInclude(i => i.Ingredient).FirstOrDefaultAsync(x => x.Id==id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }
    }
}
