namespace DesignPatternsStep1
{
    partial class OrnamentForm
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
            this.OrnamentTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.locationCombo = new System.Windows.Forms.ComboBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OrnamentTextBox
            // 
            this.OrnamentTextBox.Location = new System.Drawing.Point(12, 24);
            this.OrnamentTextBox.Name = "OrnamentTextBox";
            this.OrnamentTextBox.Size = new System.Drawing.Size(165, 20);
            this.OrnamentTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ornament text";
            // 
            // locationCombo
            // 
            this.locationCombo.FormattingEnabled = true;
            this.locationCombo.Items.AddRange(new object[] {
            "Above",
            "Below",
            "Left",
            "Right"});
            this.locationCombo.Location = new System.Drawing.Point(34, 50);
            this.locationCombo.Name = "locationCombo";
            this.locationCombo.Size = new System.Drawing.Size(121, 21);
            this.locationCombo.TabIndex = 4;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(114, 103);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 5;
            this.saveBtn.Text = "Opslaan";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // OrnamentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 138);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.locationCombo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OrnamentTextBox);
            this.Name = "OrnamentForm";
            this.Text = "Ornament";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox OrnamentTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox locationCombo;
        private System.Windows.Forms.Button saveBtn;
    }
}