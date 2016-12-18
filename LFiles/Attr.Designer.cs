namespace LFiles
{
    partial class Attr
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
            this.cbRO = new System.Windows.Forms.CheckBox();
            this.cbHI = new System.Windows.Forms.CheckBox();
            this.cbSY = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbRO
            // 
            this.cbRO.AutoSize = true;
            this.cbRO.Location = new System.Drawing.Point(12, 114);
            this.cbRO.Name = "cbRO";
            this.cbRO.Size = new System.Drawing.Size(100, 17);
            this.cbRO.TabIndex = 0;
            this.cbRO.Text = "Только чтение";
            this.cbRO.UseVisualStyleBackColor = true;
            this.cbRO.CheckedChanged += new System.EventHandler(this.cbRO_CheckedChanged);
            // 
            // cbHI
            // 
            this.cbHI.AutoSize = true;
            this.cbHI.Location = new System.Drawing.Point(174, 114);
            this.cbHI.Name = "cbHI";
            this.cbHI.Size = new System.Drawing.Size(72, 17);
            this.cbHI.TabIndex = 1;
            this.cbHI.Text = "Скрытый";
            this.cbHI.UseVisualStyleBackColor = true;
            this.cbHI.CheckedChanged += new System.EventHandler(this.cbHI_CheckedChanged);
            // 
            // cbSY
            // 
            this.cbSY.AutoSize = true;
            this.cbSY.Location = new System.Drawing.Point(326, 114);
            this.cbSY.Name = "cbSY";
            this.cbSY.Size = new System.Drawing.Size(84, 17);
            this.cbSY.TabIndex = 2;
            this.cbSY.Text = "Системный";
            this.cbSY.UseVisualStyleBackColor = true;
            this.cbSY.CheckedChanged += new System.EventHandler(this.cbSY_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(410, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Это папка";
            // 
            // Attr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 143);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbSY);
            this.Controls.Add(this.cbHI);
            this.Controls.Add(this.cbRO);
            this.Name = "Attr";
            this.Text = "Attr";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbRO;
        private System.Windows.Forms.CheckBox cbHI;
        private System.Windows.Forms.CheckBox cbSY;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}