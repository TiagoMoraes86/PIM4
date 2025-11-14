using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Models;
using SistemaChamados.Web.Services;

namespace SistemaChamados.Web.Pages.Chamados
{
    public class NovoModel : PageModel
    {
        private readonly IChamadoService _chamadoService;

        public NovoModel(IChamadoService chamadoService)
        {
            _chamadoService = chamadoService;
        }

        [BindProperty]
        public string Titulo { get; set; } = string.Empty;

        [BindProperty]
        public string Descricao { get; set; } = string.Empty;

        [BindProperty]
        public string Categoria { get; set; } = string.Empty;

        [BindProperty]
        public string Prioridade { get; set; } = string.Empty;

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var chamado = new Chamado
                {
                    Titulo = Titulo,
                    Descricao = Descricao,
                    Categoria = Categoria,
                    Prioridade = Prioridade,
                    Solicitante = userEmail
                };

                var chamadoId = await _chamadoService.CriarChamadoAsync(chamado, userEmail);

                TempData["SuccessMessage"] = $"Chamado #{chamadoId} criado com sucesso!";
                return RedirectToPage("/Chamados/Detalhes", new { id = chamadoId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao criar chamado: {ex.Message}";
                return Page();
            }
        }
    }
}
