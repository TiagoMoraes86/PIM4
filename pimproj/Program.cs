using SistemaChamados.Data;
using System;
using System.Windows.Forms;

namespace SistemaChamados
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Verificar conexão com o banco de dados e existência das tabelas
                if (DatabaseInitializer.VerificarConexaoETabelas())
                {
                    Application.Run(new FrmLogin());
                }
                else
                {
                    MessageBox.Show("Não foi possível conectar ao banco de dados ou as tabelas necessárias não existem.",
                                   "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar o sistema: {ex.Message}",
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
