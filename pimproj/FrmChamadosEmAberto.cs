using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaChamados.Controller;
using SistemaChamados.Model;

namespace SistemaChamados
{
    public partial class FrmChamadosEmAberto : Form
    {
        private string tipoUsuario = string.Empty;

        public FrmChamadosEmAberto(string tipoUsuario)
        {
            InitializeComponent();
            this.tipoUsuario = tipoUsuario;
            CarregarChamadosEmAberto();
            AjustarPermissoes();
        }

        private void AjustarPermissoes()
        {
            
            if (pnlBotoes.Controls.Contains(btnAtribuirTecnico))
            {
                pnlBotoes.Controls.Remove(btnAtribuirTecnico);
                btnAtribuirTecnico.Dispose();
            }

            
            btnInteragir.Location = new System.Drawing.Point((pnlBotoes.Width - btnInteragir.Width) / 2, 8);

            if (tipoUsuario == "usuario")
            {
                
                btnInteragir.Enabled = false;
                
                dgvChamados.ReadOnly = true;
            }
            else if (tipoUsuario == "tecnico")
            {
                
                btnInteragir.Enabled = true;
            }
        }

        private void CarregarChamadosEmAberto()
        {
            
            List<Chamado> chamadosEmAberto = GerenciadorChamados.GetInstancia().ObterChamadosEmAberto();

            
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Título", typeof(string));
            dt.Columns.Add("Categoria", typeof(string));
            dt.Columns.Add("Prioridade", typeof(string));
            dt.Columns.Add("Data Abertura", typeof(DateTime));
            dt.Columns.Add("Solicitante", typeof(string));
            dt.Columns.Add("Técnico", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            
            foreach (Chamado chamado in chamadosEmAberto)
            {
                dt.Rows.Add(
                    chamado.Id,
                    chamado.Titulo,
                    chamado.Categoria,
                    chamado.Prioridade,
                    chamado.DataAbertura,
                    chamado.Solicitante,
                    chamado.Tecnico,
                    chamado.Status
                );
            }

            dgvChamados.DataSource = dt;
            dgvChamados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnInteragir_Click(object sender, EventArgs e)
        {
            if (dgvChamados.SelectedRows.Count > 0)
            {
                int idChamado = Convert.ToInt32(dgvChamados.SelectedRows[0].Cells["ID"].Value);

                Chamado? chamado = GerenciadorChamados.GetInstancia().ObterChamadoPorId(idChamado);

                if (chamado != null)
                {
                    string novoStatus = "";
                    if (chamado.Status == "Em análise")
                        novoStatus = "Em andamento";
                    else if (chamado.Status == "Em andamento")
                        novoStatus = "Finalizado";
                    else
                        novoStatus = "Em análise";

                    chamado.AlterarStatus(novoStatus);

                    if (novoStatus == "Finalizado")
                    {
                        string solucao = Microsoft.VisualBasic.Interaction.InputBox(
                            "Informe a solução aplicada:", "Finalizar Chamado", "");

                        if (!string.IsNullOrEmpty(solucao))
                        {
                            chamado.DefinirSolucao(solucao);
                        }
                    }

                    // Atualizar o chamado no banco de dados
                    GerenciadorChamados.GetInstancia().AtualizarChamado(chamado);

                    CarregarChamadosEmAberto();

                    MessageBox.Show($"Status do chamado ID {idChamado} alterado para: {novoStatus}",
                        "Status Alterado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecione um chamado para interagir!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnAtribuirTecnico_Click(object sender, EventArgs e)
        {
            if (dgvChamados.SelectedRows.Count > 0)
            {
                int idChamado = Convert.ToInt32(dgvChamados.SelectedRows[0].Cells["ID"].Value);
                MessageBox.Show($"Atribuindo técnico ao chamado ID: {idChamado}", "Atribuição", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            else
            {
                MessageBox.Show("Selecione um chamado para atribuir um técnico!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
