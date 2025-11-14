using Npgsql;
using SistemaChamados.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Repositório para geração de relatórios e estatísticas
    /// </summary>
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public RelatorioRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<DashboardViewModel> ObterDadosDashboardAsync()
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT total_chamados, chamados_abertos, chamados_em_analise, 
                           chamados_finalizados, finalizados_mes, tempo_medio_horas
                    FROM vw_dashboard_resumo";

                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new DashboardViewModel
                    {
                        TotalChamados = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader.GetInt64(0)),
                        ChamadosAbertos = reader.IsDBNull(1) ? 0 : Convert.ToInt32(reader.GetInt64(1)),
                        ChamadosEmAnalise = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetInt64(2)),
                        ChamadosFinalizados = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetInt64(3)),
                        FinalizadosMes = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetInt64(4)),
                        TempoMedioResolucaoHoras = reader.IsDBNull(5) ? 0 : Convert.ToDouble(reader.GetValue(5))
                    };
                }

                return new DashboardViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter dados do dashboard: {ex.Message}", ex);
            }
        }

        public async Task<List<ChartDataItem>> ObterChamadosPorStatusAsync()
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT status, quantidade, percentual
                    FROM vw_chamados_por_status";

                using var cmd = new NpgsqlCommand(query, conn);

                var dados = new List<ChartDataItem>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    dados.Add(new ChartDataItem
                    {
                        Label = reader.GetString(0),
                        Valor = Convert.ToInt32(reader.GetInt64(1)),
                        Percentual = reader.GetDecimal(2)
                    });
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter chamados por status: {ex.Message}", ex);
            }
        }

        public async Task<List<ChartDataMensal>> ObterChamadosPorMesAsync(int meses = 6)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT mes, mes_formatado, quantidade, finalizados
                    FROM vw_chamados_por_mes
                    LIMIT @meses";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@meses", meses);

                var dados = new List<ChartDataMensal>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    dados.Add(new ChartDataMensal
                    {
                        Mes = reader.GetString(0),
                        MesFormatado = reader.GetString(1),
                        Quantidade = Convert.ToInt32(reader.GetInt64(2)),
                        Finalizados = Convert.ToInt32(reader.GetInt64(3))
                    });
                }

                // Inverter para ordem cronológica
                dados.Reverse();
                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter chamados por mês: {ex.Message}", ex);
            }
        }
    }
}
