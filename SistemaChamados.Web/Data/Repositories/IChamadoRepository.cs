using SistemaChamados.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Interface para repositório de chamados
    /// </summary>
    public interface IChamadoRepository
    {
        /// <summary>
        /// Cria um novo chamado
        /// </summary>
        Task<int> CriarAsync(Chamado chamado);

        /// <summary>
        /// Busca um chamado por ID
        /// </summary>
        Task<Chamado?> ObterPorIdAsync(int id);

        /// <summary>
        /// Lista todos os chamados de um usuário
        /// </summary>
        Task<List<Chamado>> ListarPorUsuarioAsync(string emailUsuario, string? filtroStatus = null);

        /// <summary>
        /// Lista todos os chamados (para técnicos/admin)
        /// </summary>
        Task<List<Chamado>> ListarTodosAsync(string? filtroStatus = null);

        /// <summary>
        /// Atualiza um chamado
        /// </summary>
        Task<bool> AtualizarAsync(Chamado chamado);

        /// <summary>
        /// Adiciona uma interação ao chamado
        /// </summary>
        Task<bool> AdicionarInteracaoAsync(int chamadoId, Interacao interacao);

        /// <summary>
        /// Lista interações de um chamado
        /// </summary>
        Task<List<Interacao>> ListarInteracoesAsync(int chamadoId);

        /// <summary>
        /// Finaliza um chamado
        /// </summary>
        Task<bool> FinalizarAsync(int chamadoId, string solucao);
    }
}
