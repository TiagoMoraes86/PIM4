namespace SistemaChamados
{
    partial class FrmChamadosFinalizados
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvChamadosFinalizados;

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
            dgvChamadosFinalizados = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvChamadosFinalizados).BeginInit();
            SuspendLayout();
            // 
            // dgvChamadosFinalizados
            // 
            dgvChamadosFinalizados.AllowUserToAddRows = false;
            dgvChamadosFinalizados.AllowUserToDeleteRows = false;
            dgvChamadosFinalizados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChamadosFinalizados.BackgroundColor = SystemColors.ActiveCaption;
            dgvChamadosFinalizados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChamadosFinalizados.Location = new Point(14, 14);
            dgvChamadosFinalizados.Margin = new Padding(4, 3, 4, 3);
            dgvChamadosFinalizados.MultiSelect = false;
            dgvChamadosFinalizados.Name = "dgvChamadosFinalizados";
            dgvChamadosFinalizados.ReadOnly = true;
            dgvChamadosFinalizados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChamadosFinalizados.Size = new Size(905, 492);
            dgvChamadosFinalizados.TabIndex = 0;
            // 
            // FrmChamadosFinalizados
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.CornflowerBlue;
            ClientSize = new Size(933, 519);
            Controls.Add(dgvChamadosFinalizados);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmChamadosFinalizados";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chamados Finalizados";
            ((System.ComponentModel.ISupportInitialize)dgvChamadosFinalizados).EndInit();
            ResumeLayout(false);
        }
    }
}
