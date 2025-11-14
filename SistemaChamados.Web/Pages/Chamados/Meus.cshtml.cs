using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Models;
using SistemaChamados.Web.Services;
using System.Collections.Generic;

namespace SistemaChamados.Web.Pages.Chamados
{
    public class MeusModel : PageModel
    {
        private readonly IChamadoService _chamadoService;

        public MeusModel(IChamadoService chamadoService)
        {
            _chamadoService = chamadoService;
        }

        public List<Chamado> Chamados { get; set; } = new List<Chamado>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                Chamados = await _chamadoService.ListarChamadosUsuarioAsync(userEmail);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erro ao listar chamados: {ex.Message}";
                return Page();
            }
        }
    }
}
