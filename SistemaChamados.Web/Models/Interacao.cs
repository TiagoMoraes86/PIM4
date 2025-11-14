using System;

namespace SistemaChamados.Web.Models
{
    /// <summary>
    /// Representa uma interação/comentário em um chamado
    /// </summary>
    public class Interacao
    {
        /// <summary>
        /// Identificador único da interação
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID do chamado relacionado
        /// </summary>
        public int ChamadoId { get; set; }

        /// <summary>
        /// Data e hora da interação
        /// </summary>
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Descrição/conteúdo da interação
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Email do usuário que fez a interação (pode ser null para interações automáticas)
        /// </summary>
        public string? Usuario { get; set; }

        /// <summary>
        /// Indica se é uma interação do sistema (automática)
        /// </summary>
        public bool IsAutomatica => string.IsNullOrEmpty(Usuario);
    }
}
