namespace SistemaChamados
{
    partial class FrmNovoChamado
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.ComboBox cbCategoria;
        private System.Windows.Forms.ComboBox cbPrioridade;
        private System.Windows.Forms.Button btnCriarChamado;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.Label lblPrioridade;

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
            txtTitulo = new TextBox();
            txtDescricao = new TextBox();
            cbCategoria = new ComboBox();
            cbPrioridade = new ComboBox();
            btnCriarChamado = new Button();
            lblTitulo = new Label();
            lblDescricao = new Label();
            lblCategoria = new Label();
            lblPrioridade = new Label();
            SuspendLayout();
            // 
            // txtTitulo
            // 
            txtTitulo.BackColor = SystemColors.ScrollBar;
            txtTitulo.Location = new Point(140, 31);
            txtTitulo.Margin = new Padding(4, 3, 4, 3);
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(408, 23);
            txtTitulo.TabIndex = 1;
            // 
            // txtDescricao
            // 
            txtDescricao.BackColor = SystemColors.ScrollBar;
            txtDescricao.Location = new Point(140, 127);
            txtDescricao.Margin = new Padding(4, 3, 4, 3);
            txtDescricao.Multiline = true;
            txtDescricao.Name = "txtDescricao";
            txtDescricao.Size = new Size(408, 172);
            txtDescricao.TabIndex = 7;
            // 
            // cbCategoria
            // 
            cbCategoria.BackColor = SystemColors.ScrollBar;
            cbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategoria.FormattingEnabled = true;
            cbCategoria.Location = new Point(140, 77);
            cbCategoria.Margin = new Padding(4, 3, 4, 3);
            cbCategoria.Name = "cbCategoria";
            cbCategoria.Size = new Size(174, 23);
            cbCategoria.TabIndex = 3;
            cbCategoria.SelectedIndexChanged += cbCategoria_SelectedIndexChanged;
            // 
            // cbPrioridade
            // 
            cbPrioridade.BackColor = SystemColors.ScrollBar;
            cbPrioridade.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPrioridade.FormattingEnabled = true;
            cbPrioridade.Location = new Point(408, 77);
            cbPrioridade.Margin = new Padding(4, 3, 4, 3);
            cbPrioridade.Name = "cbPrioridade";
            cbPrioridade.Size = new Size(139, 23);
            cbPrioridade.TabIndex = 5;
            // 
            // btnCriarChamado
            // 
            btnCriarChamado.BackColor = SystemColors.ScrollBar;
            btnCriarChamado.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCriarChamado.Location = new Point(233, 323);
            btnCriarChamado.Margin = new Padding(4, 3, 4, 3);
            btnCriarChamado.Name = "btnCriarChamado";
            btnCriarChamado.Size = new Size(140, 35);
            btnCriarChamado.TabIndex = 8;
            btnCriarChamado.Text = "Criar Chamado";
            btnCriarChamado.UseVisualStyleBackColor = false;
            btnCriarChamado.Click += btnCriarChamado_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(35, 35);
            lblTitulo.Margin = new Padding(4, 0, 4, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(41, 15);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Título:";
            // 
            // lblDescricao
            // 
            lblDescricao.AutoSize = true;
            lblDescricao.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDescricao.Location = new Point(35, 127);
            lblDescricao.Margin = new Padding(4, 0, 4, 0);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(62, 15);
            lblDescricao.TabIndex = 6;
            lblDescricao.Text = "Descrição:";
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategoria.Location = new Point(35, 81);
            lblCategoria.Margin = new Padding(4, 0, 4, 0);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(60, 15);
            lblCategoria.TabIndex = 2;
            lblCategoria.Text = "Categoria:";
            // 
            // lblPrioridade
            // 
            lblPrioridade.AutoSize = true;
            lblPrioridade.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrioridade.Location = new Point(338, 81);
            lblPrioridade.Margin = new Padding(4, 0, 4, 0);
            lblPrioridade.Name = "lblPrioridade";
            lblPrioridade.Size = new Size(64, 15);
            lblPrioridade.TabIndex = 4;
            lblPrioridade.Text = "Prioridade:";
            // 
            // FrmNovoChamado
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            ClientSize = new Size(583, 381);
            Controls.Add(btnCriarChamado);
            Controls.Add(txtDescricao);
            Controls.Add(lblDescricao);
            Controls.Add(cbPrioridade);
            Controls.Add(lblPrioridade);
            Controls.Add(cbCategoria);
            Controls.Add(lblCategoria);
            Controls.Add(txtTitulo);
            Controls.Add(lblTitulo);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmNovoChamado";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Novo Chamado";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
