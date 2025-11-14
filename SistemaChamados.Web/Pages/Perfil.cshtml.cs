using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SistemaChamados.Web.Pages
{
    public class PerfilModel : PageModel
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Tipo { get; set; }

        public void OnGet()
        {
            Nome = HttpContext.Session.GetString("UserNome");
            Email = HttpContext.Session.GetString("UserEmail");
            Tipo = HttpContext.Session.GetString("UserTipo");
        }
    }
}
