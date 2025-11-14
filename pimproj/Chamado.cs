using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaChamados.Model
{
    public class Chamado
    {
        // Propriedades
        public int Id { get; private set; }
        public string Titulo { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public string Categoria { get; private set; } = string.Empty;
        public string Prioridade { get; private set; } = string.Empty;
        public DateTime DataAbertura { get; private set; }
        public DateTime? DataFechamento { get; private set; }
        public string Solicitante { get; private set; } = string.Empty;
        public string Tecnico { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public string Solucao { get; private set; } = string.Empty;
        public List<Interacao> Interacoes { get; private set; } = new List<Interacao>();

        // Construtor
        public Chamado(int id, string titulo, string descricao, string categoria,
                      string prioridade, string solicitante)
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            Categoria = categoria;
            Prioridade = prioridade;
            DataAbertura = DateTime.Now;
            DataFechamento = null;
            Solicitante = solicitante;
            Tecnico = "";
            Status = "Aguardando atribuição";
            Solucao = "";
        }

        // Métodos

        public void DefinirDataFechamento(DateTime dataFechamento)
        {
            DataFechamento = dataFechamento;
        }

        public void AdicionarInteracaoDoBanco(DateTime dataHora, string descricao)
        {
            Interacoes.Add(new Interacao(dataHora, descricao));
        }

        public void AtribuirTecnico(string tecnico)
        {
            Tecnico = tecnico;
            Status = "Em análise";
            AdicionarInteracao($"Chamado atribuído para {tecnico}");
        }

        public void AlterarStatus(string novoStatus)
        {
            Status = novoStatus;
            AdicionarInteracao($"Status alterado para: {novoStatus}");

            if (novoStatus == "Finalizado")
            {
                DataFechamento = DateTime.Now;
            }
        }

        public void DefinirSolucao(string solucao)
        {
            Solucao = solucao;
            AdicionarInteracao($"Solução registrada: {solucao}");
        }

        private void AdicionarInteracao(string descricao)
        {
            Interacoes.Add(new Interacao(DateTime.Now, descricao));
        }

        public void AdicionarComentario(string usuario, string comentario)
        {
            string descricao = $"{usuario}: {comentario}";
            AdicionarInteracao(descricao);
        }
    }
}
