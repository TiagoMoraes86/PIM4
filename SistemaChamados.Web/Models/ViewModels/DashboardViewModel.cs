using System.Collections.Generic;

namespace SistemaChamados.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel para a página de Dashboard
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Total de chamados no sistema
        /// </summary>
        public int TotalChamados { get; set; }

        /// <summary>
        /// Número de chamados abertos
        /// </summary>
        public int ChamadosAbertos { get; set; }

        /// <summary>
        /// Número de chamados em análise
        /// </summary>
        public int ChamadosEmAnalise { get; set; }

        /// <summary>
        /// Número de chamados finalizados
        /// </summary>
        public int ChamadosFinalizados { get; set; }

        /// <summary>
        /// Número de chamados finalizados no mês atual
        /// </summary>
        public int FinalizadosMes { get; set; }

        /// <summary>
        /// Tempo médio de resolução em horas
        /// </summary>
        public double TempoMedioResolucaoHoras { get; set; }

        /// <summary>
        /// Dados para o gráfico de pizza (chamados por status)
        /// </summary>
        public List<ChartDataItem> ChamadosPorStatus { get; set; } = new List<ChartDataItem>();

        /// <summary>
        /// Dados para o gráfico de barras (chamados por mês)
        /// </summary>
        public List<ChartDataMensal> ChamadosPorMes { get; set; } = new List<ChartDataMensal>();

        /// <summary>
        /// Nome do usuário logado
        /// </summary>
        public string NomeUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do usuário logado
        /// </summary>
        public string TipoUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Lista de chamados recentes para exibição
        /// </summary>
        public List<ChamadoViewModel> ChamadosRecentes { get; set; } = new List<ChamadoViewModel>();
    }

    /// <summary>
    /// Item de dados para gráficos
    /// </summary>
    public class ChartDataItem
    {
        public string Label { get; set; } = string.Empty;
        public int Valor { get; set; }
        public decimal Percentual { get; set; }
    }

    /// <summary>
    /// Dados mensais para gráfico de barras
    /// </summary>
    public class ChartDataMensal
    {
        public string Mes { get; set; } = string.Empty;
        public string MesFormatado { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public int Finalizados { get; set; }
    }
}
