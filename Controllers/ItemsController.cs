using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BazaarResearchApp.Models;

namespace BazaarResearchApp.Controllers
{
    public class ItemsController : Controller
    {
        private readonly BazaarResearchContext _context;

        public ItemsController(BazaarResearchContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index(int id)
        {
            if (id == 0)
            {
                return View(new ItemViewModel()
                {
                    Item = new Item(),
                    Searches = new List<Search>()
                });
            }
            ItemViewModel vm = new ItemViewModel()
            {
                Item = _context.Items.Where(x => x.Id == id).Select(x => x).First(),
                Searches = await _context.Searches.Where(x => x.ItemId == id).Select(x => x).ToListAsync()
            };
            if(_context.Recipes.Select(x => x.ItemId).Contains(id))
            {
                var dic = new Dictionary<Item, (int, int)>();
                Profit profit = new Profit()
                {
                    Recipe = dic
                };
                var recipes = _context.Recipes.Select(x => x).Where(x => x.ItemId == id).ToList();
                foreach(var recipe in recipes)
                {
                    var material = _context.Items.Select(x => x).Where(x => x.Id == recipe.MaterialId).First();
                    var bazaarPrice = _context.Searches
                        .OrderBy(x => x.Id)
                        .Where(x => x.ItemId == recipe.MaterialId)
                        .Select(x => x.SinglePriceAverage)
                        .LastOrDefault() ?? 0;
                    var materialPrice = (material.NpcPrice) == 0 ?  bazaarPrice: material.NpcPrice;
                    profit.Recipe.Add(material, (materialPrice, recipe.Quantity));
                }
                profit.FailProfit = CalcMaterialProfit(id, profit.Recipe, 0);
                profit.Success0Profit = CalcMaterialProfit(id, profit.Recipe, 1);
                profit.Success1Profit = CalcMaterialProfit(id, profit.Recipe, 2);
                profit.Success2Profit = CalcMaterialProfit(id, profit.Recipe, 3);
                profit.Success3Profit = CalcMaterialProfit(id, profit.Recipe, 10);
                vm.Profit = profit;
            }
            return View(vm);
        }
        private int CalcMaterialProfit(int itemId, Dictionary<Item, (int, int)> recipes, int quantity)
        {
            var profit = (_context.Searches.OrderBy(x => x.Id).Where(x => x.ItemId == itemId).Select(x => x.SinglePriceAverage).LastOrDefault() ?? 0) * quantity;
            foreach (var recipe in recipes)
            {
                var (price, materialQuantity) = recipe.Value;
                profit = profit - price * materialQuantity;
                Console.WriteLine($"Profit after deducting {price} * {materialQuantity}: {profit}");
            }
            return profit;

        }
        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Url,NpcPrice")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Url,NpcPrice")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'BazaarResearchContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
