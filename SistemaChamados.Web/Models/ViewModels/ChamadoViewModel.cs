using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaChamados.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel para exibição resumida de um chamado (usado no Dashboard e listagens)
    /// </summary>
    public class ChamadoViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Solicitante { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel para criação de novo chamado
    /// </summary>
    public class NovoChamadoViewModel
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(255, ErrorMessage = "O título deve ter no máximo 255 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(5000, ErrorMessage = "A descrição deve ter no máximo 5000 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "A prioridade é obrigatória")]
        public string Prioridade { get; set; } = string.Empty;

        /// <summary>
        /// Lista de categorias disponíveis
        /// </summary>
        public List<Categoria> Categorias { get; set; } = new List<Categoria>();

        /// <summary>
        /// FAQs sugeridas baseadas na descrição
        /// </summary>
        public List<FAQ> FAQsSugeridas { get; set; } = new List<FAQ>();
    }

    /// <summary>
    /// ViewModel para listagem de chamados
    /// </summary>
    public class MeusChamadosViewModel
    {
        /// <summary>
        /// Lista de chamados do usuário
        /// </summary>
        public List<Chamado> Chamados { get; set; } = new List<Chamado>();

        /// <summary>
        /// Filtro de status selecionado
        /// </summary>
        public string? FiltroStatus { get; set; }

        /// <summary>
        /// Página atual
        /// </summary>
        public int PaginaAtual { get; set; } = 1;

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPaginas { get; set; }

        /// <summary>
        /// Total de chamados
        /// </summary>
        public int TotalChamados { get; set; }
    }

    /// <summary>
    /// ViewModel para detalhes do chamado
    /// </summary>
    public class DetalhesChamadoViewModel
    {
        /// <summary>
        /// Chamado completo
        /// </summary>
        public Chamado Chamado { get; set; } = new Chamado();

        /// <summary>
        /// Novo comentário a ser adicionado
        /// </summary>
        [StringLength(2000, ErrorMessage = "O comentário deve ter no máximo 2000 caracteres")]
        public string? NovoComentario { get; set; }

        /// <summary>
        /// Indica se o usuário pode editar o chamado
        /// </summary>
        public bool PodeEditar { get; set; }

        /// <summary>
        /// Indica se o usuário pode finalizar o chamado
        /// </summary>
        public bool PodeFinalizar { get; set; }
    }
}
