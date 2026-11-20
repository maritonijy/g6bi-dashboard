using Microsoft.AspNetCore.Mvc;

namespace KovaDashboard.Controllers;

public class HomeController : Controller
{
    // Open the presentation on its title slide when the site is visited.
    public IActionResult Index() => RedirectToAction("Index", "Presentation");

    // Star schema and measures
    public IActionResult Schema() => View();
}
