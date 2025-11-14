using System;

namespace SistemaChamados.Web.Models
{
    /// <summary>
    /// Representa uma sugestão gerada pela IA para resolução de chamado
    /// </summary>
    public class SugestaoIA
    {
        /// <summary>
        /// Identificador único da sugestão
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID do chamado relacionado
        /// </summary>
        public int ChamadoId { get; set; }

        /// <summary>
        /// Texto da sugestão
        /// </summary>
        public string Sugestao { get; set; } = string.Empty;

        /// <summary>
        /// Nível de confiança da sugestão (0-100)
        /// </summary>
        public decimal Confianca { get; set; }

        /// <summary>
        /// Indica se a sugestão foi aceita pelo usuário
        /// </summary>
        public bool Aceita { get; set; }

        /// <summary>
        /// Feedback do usuário sobre a sugestão
        /// </summary>
        public string? Feedback { get; set; }

        /// <summary>
        /// Data de criação da sugestão
        /// </summary>
        public DateTime CriadoEm { get; set; }

        /// <summary>
        /// Retorna a classe CSS para o badge de confiança
        /// </summary>
        public string ConfiancaBadgeClass
        {
            get
            {
                if (Confianca >= 80) return "badge bg-success";
                if (Confianca >= 60) return "badge bg-info";
                if (Confianca >= 40) return "badge bg-warning text-dark";
                return "badge bg-secondary";
            }
        }

        /// <summary>
        /// Retorna texto formatado da confiança
        /// </summary>
        public string ConfiancaTexto => $"{Confianca:F0}% de confiança";
    }
}
