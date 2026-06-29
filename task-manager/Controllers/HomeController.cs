using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace task_manager.Controllers;

public class HomeController(IStringLocalizer<HomeController> localizer) : Controller
{
    private readonly IStringLocalizer<HomeController> _localizer = localizer;

    public IActionResult Index()
    {
        ViewBag.Greeting = _localizer["buenosDias"];
        return View();
    }
}
