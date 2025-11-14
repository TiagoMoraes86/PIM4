namespace SistemaChamados.Web.Models
{
    /// <summary>
    /// Representa uma categoria de chamado
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Identificador único da categoria
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da categoria
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Indica se a categoria está ativa
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Ordem de exibição
        /// </summary>
        public int Ordem { get; set; }
    }
}
