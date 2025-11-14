using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Interface para serviço de conformidade LGPD
    /// </summary>
    public interface ILGPDService
    {
        /// <summary>
        /// Registra um acesso a dados pessoais
        /// </summary>
        Task RegistrarAcessoAsync(string usuarioEmail, string acao, string tabela, 
                                 int? registroId, string dadosAcessados, 
                                 string? ipAddress, string? userAgent);
    }
}
