using SistemaChamados.Data;
using SistemaChamados.Model;
using System.Collections.Generic;

namespace SistemaChamados.Controller
{
    public class GerenciadorUsuarios
    {
        private static GerenciadorUsuarios? _instancia;
        private UsuarioRepository _repository = new UsuarioRepository();

        public static GerenciadorUsuarios GetInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new GerenciadorUsuarios();
            }
            return _instancia;
        }

        private GerenciadorUsuarios()
        {
            // Inicializar com usuários padrão se o banco estiver vazio
            InicializarUsuariosPadrao();
        }

        private void InicializarUsuariosPadrao()
        {
            try
            {
                List<Tecnico> tecnicos = _repository.ObterTodosTecnicos();

if (tecnicos.Count == 0)
                {
                    // Adicionar usuários padrão
                    // Senhas: admin123, tecnico123, user123
                    _repository.AdicionarUsuario(new Tecnico("admin@sistema.com", "admin123", "Administrador", "Suporte Geral"));
                    _repository.AdicionarUsuario(new Tecnico("tecnico@sistema.com", "tecnico123", "Técnico Suporte", "Suporte Técnico"));
                    _repository.AdicionarUsuario(new UsuarioComum("user@sistema.com", "user123", "Usuário Teste"));
                }
            }
            catch
            {
                // Ignorar erros na inicialização
            }
        }


        public Usuario? Autenticar(string email, string senha)
        {
            return _repository.Autenticar(email, senha);
        }

        public List<Tecnico> ObterTodosTecnicos()
        {
            return _repository.ObterTodosTecnicos();
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            _repository.AdicionarUsuario(usuario);
        }
    }
}
