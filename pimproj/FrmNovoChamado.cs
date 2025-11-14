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

namespace SistemaChamados
{
    public partial class FrmNovoChamado : Form
    {
        private string usuarioSolicitante = string.Empty;

        public FrmNovoChamado(string email)
        {
            InitializeComponent();
            usuarioSolicitante = email;

            
            PreencherCategorias();
            PreencherPrioridades();
        }

        private void PreencherCategorias()
        {
            cbCategoria.Items.Add("Hardware");
            cbCategoria.Items.Add("Software");
            cbCategoria.Items.Add("Rede");
            cbCategoria.SelectedIndex = 0;
        }

        private void PreencherPrioridades()
        {
            cbPrioridade.Items.Add("Baixa");
            cbPrioridade.Items.Add("Média");
            cbPrioridade.Items.Add("Alta");
            cbPrioridade.SelectedIndex = 0;
        }

        private void btnCriarChamado_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitulo.Text) || string.IsNullOrEmpty(txtDescricao.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos!", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string titulo = txtTitulo.Text;
            string descricao = txtDescricao.Text;
            string? categoria = cbCategoria.SelectedItem?.ToString();
            string? prioridade = cbPrioridade.SelectedItem?.ToString();

            if (categoria == null || prioridade == null)
            {
                MessageBox.Show("Por favor, selecione categoria e prioridade!", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            GerenciadorChamados.GetInstancia().CriarChamado(
                titulo, descricao, categoria, prioridade, usuarioSolicitante);

            MessageBox.Show($"Chamado criado com sucesso!\n\nTítulo: {titulo}\nCategoria: {categoria}\nPrioridade: {prioridade}",
                "Chamado Criado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void cbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
