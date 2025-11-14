using Npgsql;
using SistemaChamados.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Repositório para operações com chamados
    /// </summary>
    public class ChamadoRepository : IChamadoRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public ChamadoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CriarAsync(Chamado chamado)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    INSERT INTO chamados (titulo, descricao, categoria, prioridade, data_abertura, 
                                        solicitante, status)
                    VALUES (@titulo, @descricao, @categoria, @prioridade, @data_abertura, 
                            @solicitante, @status)
                    RETURNING id";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@titulo", chamado.Titulo);
                cmd.Parameters.AddWithValue("@descricao", chamado.Descricao);
                cmd.Parameters.AddWithValue("@categoria", chamado.Categoria);
                cmd.Parameters.AddWithValue("@prioridade", chamado.Prioridade);
                cmd.Parameters.AddWithValue("@data_abertura", DateTime.Now);
                cmd.Parameters.AddWithValue("@solicitante", chamado.Solicitante);
                cmd.Parameters.AddWithValue("@status", "Aguardando atribuição");

                var id = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar chamado: {ex.Message}", ex);
            }
        }

        public async Task<Chamado?> ObterPorIdAsync(int id)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, titulo, descricao, categoria, prioridade, data_abertura, 
                           data_fechamento, solicitante, tecnico, status, solucao
                    FROM chamados
                    WHERE id = @id";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var chamado = new Chamado
                    {
                        Id = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Descricao = reader.GetString(2),
                        Categoria = reader.GetString(3),
                        Prioridade = reader.GetString(4),
                        DataCriacao = reader.GetDateTime(5),
                        DataFechamento = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                        Solicitante = reader.GetString(7),
                        Tecnico = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Status = reader.GetString(9),
                        Solucao = reader.IsDBNull(10) ? null : reader.GetString(10)
                    };

                    return chamado;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar chamado: {ex.Message}", ex);
            }
        }

        public async Task<List<Chamado>> ListarPorUsuarioAsync(string emailUsuario, string? filtroStatus = null)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, titulo, descricao, categoria, prioridade, data_abertura, 
                           data_fechamento, solicitante, tecnico, status, solucao
                    FROM chamados
                    WHERE solicitante = @email";

                if (!string.IsNullOrEmpty(filtroStatus))
                {
                    query += " AND status = @status";
                }

                query += " ORDER BY data_abertura DESC";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", emailUsuario);
                
                if (!string.IsNullOrEmpty(filtroStatus))
                {
                    cmd.Parameters.AddWithValue("@status", filtroStatus);
                }

                var chamados = new List<Chamado>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    chamados.Add(new Chamado
                    {
                        Id = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Descricao = reader.GetString(2),
                        Categoria = reader.GetString(3),
                        Prioridade = reader.GetString(4),
                        DataCriacao = reader.GetDateTime(5),
                        DataFechamento = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                        Solicitante = reader.GetString(7),
                        Tecnico = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Status = reader.GetString(9),
                        Solucao = reader.IsDBNull(10) ? null : reader.GetString(10)
                    });
                }

                return chamados;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar chamados: {ex.Message}", ex);
            }
        }

        public async Task<List<Chamado>> ListarTodosAsync(string? filtroStatus = null)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, titulo, descricao, categoria, prioridade, data_abertura, 
                           data_fechamento, solicitante, tecnico, status, solucao
                    FROM chamados";

                if (!string.IsNullOrEmpty(filtroStatus))
                {
                    query += " WHERE status = @status";
                }

                query += " ORDER BY data_abertura DESC";

                using var cmd = new NpgsqlCommand(query, conn);
                
                if (!string.IsNullOrEmpty(filtroStatus))
                {
                    cmd.Parameters.AddWithValue("@status", filtroStatus);
                }

                var chamados = new List<Chamado>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    chamados.Add(new Chamado
                    {
                        Id = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Descricao = reader.GetString(2),
                        Categoria = reader.GetString(3),
                        Prioridade = reader.GetString(4),
                        DataCriacao = reader.GetDateTime(5),
                        DataFechamento = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                        Solicitante = reader.GetString(7),
                        Tecnico = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Status = reader.GetString(9),
                        Solucao = reader.IsDBNull(10) ? null : reader.GetString(10)
                    });
                }

                return chamados;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar todos os chamados: {ex.Message}", ex);
            }
        }

        public async Task<bool> AtualizarAsync(Chamado chamado)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    UPDATE chamados
                    SET titulo = @titulo, descricao = @descricao, categoria = @categoria,
                        prioridade = @prioridade, tecnico = @tecnico, status = @status,
                        solucao = @solucao, data_fechamento = @data_fechamento
                    WHERE id = @id";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", chamado.Id);
                cmd.Parameters.AddWithValue("@titulo", chamado.Titulo);
                cmd.Parameters.AddWithValue("@descricao", chamado.Descricao);
                cmd.Parameters.AddWithValue("@categoria", chamado.Categoria);
                cmd.Parameters.AddWithValue("@prioridade", chamado.Prioridade);
                cmd.Parameters.AddWithValue("@tecnico", (object?)chamado.Tecnico ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", chamado.Status);
                cmd.Parameters.AddWithValue("@solucao", (object?)chamado.Solucao ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@data_fechamento", (object?)chamado.DataFechamento ?? DBNull.Value);

                var result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar chamado: {ex.Message}", ex);
            }
        }

        public async Task<bool> AdicionarInteracaoAsync(int chamadoId, Interacao interacao)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    INSERT INTO interacoes (chamado_id, data_hora, descricao, usuario)
                    VALUES (@chamado_id, @data_hora, @descricao, @usuario)";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@chamado_id", chamadoId);
                cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                cmd.Parameters.AddWithValue("@descricao", interacao.Descricao);
                cmd.Parameters.AddWithValue("@usuario", (object?)interacao.Usuario ?? DBNull.Value);

                var result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar interação: {ex.Message}", ex);
            }
        }

        public async Task<List<Interacao>> ListarInteracoesAsync(int chamadoId)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, chamado_id, data_hora, descricao, usuario
                    FROM interacoes
                    WHERE chamado_id = @chamado_id
                    ORDER BY data_hora ASC";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@chamado_id", chamadoId);

                var interacoes = new List<Interacao>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    interacoes.Add(new Interacao
                    {
                        Id = reader.GetInt32(0),
                        ChamadoId = reader.GetInt32(1),
                        DataHora = reader.GetDateTime(2),
                        Descricao = reader.GetString(3),
                        Usuario = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }

                return interacoes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar interações: {ex.Message}", ex);
            }
        }

        public async Task<bool> FinalizarAsync(int chamadoId, string solucao)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    UPDATE chamados
                    SET status = 'Finalizado', solucao = @solucao, data_fechamento = @data_fechamento
                    WHERE id = @id";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", chamadoId);
                cmd.Parameters.AddWithValue("@solucao", solucao);
                cmd.Parameters.AddWithValue("@data_fechamento", DateTime.Now);

                var result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao finalizar chamado: {ex.Message}", ex);
            }
        }
    }
}
