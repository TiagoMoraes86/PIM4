using System;

namespace SistemaChamados.Web.Models
{
    /// <summary>
    /// Representa um log de acesso para conformidade com LGPD
    /// </summary>
    public class LogLGPD
    {
        /// <summary>
        /// Identificador único do log
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Email do usuário que realizou a ação
        /// </summary>
        public string UsuarioEmail { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de ação realizada (acesso, modificacao, exclusao, exportacao)
        /// </summary>
        public string Acao { get; set; } = string.Empty;

        /// <summary>
        /// Nome da tabela acessada
        /// </summary>
        public string Tabela { get; set; } = string.Empty;

        /// <summary>
        /// ID do registro acessado
        /// </summary>
        public int? RegistroId { get; set; }

        /// <summary>
        /// JSON com os campos/dados acessados
        /// </summary>
        public string? DadosAcessados { get; set; }

        /// <summary>
        /// Endereço IP do usuário
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User Agent do navegador
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Data e hora do acesso
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
