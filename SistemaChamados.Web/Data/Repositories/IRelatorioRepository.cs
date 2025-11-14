using SistemaChamados.Web.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Interface para repositório de relatórios e estatísticas
    /// </summary>
    public interface IRelatorioRepository
    {
        /// <summary>
        /// Obtém dados resumidos para o dashboard
        /// </summary>
        Task<DashboardViewModel> ObterDadosDashboardAsync();

        /// <summary>
        /// Obtém dados de chamados por status
        /// </summary>
        Task<List<ChartDataItem>> ObterChamadosPorStatusAsync();

        /// <summary>
        /// Obtém dados de chamados por mês
        /// </summary>
        Task<List<ChartDataMensal>> ObterChamadosPorMesAsync(int meses = 6);
    }
}
