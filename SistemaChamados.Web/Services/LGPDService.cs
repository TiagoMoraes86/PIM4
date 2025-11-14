using SistemaChamados.Web.Data.Repositories;
using SistemaChamados.Web.Models;
using System;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Serviço para conformidade com LGPD
    /// </summary>
    public class LGPDService : ILGPDService
    {
        private readonly ILogLGPDRepository _logRepository;

        public LGPDService(ILogLGPDRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task RegistrarAcessoAsync(string usuarioEmail, string acao, string tabela, 
                                              int? registroId, string dadosAcessados, 
                                              string? ipAddress, string? userAgent)
        {
            try
            {
                var log = new LogLGPD
                {
                    UsuarioEmail = usuarioEmail,
                    Acao = acao,
                    Tabela = tabela,
                    RegistroId = registroId,
                    DadosAcessados = dadosAcessados,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    Timestamp = DateTime.Now
                };

                await _logRepository.RegistrarAsync(log);
            }
            catch (Exception ex)
            {
                // Não quebrar o fluxo principal se falhar o log
                Console.WriteLine($"Erro ao registrar log LGPD: {ex.Message}");
            }
        }
    }
}
