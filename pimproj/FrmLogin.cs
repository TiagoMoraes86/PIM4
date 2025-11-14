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

    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            // Validar campos vazios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha email e senha!", "Campos Obrigatórios",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Usuario? usuario = GerenciadorUsuarios.GetInstancia().Autenticar(email, senha);

                if (usuario != null)
                {
                    FrmPrincipal frmPrincipal = new FrmPrincipal(usuario.Email, usuario.ObterTipoUsuario());
                    frmPrincipal.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Email ou senha incorretos!\n\nVerifique suas credenciais e tente novamente.", 
                        "Erro de Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSenha.Clear();
                    txtSenha.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar ao banco de dados:\n\n{ex.Message}\n\nVerifique se o PostgreSQL está rodando e se o banco 'pim' existe.",
                    "Erro de Conexão",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
