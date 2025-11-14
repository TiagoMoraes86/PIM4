using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaChamados.Model;
using SistemaChamados.Controller;

namespace SistemaChamados
{
    public partial class FrmChamadosFinalizados : Form
    {
        public FrmChamadosFinalizados()
        {
            InitializeComponent();
            CarregarChamadosFinalizados();
        }

        private void CarregarChamadosFinalizados()
        {
            // Obter os chamados finalizados
            List<Chamado> chamadosFinalizados = GerenciadorChamados.GetInstancia().ObterChamadosFinalizados();

            // Criar DataTable para exibição
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Título", typeof(string));
            dt.Columns.Add("Categoria", typeof(string));
            dt.Columns.Add("Prioridade", typeof(string));
            dt.Columns.Add("Data Abertura", typeof(DateTime));
            dt.Columns.Add("Data Fechamento", typeof(DateTime));
            dt.Columns.Add("Solicitante", typeof(string));
            dt.Columns.Add("Técnico", typeof(string));
            dt.Columns.Add("Solução", typeof(string));

            // Preencher DataTable com os chamados
            foreach (Chamado chamado in chamadosFinalizados)
            {
                dt.Rows.Add(
                    chamado.Id,
                    chamado.Titulo,
                    chamado.Categoria,
                    chamado.Prioridade,
                    chamado.DataAbertura,
                    chamado.DataFechamento,
                    chamado.Solicitante,
                    chamado.Tecnico,
                    chamado.Solucao
                );
            }

            dgvChamadosFinalizados.DataSource = dt;
            dgvChamadosFinalizados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
