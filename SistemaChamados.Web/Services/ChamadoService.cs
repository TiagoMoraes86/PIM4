using SistemaChamados.Web.Data.Repositories;
using SistemaChamados.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Serviço para gerenciamento de chamados
    /// </summary>
    public class ChamadoService : IChamadoService
    {
        private readonly IChamadoRepository _chamadoRepository;
        private readonly ILGPDService _lgpdService;

        public ChamadoService(IChamadoRepository chamadoRepository, ILGPDService lgpdService)
        {
            _chamadoRepository = chamadoRepository;
            _lgpdService = lgpdService;
        }

        public async Task<int> CriarChamadoAsync(Chamado chamado, string usuarioEmail)
        {
            try
            {
                chamado.Solicitante = usuarioEmail;
                chamado.DataCriacao = DateTime.Now;
                chamado.Status = "Aguardando atribuição";

                var chamadoId = await _chamadoRepository.CriarAsync(chamado);

                // Registrar log LGPD
                await _lgpdService.RegistrarAcessoAsync(
                    usuarioEmail,
                    "criacao",
                    "chamados",
                    chamadoId,
                    "titulo, descricao, categoria, prioridade",
                    null,
                    null
                );

                // Adicionar interação inicial
                var interacao = new Interacao
                {
                    ChamadoId = chamadoId,
                    DataHora = DateTime.Now,
                    Descricao = "Chamado criado",
                    Usuario = null // Interação automática
                };

                await _chamadoRepository.AdicionarInteracaoAsync(chamadoId, interacao);

                return chamadoId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar chamado: {ex.Message}", ex);
            }
        }

        public async Task<Chamado?> ObterChamadoPorIdAsync(int id, string usuarioEmail)
        {
            try
            {
                var chamado = await _chamadoRepository.ObterPorIdAsync(id);

                if (chamado != null)
                {
                    // Carregar interações
                    chamado.Interacoes = await _chamadoRepository.ListarInteracoesAsync(id);

                    // Registrar log LGPD
                    await _lgpdService.RegistrarAcessoAsync(
                        usuarioEmail,
                        "acesso",
                        "chamados",
                        id,
                        "todos os campos",
                        null,
                        null
                    );
                }

                return chamado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter chamado: {ex.Message}", ex);
            }
        }

        public async Task<List<Chamado>> ListarChamadosUsuarioAsync(string email, string? filtroStatus = null)
        {
            try
            {
                var chamados = await _chamadoRepository.ListarPorUsuarioAsync(email, filtroStatus);

                // Registrar log LGPD
                await _lgpdService.RegistrarAcessoAsync(
                    email,
                    "listagem",
                    "chamados",
                    null,
                    "id, titulo, status, data_abertura",
                    null,
                    null
                );

                return chamados;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar chamados: {ex.Message}", ex);
            }
        }

        public async Task<bool> AdicionarComentarioAsync(int chamadoId, string comentario, string usuarioEmail)
        {
            try
            {
                var interacao = new Interacao
                {
                    ChamadoId = chamadoId,
                    DataHora = DateTime.Now,
                    Descricao = comentario,
                    Usuario = usuarioEmail
                };

                var resultado = await _chamadoRepository.AdicionarInteracaoAsync(chamadoId, interacao);

                // Registrar log LGPD
                await _lgpdService.RegistrarAcessoAsync(
                    usuarioEmail,
                    "modificacao",
                    "interacoes",
                    chamadoId,
                    "descricao",
                    null,
                    null
                );

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar comentário: {ex.Message}", ex);
            }
        }

        public async Task<bool> FinalizarChamadoAsync(int chamadoId, string solucao, string usuarioEmail)
        {
            try
            {
                var resultado = await _chamadoRepository.FinalizarAsync(chamadoId, solucao);

                if (resultado)
                {
                    // Adicionar interação de finalização
                    var interacao = new Interacao
                    {
                        ChamadoId = chamadoId,
                        DataHora = DateTime.Now,
                        Descricao = $"Chamado finalizado. Solução: {solucao}",
                        Usuario = usuarioEmail
                    };

                    await _chamadoRepository.AdicionarInteracaoAsync(chamadoId, interacao);

                    // Registrar log LGPD
                    await _lgpdService.RegistrarAcessoAsync(
                        usuarioEmail,
                        "modificacao",
                        "chamados",
                        chamadoId,
                        "status, solucao, data_fechamento",
                        null,
                        null
                    );
                }

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao finalizar chamado: {ex.Message}", ex);
            }
        }
    }
}
