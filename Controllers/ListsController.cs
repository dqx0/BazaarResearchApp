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
    public class ListsController : Controller
    {
        private readonly BazaarResearchContext _context;

        public ListsController(BazaarResearchContext context)
        {
            _context = context;
        }

        // GET: Lists
        public async Task<IActionResult> Index(int id)
        {
            var bazaarResearchContext = await _context.Lists.Where(x => x.SearchId == id).Select(x => x).ToListAsync();
            int itemId = _context.Searches.Where(x => x.Id == id).Select(x => x.ItemId).FirstOrDefault();
            string itemName = _context.Items.Where(x => x.Id == itemId).Select(x => x.Name).FirstOrDefault() ?? "";
            string date = _context.Searches.Where(x => x.Id == id).Select(x => x.CreatedAt).FirstOrDefault().ToString("yyyy年MM月dd日 HH時mm分") ?? "";
            ListsViewModel vm = new ListsViewModel()
            {
                Lists = bazaarResearchContext,
                ItemName = itemName,
                Date = date
            };
            return View(vm);
        }

        // GET: Lists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lists == null)
            {
                return NotFound();
            }

            var list = await _context.Lists
                .Include(l => l.Search)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // GET: Lists/Create
        public IActionResult Create()
        {
            ViewData["SearchId"] = new SelectList(_context.Searches, "Id", "Id");
            return View();
        }

        // POST: Lists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SearchId,Price,Quantity,SinglePrice,Seller,Start,End")] List list)
        {
            if (ModelState.IsValid)
            {
                _context.Add(list);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SearchId"] = new SelectList(_context.Searches, "Id", "Id", list.SearchId);
            return View(list);
        }

        // GET: Lists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lists == null)
            {
                return NotFound();
            }

            var list = await _context.Lists.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }
            ViewData["SearchId"] = new SelectList(_context.Searches, "Id", "Id", list.SearchId);
            return View(list);
        }

        // POST: Lists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SearchId,Price,Quantity,SinglePrice,Seller,Start,End")] List list)
        {
            if (id != list.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(list);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.Id))
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
            ViewData["SearchId"] = new SelectList(_context.Searches, "Id", "Id", list.SearchId);
            return View(list);
        }

        // GET: Lists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lists == null)
            {
                return NotFound();
            }

            var list = await _context.Lists
                .Include(l => l.Search)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lists == null)
            {
                return Problem("Entity set 'BazaarResearchContext.Lists'  is null.");
            }
            var list = await _context.Lists.FindAsync(id);
            if (list != null)
            {
                _context.Lists.Remove(list);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListExists(int id)
        {
          return (_context.Lists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
