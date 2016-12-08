﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignPatternsStep1
{
    public partial class OrnamentForm : Form
    {
        Shape currentShape;
        Composite currentGroup;
        private Form1 form = Application.OpenForms.Cast<Form>().Last() as Form1;

        public OrnamentForm(Shape s, Composite c)
        {
            InitializeComponent();
            locationCombo.SelectedIndex = 0;
            this.currentShape = s;
            this.currentGroup = c;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (OrnamentTextBox.Text.Equals(""))
            {
                MessageBox.Show("Voer het ornament velt in.");
            }
            else
            {
                if (currentShape == null)
                    addOrnament(null, currentGroup);
                else addOrnament(currentShape, null);
            }
        }
        private void addOrnament(Shape s, Composite c)
        {
            switch (locationCombo.SelectedItem.ToString())
            {
                case "Above":
                    {
                        AboveOrnament aOrnament = new AboveOrnament(s, c,
                            OrnamentTextBox.Text.ToString());
                        CreateLabel(aOrnament, "Above");
                        break;
                    }
                case "Below":
                    {
                        BelowOrnament bOrnament = new BelowOrnament(s, c,
                            OrnamentTextBox.Text.ToString());
                        CreateLabel(bOrnament, "Below");
                        break;
                    }
                case "Left":
                    {
                        LeftOrnament lOrnament = new LeftOrnament(s, c,
                            OrnamentTextBox.Text.ToString());
                        CreateLabel(lOrnament, "Left");
                        break;
                    }
                case "Right":
                    {
                        RightOrnament rOrnament = new RightOrnament(s, c,
                            OrnamentTextBox.Text.ToString());
                        CreateLabel(rOrnament, "Right");
                        break;
                    }
            }
        }

        private void CreateLabel(DecoratorPattern typeOrnament, string type)
        {
            Label OrnamentLabel = new Label();
            OrnamentLabel.Location = typeOrnament.OrnamentLocation;
            OrnamentLabel.Name = type;
            OrnamentLabel.Text = OrnamentTextBox.Text.ToString();
            OrnamentLabel.Size = new Size(30, 15);
            OrnamentLabel.AutoSize = true;

            form.Controls.Add(OrnamentLabel);
            //currentShape.OrnamentList.Add(OrnamentLabel);
            //form.Refresh();
            this.Close();
        }
    }
}
