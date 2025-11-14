namespace SistemaChamados
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnNovoChamado;
        private System.Windows.Forms.Button btnChamadosEmAberto;
        private System.Windows.Forms.Button btnChamadosFinalizados;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Label lblBemVindo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnNovoChamado = new Button();
            btnChamadosEmAberto = new Button();
            btnChamadosFinalizados = new Button();
            btnSair = new Button();
            lblBemVindo = new Label();
            SuspendLayout();
            // 
            // btnNovoChamado
            // 
            btnNovoChamado.BackColor = SystemColors.ScrollBar;
            btnNovoChamado.Location = new Point(117, 94);
            btnNovoChamado.Margin = new Padding(4, 3, 4, 3);
            btnNovoChamado.Name = "btnNovoChamado";
            btnNovoChamado.Size = new Size(233, 46);
            btnNovoChamado.TabIndex = 0;
            btnNovoChamado.Text = "Novo Chamado";
            btnNovoChamado.UseVisualStyleBackColor = false;
            btnNovoChamado.Click += btnNovoChamado_Click;
            // 
            // btnChamadosEmAberto
            // 
            btnChamadosEmAberto.BackColor = SystemColors.ScrollBar;
            btnChamadosEmAberto.Location = new Point(117, 162);
            btnChamadosEmAberto.Margin = new Padding(4, 3, 4, 3);
            btnChamadosEmAberto.Name = "btnChamadosEmAberto";
            btnChamadosEmAberto.Size = new Size(233, 46);
            btnChamadosEmAberto.TabIndex = 1;
            btnChamadosEmAberto.Text = "Chamados em Aberto";
            btnChamadosEmAberto.UseVisualStyleBackColor = false;
            btnChamadosEmAberto.Click += btnChamadosEmAberto_Click;
            // 
            // btnChamadosFinalizados
            // 
            btnChamadosFinalizados.BackColor = SystemColors.ScrollBar;
            btnChamadosFinalizados.Location = new Point(117, 231);
            btnChamadosFinalizados.Margin = new Padding(4, 3, 4, 3);
            btnChamadosFinalizados.Name = "btnChamadosFinalizados";
            btnChamadosFinalizados.Size = new Size(233, 46);
            btnChamadosFinalizados.TabIndex = 2;
            btnChamadosFinalizados.Text = "Chamados Finalizados";
            btnChamadosFinalizados.UseVisualStyleBackColor = false;
            btnChamadosFinalizados.Click += btnChamadosFinalizados_Click;
            // 
            // btnSair
            // 
            btnSair.BackColor = SystemColors.ScrollBar;
            btnSair.Location = new Point(117, 300);
            btnSair.Margin = new Padding(4, 3, 4, 3);
            btnSair.Name = "btnSair";
            btnSair.Size = new Size(233, 46);
            btnSair.TabIndex = 3;
            btnSair.Text = "Sair";
            btnSair.UseVisualStyleBackColor = false;
            btnSair.Click += btnSair_Click;
            // 
            // lblBemVindo
            // 
            lblBemVindo.AutoSize = true;
            lblBemVindo.Font = new Font("Franklin Gothic Medium", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBemVindo.Location = new Point(178, 9);
            lblBemVindo.Margin = new Padding(4, 0, 4, 0);
            lblBemVindo.Name = "lblBemVindo";
            lblBemVindo.Size = new Size(102, 24);
            lblBemVindo.TabIndex = 4;
            lblBemVindo.Text = "Bem-vindo!";
            lblBemVindo.Click += lblBemVindo_Click;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            ClientSize = new Size(467, 381);
            Controls.Add(lblBemVindo);
            Controls.Add(btnSair);
            Controls.Add(btnChamadosFinalizados);
            Controls.Add(btnChamadosEmAberto);
            Controls.Add(btnNovoChamado);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistema de Chamados - Menu Principal";
            FormClosed += FrmPrincipal_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
