using SistemaChamados.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Interface para repositório de FAQs
    /// </summary>
    public interface IFAQRepository
    {
        /// <summary>
        /// Busca FAQs por palavras-chave
        /// </summary>
        Task<List<FAQ>> BuscarPorPalavrasChaveAsync(string[] palavrasChave, int limite = 3);

        /// <summary>
        /// Busca FAQs por categoria
        /// </summary>
        Task<List<FAQ>> BuscarPorCategoriaAsync(string categoria);

        /// <summary>
        /// Incrementa a relevância de uma FAQ
        /// </summary>
        Task<bool> IncrementarRelevanciaAsync(int faqId);

        /// <summary>
        /// Lista as FAQs mais populares
        /// </summary>
        Task<List<FAQ>> ListarPopularesAsync(int limite = 10);

        /// <summary>
        /// Cria uma nova FAQ
        /// </summary>
        Task<int> CriarAsync(FAQ faq);
    }
}
