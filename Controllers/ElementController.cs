using Microsoft.AspNetCore.Mvc;
using KovaDashboard.Models;

namespace KovaDashboard.Controllers;

[Route("element")]
public class ElementController : Controller
{
    [HttpGet("{id}")]
    public IActionResult Details(string id)
    {
        var all = ElementCatalog.All;
        var index = all.FindIndex(e => e.Id == id);
        if (index < 0) return NotFound();

        var item = all[index];
        var sameDashboard = all.Where(e => e.Dashboard == item.Dashboard).ToList();
        var position = sameDashboard.FindIndex(e => e.Id == id) + 1;

        var page = new ElementPage(
            item,
            $"Dashboard {item.Dashboard} · Element {position} of {sameDashboard.Count}",
            index > 0 ? all[index - 1] : null,
            index < all.Count - 1 ? all[index + 1] : null);

        return View(page);
    }
}
