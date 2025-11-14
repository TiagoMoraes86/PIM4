using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SistemaChamados.Web.Services;

namespace SistemaChamados.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Senha { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // Limpar sessão ao acessar página de login
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Validar credenciais
                var usuario = await _authService.ValidarCredenciaisAsync(Email, Senha);

                if (usuario == null)
                {
                    ErrorMessage = "Usuário ou senha inválidos";
                    return Page();
                }

                // Obter IP e User Agent
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                // Registrar login (log LGPD)
                await _authService.RegistrarLoginAsync(Email, ipAddress ?? "unknown", userAgent);

                // Salvar dados na sessão
                HttpContext.Session.SetString("UserEmail", usuario.Email);
                HttpContext.Session.SetString("UserNome", usuario.Nome);
                HttpContext.Session.SetString("UserTipo", usuario.Tipo);

                // Redirecionar para dashboard
                return RedirectToPage("/Dashboard/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao fazer login: {ex.Message}";
                return Page();
            }
        }
    }
}
