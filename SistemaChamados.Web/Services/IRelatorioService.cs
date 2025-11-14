using SistemaChamados.Web.Models.ViewModels;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Interface para serviço de relatórios
    /// </summary>
    public interface IRelatorioService
    {
        /// <summary>
        /// Obtém dados completos para o dashboard
        /// </summary>
        Task<DashboardViewModel> ObterDadosDashboardAsync(string nomeUsuario, string tipoUsuario);

        /// <summary>
        /// Obtém os chamados mais recentes
        /// </summary>
        Task<List<ChamadoViewModel>> ObterChamadosRecentesAsync(int limite = 8);
    }
}
