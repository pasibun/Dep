namespace DesignPatternsStep1
{
    partial class Form1
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
            this.drawEllipseBtn = new System.Windows.Forms.Button();
            this.drawRectangleBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.selectedLabel = new System.Windows.Forms.Label();
            this.noneButton = new System.Windows.Forms.Button();
            this.resize = new System.Windows.Forms.CheckBox();
            this.UndoButton = new System.Windows.Forms.Button();
            this.RedoButton = new System.Windows.Forms.Button();
            this.importBtn = new System.Windows.Forms.Button();
            this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.newGroupBtn = new System.Windows.Forms.Button();
            this.showGroupsBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.exportBtn = new System.Windows.Forms.Button();
            this.moveCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // drawEllipseBtn
            // 
            this.drawEllipseBtn.Location = new System.Drawing.Point(12, 12);
            this.drawEllipseBtn.Name = "drawEllipseBtn";
            this.drawEllipseBtn.Size = new System.Drawing.Size(140, 31);
            this.drawEllipseBtn.TabIndex = 0;
            this.drawEllipseBtn.Text = "Draw Ellipse";
            this.drawEllipseBtn.UseVisualStyleBackColor = true;
            this.drawEllipseBtn.Click += new System.EventHandler(this.drawEllipseBtn_Click);
            // 
            // drawRectangleBtn
            // 
            this.drawRectangleBtn.Location = new System.Drawing.Point(183, 12);
            this.drawRectangleBtn.Name = "drawRectangleBtn";
            this.drawRectangleBtn.Size = new System.Drawing.Size(140, 31);
            this.drawRectangleBtn.TabIndex = 1;
            this.drawRectangleBtn.Text = "Draw Rectangle";
            this.drawRectangleBtn.UseVisualStyleBackColor = true;
            this.drawRectangleBtn.Click += new System.EventHandler(this.drawRectangleBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(527, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Draw:";
            // 
            // selectedLabel
            // 
            this.selectedLabel.AutoSize = true;
            this.selectedLabel.Location = new System.Drawing.Point(600, 19);
            this.selectedLabel.Name = "selectedLabel";
            this.selectedLabel.Size = new System.Drawing.Size(0, 17);
            this.selectedLabel.TabIndex = 3;
            // 
            // noneButton
            // 
            this.noneButton.Location = new System.Drawing.Point(354, 12);
            this.noneButton.Name = "noneButton";
            this.noneButton.Size = new System.Drawing.Size(140, 31);
            this.noneButton.TabIndex = 4;
            this.noneButton.Text = "None";
            this.noneButton.UseVisualStyleBackColor = true;
            this.noneButton.Click += new System.EventHandler(this.noneButton_Click);
            // 
            // resize
            // 
            this.resize.AutoSize = true;
            this.resize.Location = new System.Drawing.Point(14, 97);
            this.resize.Name = "resize";
            this.resize.Size = new System.Drawing.Size(73, 21);
            this.resize.TabIndex = 6;
            this.resize.Text = "Resize";
            this.resize.UseVisualStyleBackColor = true;
            // 
            // UndoButton
            // 
            this.UndoButton.Location = new System.Drawing.Point(12, 169);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(75, 23);
            this.UndoButton.TabIndex = 7;
            this.UndoButton.Text = "Undo";
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // RedoButton
            // 
            this.RedoButton.Location = new System.Drawing.Point(12, 199);
            this.RedoButton.Name = "RedoButton";
            this.RedoButton.Size = new System.Drawing.Size(75, 23);
            this.RedoButton.TabIndex = 8;
            this.RedoButton.Text = "Redo";
            this.RedoButton.UseVisualStyleBackColor = true;
            this.RedoButton.Click += new System.EventHandler(this.RedoButton_Click);
            // 
            // importBtn
            // 
            this.importBtn.Location = new System.Drawing.Point(12, 228);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(75, 23);
            this.importBtn.TabIndex = 9;
            this.importBtn.Text = "Import";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // importFileDialog
            // 
            this.importFileDialog.Filter = "txt files (*.txt)|*.txt";
            // 
            // newGroupBtn
            // 
            this.newGroupBtn.Location = new System.Drawing.Point(12, 49);
            this.newGroupBtn.Name = "newGroupBtn";
            this.newGroupBtn.Size = new System.Drawing.Size(139, 32);
            this.newGroupBtn.TabIndex = 10;
            this.newGroupBtn.Text = "New Group";
            this.newGroupBtn.UseVisualStyleBackColor = true;
            this.newGroupBtn.Click += new System.EventHandler(this.newGroupBtn_Click);
            // 
            // showGroupsBtn
            // 
            this.showGroupsBtn.Location = new System.Drawing.Point(183, 50);
            this.showGroupsBtn.Name = "showGroupsBtn";
            this.showGroupsBtn.Size = new System.Drawing.Size(140, 31);
            this.showGroupsBtn.TabIndex = 11;
            this.showGroupsBtn.Text = "Show Groups";
            this.showGroupsBtn.UseVisualStyleBackColor = true;
            this.showGroupsBtn.Click += new System.EventHandler(this.showGroupsBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(354, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 31);
            this.button1.TabIndex = 12;
            this.button1.Text = "Save Group";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // exportBtn
            // 
            this.exportBtn.Location = new System.Drawing.Point(12, 258);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(75, 23);
            this.exportBtn.TabIndex = 13;
            this.exportBtn.Text = "Export";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // moveCheckBox
            // 
            this.moveCheckBox.AutoSize = true;
            this.moveCheckBox.Location = new System.Drawing.Point(14, 125);
            this.moveCheckBox.Name = "moveCheckBox";
            this.moveCheckBox.Size = new System.Drawing.Size(64, 21);
            this.moveCheckBox.TabIndex = 14;
            this.moveCheckBox.Text = "Move";
            this.moveCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 377);
            this.Controls.Add(this.moveCheckBox);
            this.Controls.Add(this.exportBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.showGroupsBtn);
            this.Controls.Add(this.newGroupBtn);
            this.Controls.Add(this.importBtn);
            this.Controls.Add(this.RedoButton);
            this.Controls.Add(this.UndoButton);
            this.Controls.Add(this.resize);
            this.Controls.Add(this.noneButton);
            this.Controls.Add(this.selectedLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.drawRectangleBtn);
            this.Controls.Add(this.drawEllipseBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button drawEllipseBtn;
        private System.Windows.Forms.Button drawRectangleBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label selectedLabel;
        private System.Windows.Forms.Button noneButton;
        private System.Windows.Forms.CheckBox resize;
        private System.Windows.Forms.Button UndoButton;
        private System.Windows.Forms.Button RedoButton;
        private System.Windows.Forms.Button importBtn;
        private System.Windows.Forms.OpenFileDialog importFileDialog;
        private System.Windows.Forms.Button newGroupBtn;
        private System.Windows.Forms.Button showGroupsBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.CheckBox moveCheckBox;
    }
}

