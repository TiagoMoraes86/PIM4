using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaChamados.Model
{
    public abstract class Usuario
    {
        public string Email { get; protected set; } = string.Empty;
        public string Senha { get; protected set; } = string.Empty;
        public string Nome { get; protected set; } = string.Empty;

        protected Usuario(string email, string senha, string nome)
        {
            Email = email;
            Senha = senha;
            Nome = nome;
        }

        public abstract string ObterTipoUsuario();
    }
}
