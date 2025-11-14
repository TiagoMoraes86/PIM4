using SistemaChamados.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Data.Repositories
{
    /// <summary>
    /// Interface para repositório de categorias
    /// </summary>
    public interface ICategoriaRepository
    {
        /// <summary>
        /// Lista todas as categorias ativas
        /// </summary>
        Task<List<Categoria>> ListarAtivasAsync();

        /// <summary>
        /// Lista todas as categorias
        /// </summary>
        Task<List<Categoria>> ListarTodasAsync();
    }
}
