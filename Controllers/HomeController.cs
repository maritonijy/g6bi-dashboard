using Microsoft.AspNetCore.Mvc;

namespace KovaDashboard.Controllers;

public class HomeController : Controller
{
    // The presentation deck
    public IActionResult Index() => View();

    // Star schema and measures
    public IActionResult Schema() => View();
}
