using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp;

namespace Server.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ServerEngine _engine;
        public SearchModel(ServerEngine engine) => _engine = engine;

        public async Task OnGetAsync(string? q, CancellationToken ct)
        {
            if (q == "clear")
                await _engine.ClearLeaseOwnersAsync(ct);
        }
    }
}
