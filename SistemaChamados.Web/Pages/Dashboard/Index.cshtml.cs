using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Models.ViewModels;
using SistemaChamados.Web.Services;

namespace SistemaChamados.Web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IRelatorioService _relatorioService;

        public IndexModel(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        public DashboardViewModel DashboardData { get; set; } = new DashboardViewModel();

        public async Task<IActionResult> OnGetAsync()
        {
            // Verificar se está logado
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var nomeUsuario = HttpContext.Session.GetString("UserNome") ?? "Usuário";
                var tipoUsuario = HttpContext.Session.GetString("UserTipo") ?? "comum";

                DashboardData = await _relatorioService.ObterDadosDashboardAsync(nomeUsuario, tipoUsuario);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar dashboard: {ex.Message}";
                return Page();
            }
        }
    }
}
