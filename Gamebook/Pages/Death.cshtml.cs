using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gamebook.Pages;

public class Death : PageModel
{
    public void OnGet()
    {
        TempData["menu"] = true;
    }
}