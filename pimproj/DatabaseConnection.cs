using Npgsql;
using System;

namespace SistemaChamados.Data
{
    public class DatabaseConnection
    {
        // Verifique se estes dados correspondem à sua instalação do PostgreSQL
        private static string connectionString = "Server=localhost;Port=5432;Database=pim;User Id=postgres;Password=2004;";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
