using Npgsql;
using SistemaChamados.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Repositório para operações com categorias
    /// </summary>
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public CategoriaRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Categoria>> ListarAtivasAsync()
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, nome, descricao, ativo, ordem
                    FROM categorias
                    WHERE ativo = true
                    ORDER BY ordem, nome";

                using var cmd = new NpgsqlCommand(query, conn);

                var categorias = new List<Categoria>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    categorias.Add(new Categoria
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Descricao = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Ativo = reader.GetBoolean(3),
                        Ordem = reader.GetInt32(4)
                    });
                }

                return categorias;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar categorias ativas: {ex.Message}", ex);
            }
        }

        public async Task<List<Categoria>> ListarTodasAsync()
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, nome, descricao, ativo, ordem
                    FROM categorias
                    ORDER BY ordem, nome";

                using var cmd = new NpgsqlCommand(query, conn);

                var categorias = new List<Categoria>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    categorias.Add(new Categoria
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Descricao = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Ativo = reader.GetBoolean(3),
                        Ordem = reader.GetInt32(4)
                    });
                }

                return categorias;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar todas as categorias: {ex.Message}", ex);
            }
        }
    }
}
