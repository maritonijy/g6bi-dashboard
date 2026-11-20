using Microsoft.AspNetCore.Mvc;

namespace KovaDashboard.Controllers;

[Route("presentation")]
public class PresentationController : Controller
{
    // PowerPoint-style deck: opening pages, data tables, interactive dashboards, element explanations
    [HttpGet("")]
    public IActionResult Index() => View();
}
