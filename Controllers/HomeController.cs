using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BazaarResearchApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BazaarResearchApp.Controllers;

public class HomeController : Controller
{
    private readonly BazaarResearchContext _context;

    public HomeController(BazaarResearchContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Search(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            SearchViewModel vm = new SearchViewModel()
            {
                Items = await _context.Items
                    .Where(x => x.Name.Contains(searchString))
                    .ToListAsync(),
            };

            return View(vm);
        }

        else
        {
            return RedirectToAction(nameof(Index));
        }
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

