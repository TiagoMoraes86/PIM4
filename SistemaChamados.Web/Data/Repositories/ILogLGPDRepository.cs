using SistemaChamados.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Interface para repositório de logs LGPD
    /// </summary>
    public interface ILogLGPDRepository
    {
        /// <summary>
        /// Registra um novo log de acesso
        /// </summary>
        Task<bool> RegistrarAsync(LogLGPD log);

        /// <summary>
        /// Lista logs de um usuário específico
        /// </summary>
        Task<List<LogLGPD>> ListarPorUsuarioAsync(string email, int limite = 100);

        /// <summary>
        /// Lista todos os logs (para auditoria)
        /// </summary>
        Task<List<LogLGPD>> ListarTodosAsync(int limite = 100);
    }
}
