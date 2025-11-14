using System;
using System.Collections.Generic;

namespace SistemaChamados.Web.Models
{
    /// <summary>
    /// Representa um chamado de suporte técnico no sistema
    /// </summary>
    public class Chamado
    {
        /// <summary>
        /// Identificador único do chamado
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título resumido do chamado
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do problema
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Categoria do chamado (Hardware, Software, Rede, etc.)
        /// </summary>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Prioridade do chamado (Baixa, Média, Alta, Urgente)
        /// </summary>
        public string Prioridade { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora de abertura do chamado
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Data e hora de fechamento do chamado (null se ainda aberto)
        /// </summary>
        public DateTime? DataFechamento { get; set; }

        /// <summary>
        /// Email do usuário que abriu o chamado
        /// </summary>
        public string Solicitante { get; set; } = string.Empty;

        /// <summary>
        /// Email do técnico responsável (null se não atribuído)
        /// </summary>
        public string? Tecnico { get; set; }

        /// <summary>
        /// Status atual do chamado
        /// </summary>
        public string Status { get; set; } = "Aguardando atribuição";

        /// <summary>
        /// Solução aplicada ao chamado (preenchido ao finalizar)
        /// </summary>
        public string? Solucao { get; set; }

        /// <summary>
        /// Lista de interações/comentários do chamado
        /// </summary>
        public List<Interacao> Interacoes { get; set; } = new List<Interacao>();

        /// <summary>
        /// Sugestões da IA para este chamado
        /// </summary>
        public List<SugestaoIA> Sugestoes { get; set; } = new List<SugestaoIA>();

        /// <summary>
        /// Calcula o tempo decorrido desde a abertura
        /// </summary>
        public TimeSpan TempoDecorrido
        {
            get
            {
                var dataFim = DataFechamento ?? DateTime.Now;
                return dataFim - DataCriacao;
            }
        }

        /// <summary>
        /// Retorna a classe CSS para o badge de status
        /// </summary>
        public string StatusBadgeClass
        {
            get
            {
                return Status switch
                {
                    "Aguardando atribuição" => "badge bg-warning text-dark",
                    "Aberto" => "badge bg-info",
                    "Em análise" => "badge bg-primary",
                    "Finalizado" => "badge bg-success",
                    "Cancelado" => "badge bg-secondary",
                    _ => "badge bg-secondary"
                };
            }
        }

        /// <summary>
        /// Retorna a classe CSS para o badge de prioridade
        /// </summary>
        public string PrioridadeBadgeClass
        {
            get
            {
                return Prioridade.ToLower() switch
                {
                    "baixa" => "badge bg-secondary",
                    "média" or "media" => "badge bg-info",
                    "alta" => "badge bg-warning text-dark",
                    "urgente" => "badge bg-danger",
                    _ => "badge bg-secondary"
                };
            }
        }
    }
}
