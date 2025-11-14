using Npgsql;
using SistemaChamados.Model;
using System;
using System.Collections.Generic;

namespace SistemaChamados.Data
{
    public class ChamadoRepository
    {
        public Chamado CriarChamado(string titulo, string descricao, string categoria,
                                    string prioridade, string solicitante)
        {
            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                            INSERT INTO chamados 
                            (titulo, descricao, categoria, prioridade, data_abertura, solicitante, status)
                            VALUES 
                            (@titulo, @descricao, @categoria, @prioridade, @dataAbertura, @solicitante, @status)
                            RETURNING id";

                        DateTime dataAbertura = DateTime.Now;

                        cmd.Parameters.AddWithValue("@titulo", titulo);
                        cmd.Parameters.AddWithValue("@descricao", descricao);
                        cmd.Parameters.AddWithValue("@categoria", categoria);
                        cmd.Parameters.AddWithValue("@prioridade", prioridade);
                        cmd.Parameters.AddWithValue("@dataAbertura", dataAbertura);
                        cmd.Parameters.AddWithValue("@solicitante", solicitante);
                        cmd.Parameters.AddWithValue("@status", "Aguardando atribuição");

                        int id = (int)cmd.ExecuteScalar();

                        return new Chamado(id, titulo, descricao, categoria, prioridade, solicitante);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar chamado: " + ex.Message);
            }
        }

        public void AtualizarChamado(Chamado chamado)
        {
            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                            UPDATE chamados SET 
                            tecnico = @tecnico,
                            status = @status,
                            solucao = @solucao,
                            data_fechamento = @dataFechamento
                            WHERE id = @id";

                        cmd.Parameters.AddWithValue("@tecnico", chamado.Tecnico ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@status", chamado.Status);
                        cmd.Parameters.AddWithValue("@solucao", chamado.Solucao ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dataFechamento",
                            chamado.DataFechamento.HasValue ?
                            (object)chamado.DataFechamento.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", chamado.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar chamado: " + ex.Message);
            }
        }

        public List<Chamado> ObterChamadosEmAberto()
        {
            List<Chamado> chamados = new List<Chamado>();

            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                            SELECT * FROM chamados 
                            WHERE status != 'Finalizado'
                            ORDER BY data_abertura DESC";

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chamados.Add(LerChamadoDoReader(reader));
                            }
                        }
                    }
                }

                // Carregar interações para cada chamado
                foreach (Chamado chamado in chamados)
                {
                    CarregarInteracoes(chamado);
                }

                return chamados;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter chamados em aberto: " + ex.Message);
            }
        }

        public List<Chamado> ObterChamadosFinalizados()
        {
            List<Chamado> chamados = new List<Chamado>();

            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                            SELECT * FROM chamados 
                            WHERE status = 'Finalizado'
                            ORDER BY data_fechamento DESC";

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chamados.Add(LerChamadoDoReader(reader));
                            }
                        }
                    }
                }

                // Carregar interações para cada chamado
                foreach (Chamado chamado in chamados)
                {
                    CarregarInteracoes(chamado);
                }

                return chamados;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter chamados finalizados: " + ex.Message);
            }
        }

        public Chamado ObterChamadoPorId(int id)
        {
            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                            SELECT * FROM chamados 
                            WHERE id = @id";

                        cmd.Parameters.AddWithValue("@id", id);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Chamado chamado = LerChamadoDoReader(reader);
                                CarregarInteracoes(chamado);
                                return chamado;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter chamado por ID: " + ex.Message);
            }
        }

        private Chamado LerChamadoDoReader(NpgsqlDataReader reader)
        {
            int id = Convert.ToInt32(reader["id"]);
            string titulo = reader["titulo"].ToString();
            string descricao = reader["descricao"].ToString();
            string categoria = reader["categoria"].ToString();
            string prioridade = reader["prioridade"].ToString();
            string solicitante = reader["solicitante"].ToString();

            Chamado chamado = new Chamado(id, titulo, descricao, categoria, prioridade, solicitante);

            if (reader["tecnico"] != DBNull.Value)
            {
                chamado.AtribuirTecnico(reader["tecnico"].ToString());
            }

            chamado.AlterarStatus(reader["status"].ToString());

            if (reader["solucao"] != DBNull.Value)
            {
                chamado.DefinirSolucao(reader["solucao"].ToString());
            }

            if (reader["data_fechamento"] != DBNull.Value)
            {
                chamado.DefinirDataFechamento(Convert.ToDateTime(reader["data_fechamento"]));
            }

            return chamado;
        }



        public void AdicionarInteracao(int chamadoId, Interacao interacao)
        {
            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                            INSERT INTO interacoes 
                            (chamado_id, data_hora, descricao)
                            VALUES 
                            (@chamadoId, @dataHora, @descricao)";

                        cmd.Parameters.AddWithValue("@chamadoId", chamadoId);
                        cmd.Parameters.AddWithValue("@dataHora", interacao.DataHora);
                        cmd.Parameters.AddWithValue("@descricao", interacao.Descricao);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar interação: " + ex.Message);
            }
        }

        private void CarregarInteracoes(Chamado chamado)
        {
            try
            {
                using (NpgsqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = @"
                    SELECT * FROM interacoes 
                    WHERE chamado_id = @chamadoId
                    ORDER BY data_hora";

                        cmd.Parameters.AddWithValue("@chamadoId", chamado.Id);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime dataHora = Convert.ToDateTime(reader["data_hora"]);
                                string descricao = reader["descricao"].ToString();

                                // Usar o método que criamos para adicionar interações do banco
                                chamado.AdicionarInteracaoDoBanco(dataHora, descricao);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar interações: " + ex.Message);
            }
        }

    }
}
