using BCrypt.Net;
using SistemaChamados.Web.Data.Repositories;
using SistemaChamados.Web.Models;
using System;
using System.Threading.Tasks;

namespace SistemaChamados.Web.Services
{
    /// <summary>
    /// Serviço de autenticação de usuários
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILGPDService _lgpdService;

        public AuthService(IUsuarioRepository usuarioRepository, ILGPDService lgpdService)
        {
            _usuarioRepository = usuarioRepository;
            _lgpdService = lgpdService;
        }

        public async Task<Usuario?> ValidarCredenciaisAsync(string email, string senha)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorEmailAsync(email);

                if (usuario == null || !usuario.Ativo)
                {
                    return null;
                }

                // Verifica a senha
                if (VerificarSenha(senha, usuario.Senha))
                {
                    return usuario;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao validar credenciais: {ex.Message}", ex);
            }
        }

        public async Task RegistrarLoginAsync(string email, string ipAddress, string userAgent)
        {
            try
            {
                await _lgpdService.RegistrarAcessoAsync(
                    email,
                    "login",
                    "usuarios",
                    null,
                    "email, nome, tipo",
                    ipAddress,
                    userAgent
                );
            }
            catch (Exception ex)
            {
                // Não quebrar o fluxo se falhar o log
                Console.WriteLine($"Erro ao registrar login: {ex.Message}");
            }
        }

        public string HashSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool VerificarSenha(string senha, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(senha, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}
