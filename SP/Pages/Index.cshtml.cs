using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SP.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("user authenticated");
        }
        else{
             _logger.LogInformation("user not authenticated");
        }
    }
}
