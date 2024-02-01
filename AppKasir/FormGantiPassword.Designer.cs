
namespace AppKasir
{
    partial class FormGantiPassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_simpan = new System.Windows.Forms.Button();
            this.LBLUsername = new System.Windows.Forms.TextBox();
            this.LBLPasswordBaru = new System.Windows.Forms.TextBox();
            this.LBLKonfPasswordBaru = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password Baru";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Konfirmasi Password Baru";
            // 
            // btn_simpan
            // 
            this.btn_simpan.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_simpan.Location = new System.Drawing.Point(335, 196);
            this.btn_simpan.Name = "btn_simpan";
            this.btn_simpan.Size = new System.Drawing.Size(108, 47);
            this.btn_simpan.TabIndex = 3;
            this.btn_simpan.Text = "SIMPAN";
            this.btn_simpan.UseVisualStyleBackColor = false;
            this.btn_simpan.Click += new System.EventHandler(this.btn_simpan_Click);
            // 
            // LBLUsername
            // 
            this.LBLUsername.Location = new System.Drawing.Point(229, 62);
            this.LBLUsername.Name = "LBLUsername";
            this.LBLUsername.Size = new System.Drawing.Size(214, 22);
            this.LBLUsername.TabIndex = 4;
            // 
            // LBLPasswordBaru
            // 
            this.LBLPasswordBaru.Location = new System.Drawing.Point(229, 106);
            this.LBLPasswordBaru.Name = "LBLPasswordBaru";
            this.LBLPasswordBaru.Size = new System.Drawing.Size(214, 22);
            this.LBLPasswordBaru.TabIndex = 5;
            // 
            // LBLKonfPasswordBaru
            // 
            this.LBLKonfPasswordBaru.Location = new System.Drawing.Point(229, 149);
            this.LBLKonfPasswordBaru.Name = "LBLKonfPasswordBaru";
            this.LBLKonfPasswordBaru.Size = new System.Drawing.Size(214, 22);
            this.LBLKonfPasswordBaru.TabIndex = 6;
            // 
            // FormGantiPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 280);
            this.Controls.Add(this.LBLKonfPasswordBaru);
            this.Controls.Add(this.LBLPasswordBaru);
            this.Controls.Add(this.LBLUsername);
            this.Controls.Add(this.btn_simpan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormGantiPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ganti Password";
            this.Load += new System.EventHandler(this.FormGantiPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_simpan;
        private System.Windows.Forms.TextBox LBLUsername;
        private System.Windows.Forms.TextBox LBLPasswordBaru;
        private System.Windows.Forms.TextBox LBLKonfPasswordBaru;
    }
}