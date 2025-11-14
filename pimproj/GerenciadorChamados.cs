using SistemaChamados.Data;
using SistemaChamados.Model;
using System;
using System.Collections.Generic;

namespace SistemaChamados.Controller
{
    public class GerenciadorChamados
    {
        private static GerenciadorChamados? _instancia;
        private ChamadoRepository _repository = new ChamadoRepository();

        public static GerenciadorChamados GetInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new GerenciadorChamados();
            }
            return _instancia;
        }

        private GerenciadorChamados()
        {
            
        }

        public Chamado CriarChamado(string titulo, string descricao, string categoria,
                                   string prioridade, string solicitante)
        {
            return _repository.CriarChamado(titulo, descricao, categoria, prioridade, solicitante);
        }

        public List<Chamado> ObterChamadosEmAberto()
        {
            return _repository.ObterChamadosEmAberto();
        }

        public List<Chamado> ObterChamadosFinalizados()
        {
            return _repository.ObterChamadosFinalizados();
        }

        public Chamado? ObterChamadoPorId(int id)
        {
            return _repository.ObterChamadoPorId(id);
        }

        public void AtualizarChamado(Chamado chamado)
        {
            _repository.AtualizarChamado(chamado);
        }
    }
}
