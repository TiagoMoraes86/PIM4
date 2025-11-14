using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Models;
using SistemaChamados.Web.Services;

namespace SistemaChamados.Web.Pages.Chamados
{
    public class DetalhesModel : PageModel
    {
        private readonly IChamadoService _chamadoService;

        public DetalhesModel(IChamadoService chamadoService)
        {
            _chamadoService = chamadoService;
        }

        public Chamado Chamado { get; set; } = new Chamado();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var chamado = await _chamadoService.ObterChamadoPorIdAsync(id, userEmail);
                
                if (chamado == null)
                {
                    return NotFound();
                }

                Chamado = chamado;
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao carregar chamado: {ex.Message}";
                return RedirectToPage("/Chamados/Meus");
            }
        }

        public async Task<IActionResult> OnPostAsync(int chamadoId, string comentario)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                await _chamadoService.AdicionarComentarioAsync(chamadoId, comentario, userEmail);
                TempData["SuccessMessage"] = "Comentário adicionado com sucesso!";
                return RedirectToPage(new { id = chamadoId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao adicionar comentário: {ex.Message}";
                return RedirectToPage(new { id = chamadoId });
            }
        }
    }
}
