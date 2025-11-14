using SistemaChamados.Web.Data.Repositories;
using SistemaChamados.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Serviço para gerenciamento de FAQs com busca inteligente
    /// </summary>
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _faqRepository;

        // Palavras comuns a serem ignoradas (stop words em português)
        private static readonly HashSet<string> StopWords = new HashSet<string>
        {
            "o", "a", "os", "as", "um", "uma", "uns", "umas",
            "de", "do", "da", "dos", "das", "em", "no", "na", "nos", "nas",
            "por", "para", "com", "sem", "sob", "sobre",
            "e", "ou", "mas", "porém", "contudo",
            "que", "qual", "quais", "como", "quando", "onde",
            "é", "são", "está", "estão", "foi", "foram",
            "ter", "tem", "tinha", "tenho",
            "ser", "sou", "era", "foi",
            "fazer", "faz", "fez", "faço",
            "meu", "minha", "seu", "sua", "nosso", "nossa"
        };

        public FAQService(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task<List<FAQ>> BuscarFAQsRelevantesAsync(string descricao, int limite = 3)
        {
            try
            {
                // Extrair palavras-chave da descrição
                var palavrasChave = ExtrairPalavrasChave(descricao);

                if (palavrasChave.Length == 0)
                {
                    // Se não houver palavras-chave, retornar FAQs populares
                    return await _faqRepository.ListarPopularesAsync(limite);
                }

                // Buscar FAQs que contenham as palavras-chave
                var faqs = await _faqRepository.BuscarPorPalavrasChaveAsync(palavrasChave, limite * 3);

                if (faqs.Count == 0)
                {
                    // Se não encontrou nada, retornar FAQs populares
                    return await _faqRepository.ListarPopularesAsync(limite);
                }

                // Calcular score de similaridade para cada FAQ
                foreach (var faq in faqs)
                {
                    faq.ScoreSimilaridade = CalcularSimilaridade(palavrasChave, faq.PalavrasChave);
                }

                // Ordenar por score e retornar top N
                return faqs
                    .OrderByDescending(f => f.ScoreSimilaridade)
                    .ThenByDescending(f => f.Relevancia)
                    .Take(limite)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar FAQs relevantes: {ex.Message}", ex);
            }
        }

        public async Task MarcarFAQComoUtilAsync(int faqId)
        {
            try
            {
                await _faqRepository.IncrementarRelevanciaAsync(faqId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao marcar FAQ como útil: {ex.Message}", ex);
            }
        }

        public async Task<List<FAQ>> ListarFAQsPopularesAsync(int limite = 10)
        {
            try
            {
                return await _faqRepository.ListarPopularesAsync(limite);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar FAQs populares: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Extrai palavras-chave relevantes de um texto
        /// </summary>
        private string[] ExtrairPalavrasChave(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return Array.Empty<string>();

            // Converter para minúsculas e remover pontuação
            texto = texto.ToLowerInvariant();
            texto = Regex.Replace(texto, @"[^\w\s]", " ");

            // Dividir em palavras
            var palavras = texto.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Filtrar stop words e palavras muito curtas
            var palavrasChave = palavras
                .Where(p => p.Length >= 3 && !StopWords.Contains(p))
                .Distinct()
                .ToArray();

            return palavrasChave;
        }

        /// <summary>
        /// Calcula a similaridade entre duas listas de palavras-chave
        /// Retorna um score de 0 a 1
        /// </summary>
        private double CalcularSimilaridade(string[] palavras1, string[] palavras2)
        {
            if (palavras1.Length == 0 || palavras2.Length == 0)
                return 0;

            // Converter para conjuntos para facilitar operações
            var set1 = new HashSet<string>(palavras1.Select(p => p.ToLowerInvariant()));
            var set2 = new HashSet<string>(palavras2.Select(p => p.ToLowerInvariant()));

            // Calcular interseção e união
            var intersecao = set1.Intersect(set2).Count();
            var uniao = set1.Union(set2).Count();

            // Coeficiente de Jaccard: |A ∩ B| / |A ∪ B|
            if (uniao == 0)
                return 0;

            return (double)intersecao / uniao;
        }
    }
}
