using SistemaChamados.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Interface para serviço de FAQ com IA
    /// </summary>
    public interface IFAQService
    {
        /// <summary>
        /// Busca FAQs relevantes baseadas em uma descrição
        /// </summary>
        Task<List<FAQ>> BuscarFAQsRelevantesAsync(string descricao, int limite = 3);

        /// <summary>
        /// Marca uma FAQ como útil (incrementa relevância)
        /// </summary>
        Task MarcarFAQComoUtilAsync(int faqId);

        /// <summary>
        /// Lista FAQs populares
        /// </summary>
        Task<List<FAQ>> ListarFAQsPopularesAsync(int limite = 10);
    }
}
