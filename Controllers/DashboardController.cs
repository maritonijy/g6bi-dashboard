using Microsoft.AspNetCore.Mvc;

namespace KovaDashboard.Controllers;

[Route("dashboard")]
public class DashboardController : Controller
{
    [HttpGet("1")]
    public IActionResult Executive() => View();

    [HttpGet("2")]
    public IActionResult Profitability() => View();

    [HttpGet("3")]
    public IActionResult Cash() => View();
}
