using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaChamados
{
    public partial class FrmPrincipal : Form
    {
        private string usuarioLogado = string.Empty;
        private string tipoUsuario = string.Empty;

        public FrmPrincipal(string email, string tipo)
        {
            InitializeComponent();
            usuarioLogado = email;
            tipoUsuario = tipo;
            lblBemVindo.Text = $"Bem-vindo(a)!";
        }

        private void btnNovoChamado_Click(object sender, EventArgs e)
        {
            FrmNovoChamado frmNovoChamado = new FrmNovoChamado(usuarioLogado);
            frmNovoChamado.ShowDialog();
        }

        private void btnChamadosEmAberto_Click(object sender, EventArgs e)
        {
            FrmChamadosEmAberto frmChamadosEmAberto = new FrmChamadosEmAberto(tipoUsuario);
            frmChamadosEmAberto.ShowDialog();
        }

        private void btnChamadosFinalizados_Click(object sender, EventArgs e)
        {
            FrmChamadosFinalizados frmChamadosFinalizados = new FrmChamadosFinalizados();
            frmChamadosFinalizados.ShowDialog();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void lblBemVindo_Click(object sender, EventArgs e)
        {

        }
    }
}
