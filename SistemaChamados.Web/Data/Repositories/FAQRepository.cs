using Npgsql;
using SistemaChamados.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Repositório para operações com FAQs
    /// </summary>
    public class FAQRepository : IFAQRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public FAQRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<FAQ>> BuscarPorPalavrasChaveAsync(string[] palavrasChave, int limite = 3)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                // Busca FAQs que contenham qualquer uma das palavras-chave
                var query = @"
                    SELECT id, pergunta, resposta, categoria, relevancia, palavras_chave, 
                           criado_em, atualizado_em
                    FROM faqs
                    WHERE palavras_chave && @palavras_chave
                    ORDER BY relevancia DESC, id DESC
                    LIMIT @limite";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@palavras_chave", palavrasChave);
                cmd.Parameters.AddWithValue("@limite", limite);

                var faqs = new List<FAQ>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    faqs.Add(new FAQ
                    {
                        Id = reader.GetInt32(0),
                        Pergunta = reader.GetString(1),
                        Resposta = reader.GetString(2),
                        Categoria = reader.GetString(3),
                        Relevancia = reader.GetInt32(4),
                        PalavrasChave = (string[])reader.GetValue(5),
                        CriadoEm = reader.GetDateTime(6),
                        AtualizadoEm = reader.GetDateTime(7)
                    });
                }

                return faqs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar FAQs: {ex.Message}", ex);
            }
        }

        public async Task<List<FAQ>> BuscarPorCategoriaAsync(string categoria)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, pergunta, resposta, categoria, relevancia, palavras_chave, 
                           criado_em, atualizado_em
                    FROM faqs
                    WHERE categoria = @categoria
                    ORDER BY relevancia DESC, id DESC";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@categoria", categoria);

                var faqs = new List<FAQ>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    faqs.Add(new FAQ
                    {
                        Id = reader.GetInt32(0),
                        Pergunta = reader.GetString(1),
                        Resposta = reader.GetString(2),
                        Categoria = reader.GetString(3),
                        Relevancia = reader.GetInt32(4),
                        PalavrasChave = (string[])reader.GetValue(5),
                        CriadoEm = reader.GetDateTime(6),
                        AtualizadoEm = reader.GetDateTime(7)
                    });
                }

                return faqs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar FAQs por categoria: {ex.Message}", ex);
            }
        }

        public async Task<bool> IncrementarRelevanciaAsync(int faqId)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = "SELECT incrementar_relevancia_faq(@faq_id)";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@faq_id", faqId);

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao incrementar relevância: {ex.Message}", ex);
            }
        }

        public async Task<List<FAQ>> ListarPopularesAsync(int limite = 10)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    SELECT id, pergunta, resposta, categoria, relevancia, palavras_chave
                    FROM vw_faqs_populares
                    LIMIT @limite";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@limite", limite);

                var faqs = new List<FAQ>();
                using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    faqs.Add(new FAQ
                    {
                        Id = reader.GetInt32(0),
                        Pergunta = reader.GetString(1),
                        Resposta = reader.GetString(2),
                        Categoria = reader.GetString(3),
                        Relevancia = reader.GetInt32(4),
                        PalavrasChave = (string[])reader.GetValue(5)
                    });
                }

                return faqs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar FAQs populares: {ex.Message}", ex);
            }
        }

        public async Task<int> CriarAsync(FAQ faq)
        {
            try
            {
                using var conn = _dbConnection.GetConnection();
                await conn.OpenAsync();

                var query = @"
                    INSERT INTO faqs (pergunta, resposta, categoria, palavras_chave, relevancia)
                    VALUES (@pergunta, @resposta, @categoria, @palavras_chave, @relevancia)
                    RETURNING id";

                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@pergunta", faq.Pergunta);
                cmd.Parameters.AddWithValue("@resposta", faq.Resposta);
                cmd.Parameters.AddWithValue("@categoria", faq.Categoria);
                cmd.Parameters.AddWithValue("@palavras_chave", faq.PalavrasChave);
                cmd.Parameters.AddWithValue("@relevancia", faq.Relevancia);

                var id = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar FAQ: {ex.Message}", ex);
            }
        }
    }
}
