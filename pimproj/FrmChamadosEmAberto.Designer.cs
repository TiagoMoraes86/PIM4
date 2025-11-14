namespace SistemaChamados
{
    partial class FrmChamadosEmAberto
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvChamados;
        private System.Windows.Forms.Button btnInteragir;
        private System.Windows.Forms.Button btnAtribuirTecnico;
        private System.Windows.Forms.Panel pnlBotoes;

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
            dgvChamados = new DataGridView();
            btnInteragir = new Button();
            btnAtribuirTecnico = new Button();
            pnlBotoes = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvChamados).BeginInit();
            pnlBotoes.SuspendLayout();
            SuspendLayout();
            // 
            // dgvChamados
            // 
            dgvChamados.AllowUserToAddRows = false;
            dgvChamados.AllowUserToDeleteRows = false;
            dgvChamados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChamados.BackgroundColor = SystemColors.ActiveCaption;
            dgvChamados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChamados.Location = new Point(14, 14);
            dgvChamados.Margin = new Padding(4, 3, 4, 3);
            dgvChamados.MultiSelect = false;
            dgvChamados.Name = "dgvChamados";
            dgvChamados.ReadOnly = true;
            dgvChamados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChamados.Size = new Size(905, 445);
            dgvChamados.TabIndex = 0;
            // 
            // btnInteragir
            // 
            btnInteragir.BackColor = SystemColors.ScrollBar;
            btnInteragir.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnInteragir.Location = new Point(397, 9);
            btnInteragir.Margin = new Padding(4, 3, 4, 3);
            btnInteragir.Name = "btnInteragir";
            btnInteragir.Size = new Size(140, 27);
            btnInteragir.TabIndex = 0;
            btnInteragir.Text = "Interagir";
            btnInteragir.UseVisualStyleBackColor = false;
            btnInteragir.Click += btnInteragir_Click;
            // 
            // btnAtribuirTecnico
            // 
            btnAtribuirTecnico.Location = new Point(430, 8);
            btnAtribuirTecnico.Name = "btnAtribuirTecnico";
            btnAtribuirTecnico.Size = new Size(120, 23);
            btnAtribuirTecnico.TabIndex = 1;
            btnAtribuirTecnico.Text = "Atribuir Técnico";
            btnAtribuirTecnico.UseVisualStyleBackColor = true;
            btnAtribuirTecnico.Click += btnAtribuirTecnico_Click;
            // 
            // pnlBotoes
            // 
            pnlBotoes.Controls.Add(btnInteragir);
            pnlBotoes.Dock = DockStyle.Bottom;
            pnlBotoes.Location = new Point(0, 473);
            pnlBotoes.Margin = new Padding(4, 3, 4, 3);
            pnlBotoes.Name = "pnlBotoes";
            pnlBotoes.Size = new Size(933, 46);
            pnlBotoes.TabIndex = 1;
            // 
            // FrmChamadosEmAberto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            ClientSize = new Size(933, 519);
            Controls.Add(pnlBotoes);
            Controls.Add(dgvChamados);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmChamadosEmAberto";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chamados em Aberto";
            ((System.ComponentModel.ISupportInitialize)dgvChamados).EndInit();
            pnlBotoes.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
