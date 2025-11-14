using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaChamados.Model
{
    public class Interacao
    {
        public DateTime DataHora { get; private set; }
        public string Descricao { get; private set; } = string.Empty;

        public Interacao(DateTime dataHora, string descricao)
        {
            DataHora = dataHora;
            Descricao = descricao;
        }
    }
}
