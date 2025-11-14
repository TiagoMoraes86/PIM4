using System;

namespace SistemaChamados.Web.Models
{
    /// <summary>
    /// Representa uma pergunta frequente (FAQ) do sistema
    /// </summary>
    public class FAQ
    {
        /// <summary>
        /// Identificador único da FAQ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Pergunta
        /// </summary>
        public string Pergunta { get; set; } = string.Empty;

        /// <summary>
        /// Resposta/solução
        /// </summary>
        public string Resposta { get; set; } = string.Empty;

        /// <summary>
        /// Categoria da FAQ
        /// </summary>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Contador de relevância (quantas vezes foi útil)
        /// </summary>
        public int Relevancia { get; set; }

        /// <summary>
        /// Palavras-chave para busca
        /// </summary>
        public string[] PalavrasChave { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CriadoEm { get; set; }

        /// <summary>
        /// Data da última atualização
        /// </summary>
        public DateTime AtualizadoEm { get; set; }

        /// <summary>
        /// Score de similaridade (usado em buscas)
        /// </summary>
        public double ScoreSimilaridade { get; set; }
    }
}
