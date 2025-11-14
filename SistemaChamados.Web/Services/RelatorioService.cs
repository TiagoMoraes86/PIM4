using SistemaChamados.Web.Data.Repositories;
using SistemaChamados.Web.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Serviço para geração de relatórios e estatísticas
    /// </summary>
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _relatorioRepository;

        private readonly IChamadoRepository _chamadoRepository;

        public RelatorioService(IRelatorioRepository relatorioRepository, IChamadoRepository chamadoRepository)
        {
            _relatorioRepository = relatorioRepository;
            _chamadoRepository = chamadoRepository;
        }

        public async Task<DashboardViewModel> ObterDadosDashboardAsync(string nomeUsuario, string tipoUsuario)
        {
            try
            {
                // Obter dados resumidos
                var dashboard = await _relatorioRepository.ObterDadosDashboardAsync();

                // Obter dados para gráficos
                dashboard.ChamadosPorStatus = await _relatorioRepository.ObterChamadosPorStatusAsync();
                dashboard.ChamadosPorMes = await _relatorioRepository.ObterChamadosPorMesAsync(6);

                // Adicionar informações do usuário
                dashboard.NomeUsuario = nomeUsuario;
                dashboard.TipoUsuario = tipoUsuario;

                // Obter chamados recentes
                dashboard.ChamadosRecentes = await ObterChamadosRecentesAsync(8);

                return dashboard;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter dados do dashboard: {ex.Message}", ex);
            }
        }

        public async Task<List<ChamadoViewModel>> ObterChamadosRecentesAsync(int limite = 8)
        {
            var chamados = await _chamadoRepository.ListarTodosAsync();
            var recentes = chamados.OrderByDescending(c => c.DataCriacao).Take(limite).ToList();

            var viewModels = new List<ChamadoViewModel>();
            foreach (var c in recentes)
            {
                viewModels.Add(new ChamadoViewModel
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Status = c.Status,
                    Solicitante = c.Solicitante
                });
            }

            return viewModels;
        }
    }
}
