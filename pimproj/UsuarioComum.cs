using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaChamados.Model
{
    public class UsuarioComum : Usuario
    {
        public UsuarioComum(string email, string senha, string nome)
            : base(email, senha, nome)
        {
        }

        public override string ObterTipoUsuario()
        {
            return "usuario";
        }
    }

    public class Tecnico : Usuario
    {
        public string Especialidade { get; private set; } = string.Empty;

        public Tecnico(string email, string senha, string nome, string especialidade)
            : base(email, senha, nome)
        {
            Especialidade = especialidade;
        }

        public override string ObterTipoUsuario()
        {
            return "tecnico";
        }
    }
}
