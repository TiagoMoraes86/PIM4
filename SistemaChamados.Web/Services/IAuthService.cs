using SistemaChamados.Web.Models;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Interface para serviço de autenticação
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Valida as credenciais do usuário
        /// </summary>
        Task<Usuario?> ValidarCredenciaisAsync(string email, string senha);

        /// <summary>
        /// Registra o login do usuário (log LGPD)
        /// </summary>
        Task RegistrarLoginAsync(string email, string ipAddress, string userAgent);

        /// <summary>
        /// Cria hash da senha
        /// </summary>
        string HashSenha(string senha);

        /// <summary>
        /// Verifica se a senha corresponde ao hash
        /// </summary>
        bool VerificarSenha(string senha, string hash);
    }
}
