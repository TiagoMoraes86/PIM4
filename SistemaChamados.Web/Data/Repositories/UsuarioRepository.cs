using Npgsql;
using SistemaChamados.Web.Models;
using System;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Repositório para operações com usuários
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public UsuarioRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT email, senha, nome, tipo, ativo, criado_em
                    FROM usuarios
                    WHERE email = @email";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Usuario
                    {
                        Email = reader.GetString(0),
                        Senha = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Tipo = reader.GetString(3),
                        Ativo = reader.GetBoolean(4),
                        CriadoEm = reader.GetDateTime(5)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário: {ex.Message}", ex);
            }
        }

        public async Task<bool> CriarAsync(Usuario usuario)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    INSERT INTO usuarios (email, senha, nome, tipo, ativo, criado_em)
                    VALUES (@email, @senha, @nome, @tipo, @ativo, @criado_em)";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", usuario.Email);
                cmd.Parameters.AddWithValue("@senha", usuario.Senha);
                cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@tipo", usuario.Tipo);
                cmd.Parameters.AddWithValue("@ativo", usuario.Ativo);
                cmd.Parameters.AddWithValue("@criado_em", DateTime.Now);

                var result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar usuário: {ex.Message}", ex);
            }
        }

        public async Task<bool> AtualizarAsync(Usuario usuario)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    UPDATE usuarios
                    SET senha = @senha, nome = @nome, tipo = @tipo, ativo = @ativo
                    WHERE email = @email";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", usuario.Email);
                cmd.Parameters.AddWithValue("@senha", usuario.Senha);
                cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                cmd.Parameters.AddWithValue("@tipo", usuario.Tipo);
                cmd.Parameters.AddWithValue("@ativo", usuario.Ativo);

                var result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar usuário: {ex.Message}", ex);
            }
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = "SELECT COUNT(*) FROM usuarios WHERE email = @email";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);

                var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar email: {ex.Message}", ex);
            }
        }
    }
}
