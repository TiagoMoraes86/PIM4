using SistemaChamados.Web.Models;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Interface para repositório de usuários
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Busca um usuário por email
        /// </summary>
        Task<Usuario?> ObterPorEmailAsync(string email);

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        Task<bool> CriarAsync(Usuario usuario);

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        Task<bool> AtualizarAsync(Usuario usuario);

        /// <summary>
        /// Verifica se um email já está cadastrado
        /// </summary>
        Task<bool> EmailExisteAsync(string email);
    }
}
