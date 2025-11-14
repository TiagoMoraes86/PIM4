using Npgsql;
using SistemaChamados.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Repositório para operações com logs LGPD
    /// </summary>
    public class LogLGPDRepository : ILogLGPDRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public LogLGPDRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> RegistrarAsync(LogLGPD log)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    INSERT INTO logs_lgpd (usuario_email, acao, tabela, registro_id, 
                                          dados_acessados, ip_address, user_agent, timestamp)
                    VALUES (@usuario_email, @acao, @tabela, @registro_id, 
                            @dados_acessados, @ip_address, @user_agent, @timestamp)";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@usuario_email", log.UsuarioEmail);
                cmd.Parameters.AddWithValue("@acao", log.Acao);
                cmd.Parameters.AddWithValue("@tabela", log.Tabela);
                cmd.Parameters.AddWithValue("@registro_id", (object?)log.RegistroId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dados_acessados", (object?)log.DadosAcessados ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ip_address", (object?)log.IpAddress ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@user_agent", (object?)log.UserAgent ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);

                var result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Erro ao registrar log LGPD: {ex.Message}");
                return false;
            }
        }

        public async Task<List<LogLGPD>> ListarPorUsuarioAsync(string email, int limite = 100)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, usuario_email, acao, tabela, registro_id, dados_acessados, 
                           ip_address, user_agent, timestamp
                    FROM logs_lgpd
                    WHERE usuario_email = @email
                    ORDER BY timestamp DESC
                    LIMIT @limite";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@limite", limite);

                var logs = new List<LogLGPD>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    logs.Add(new LogLGPD
                    {
                        Id = reader.GetInt32(0),
                        UsuarioEmail = reader.GetString(1),
                        Acao = reader.GetString(2),
                        Tabela = reader.GetString(3),
                        RegistroId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        DadosAcessados = reader.IsDBNull(5) ? null : reader.GetString(5),
                        IpAddress = reader.IsDBNull(6) ? null : reader.GetString(6),
                        UserAgent = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Timestamp = reader.GetDateTime(8)
                    });
                }

                return logs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar logs: {ex.Message}", ex);
            }
        }

        public async Task<List<LogLGPD>> ListarTodosAsync(int limite = 100)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, usuario_email, acao, tabela, registro_id, dados_acessados, 
                           ip_address, user_agent, timestamp
                    FROM logs_lgpd
                    ORDER BY timestamp DESC
                    LIMIT @limite";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@limite", limite);

                var logs = new List<LogLGPD>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    logs.Add(new LogLGPD
                    {
                        Id = reader.GetInt32(0),
                        UsuarioEmail = reader.GetString(1),
                        Acao = reader.GetString(2),
                        Tabela = reader.GetString(3),
                        RegistroId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        DadosAcessados = reader.IsDBNull(5) ? null : reader.GetString(5),
                        IpAddress = reader.IsDBNull(6) ? null : reader.GetString(6),
                        UserAgent = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Timestamp = reader.GetDateTime(8)
                    });
                }

                return logs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar todos os logs: {ex.Message}", ex);
            }
        }
    }
}
