using SistemaChamados.Model;
using Xunit;
using System.Linq;

namespace SistemaChamados.Tests
{
    public class ChamadoTests
    {
        [Fact]
        public void CriarChamado_DeveDefinirPropriedadesCorretamente()
        {
            
            int id = 1;
            string titulo = "Erro no sistema";
            string descricao = "Não é possível logar.";
            string categoria = "Software";
            string prioridade = "Alta";
            string solicitante = "usuario@exemplo.com";

            
            var chamado = new Chamado(id, titulo, descricao, categoria, prioridade, solicitante);

            
            Assert.Equal(id, chamado.Id);
            Assert.Equal(titulo, chamado.Titulo);
            Assert.Equal(descricao, chamado.Descricao);
            Assert.Equal(categoria, chamado.Categoria);
            Assert.Equal(prioridade, chamado.Prioridade);
            Assert.Equal(solicitante, chamado.Solicitante);
            Assert.Equal("Aguardando atribuição", chamado.Status);
            Assert.Null(chamado.DataFechamento);
            Assert.Empty(chamado.Interacoes); 
        }

        [Fact]
        public void AlterarStatus_ParaFinalizado_DeveDefinirDataFechamento()
        {
            
            var chamado = new Chamado(2, "Problema na rede", "Sem acesso à internet", "Rede", "Média", "user@exemplo.com");

           
            chamado.AlterarStatus("Finalizado");

            
            Assert.Equal("Finalizado", chamado.Status);
            Assert.NotNull(chamado.DataFechamento);
            Assert.Contains(chamado.Interacoes, i => i.Descricao.Contains("Status alterado para: Finalizado"));
        }
    }
}