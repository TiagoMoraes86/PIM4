using Npgsql;
using System;

namespace SistemaChamados.Web.Data
{
    /// <summary>
    /// Gerencia a conexão com o banco de dados PostgreSQL
    /// </summary>
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Retorna uma nova conexão com o banco de dados
        /// </summary>
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        /// <summary>
        /// Testa a conexão com o banco de dados
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Executa o script de migrations
        /// </summary>
        public void ExecuteMigrations(string scriptPath)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();

                var script = System.IO.File.ReadAllText(scriptPath);
                using var cmd = new NpgsqlCommand(script, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao executar migrations: {ex.Message}", ex);
            }
        }
    }
}
