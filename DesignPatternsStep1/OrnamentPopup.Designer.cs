namespace DesignPatternsStep1
{
    partial class OrnamentPopup
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
            this.addOrnamentBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ornamentInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // addOrnamentBtn
            // 
            this.addOrnamentBtn.Location = new System.Drawing.Point(15, 156);
            this.addOrnamentBtn.Name = "addOrnamentBtn";
            this.addOrnamentBtn.Size = new System.Drawing.Size(141, 23);
            this.addOrnamentBtn.TabIndex = 0;
            this.addOrnamentBtn.Text = "Add Ornament";
            this.addOrnamentBtn.UseVisualStyleBackColor = true;
            this.addOrnamentBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(226, 156);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "New Ornament";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Above",
            "Below",
            "Left",
            "Right"});
            this.comboBox1.Location = new System.Drawing.Point(15, 96);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 4;
            // 
            // ornamentInput
            // 
            this.ornamentInput.Location = new System.Drawing.Point(15, 45);
            this.ornamentInput.Name = "ornamentInput";
            this.ornamentInput.Size = new System.Drawing.Size(121, 22);
            this.ornamentInput.TabIndex = 5;
            // 
            // OrnamentPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 194);
            this.Controls.Add(this.ornamentInput);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.addOrnamentBtn);
            this.Name = "OrnamentPopup";
            this.Text = "OrnamentPopup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addOrnamentBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox ornamentInput;
    }
}