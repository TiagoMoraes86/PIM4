using SistemaChamados.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Interface para serviço de chamados
    /// </summary>
    public interface IChamadoService
    {
        /// <summary>
        /// Cria um novo chamado
        /// </summary>
        Task<int> CriarChamadoAsync(Chamado chamado, string usuarioEmail);

        /// <summary>
        /// Obtém um chamado por ID
        /// </summary>
        Task<Chamado?> ObterChamadoPorIdAsync(int id, string usuarioEmail);

        /// <summary>
        /// Lista chamados do usuário
        /// </summary>
        Task<List<Chamado>> ListarChamadosUsuarioAsync(string email, string? filtroStatus = null);

        /// <summary>
        /// Adiciona um comentário ao chamado
        /// </summary>
        Task<bool> AdicionarComentarioAsync(int chamadoId, string comentario, string usuarioEmail);

        /// <summary>
        /// Finaliza um chamado
        /// </summary>
        Task<bool> FinalizarChamadoAsync(int chamadoId, string solucao, string usuarioEmail);
    }
}
