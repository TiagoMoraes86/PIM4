using Npgsql;
using System;

namespace SistemaChamados.Data
{
    public class DatabaseInitializer
    {
        public static bool VerificarConexaoETabelas()
        {
            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    // Verificar se as tabelas existem
                    if (!TabelaExiste(conn, "usuarios") ||
                        !TabelaExiste(conn, "chamados") ||
                        !TabelaExiste(conn, "interacoes"))
                    {
                        throw new Exception("Uma ou mais tabelas necessárias não foram encontradas no banco de dados.");
                    }

                    Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso e todas as tabelas existem.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar o banco de dados: {ex.Message}");
                throw;
            }
        }

        private static bool TabelaExiste(NpgsqlConnection conn, string nomeTabela)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = @"
                    SELECT EXISTS (
                        SELECT FROM information_schema.tables 
                        WHERE table_schema = 'public' 
                        AND table_name = @nomeTabela
                    );";

                cmd.Parameters.AddWithValue("@nomeTabela", nomeTabela);

                return (bool)cmd.ExecuteScalar();
            }
        }
    }
}
