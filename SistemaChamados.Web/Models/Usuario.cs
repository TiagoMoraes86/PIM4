using System;

namespace SistemaChamados.Web.Models
{
    /// <summary>
    /// Representa um usuário do sistema
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Email do usuário (chave primária)
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash da senha do usuário
        /// </summary>
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de usuário (comum, tecnico, admin)
        /// </summary>
        public string Tipo { get; set; } = "comum";

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Data de criação do usuário
        /// </summary>
        public DateTime CriadoEm { get; set; }

        /// <summary>
        /// Verifica se o usuário é administrador
        /// </summary>
        public bool IsAdmin => Tipo.ToLower() == "admin";

        /// <summary>
        /// Verifica se o usuário é técnico
        /// </summary>
        public bool IsTecnico => Tipo.ToLower() == "tecnico" || IsAdmin;

        /// <summary>
        /// Verifica se o usuário é comum
        /// </summary>
        public bool IsComum => Tipo.ToLower() == "comum";
    }
}
