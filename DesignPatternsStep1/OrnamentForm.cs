using System;
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
        bool group;
        private Form1 form = Application.OpenForms.Cast<Form>().Last() as Form1;

        public OrnamentForm(Shape s, bool group)
        {
            InitializeComponent();
            locationCombo.SelectedIndex = 0;
            this.currentShape = s;
            this.group = group;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (OrnamentTextBox.Text.Equals(""))
            {
                MessageBox.Show("Voer het ornament velt in.");
            }
            else
            {
                currentShape.OrnamentText = OrnamentTextBox.Text.ToString();
                switch (locationCombo.SelectedItem.ToString()) {
                    case "Above":
                        {
                            AboveOrnament aOrnament = new AboveOrnament(currentShape,
                                OrnamentTextBox.Text.ToString());
                            CreateLabel(aOrnament, "Above");
                            break;
                        }
                    case "Below":
                        {
                            BelowOrnament bOrnament = new BelowOrnament(currentShape,
                                OrnamentTextBox.Text.ToString());
                            CreateLabel(bOrnament, "Below");
                            break;
                        }
                    case "Left":
                        {
                            LeftOrnament lOrnament = new LeftOrnament(currentShape, 
                                OrnamentTextBox.Text.ToString());
                            CreateLabel(lOrnament, "Left");
                            break;
                        }
                    case "Right":
                        {
                            RightOrnament rOrnament = new RightOrnament(currentShape,
                                OrnamentTextBox.Text.ToString());
                            CreateLabel(rOrnament, "Right");
                            break;
                        }                    
                }                
            }
        }

        private void CreateLabel(DecoratorPattern typeOrnament, string type) {
            Label OrnamentLabel = new Label();
            OrnamentLabel.Location = typeOrnament.OrnamentLocation;
            OrnamentLabel.Name = type;
            OrnamentLabel.Text = currentShape.OrnamentText;
            OrnamentLabel.Size = new Size(30, 15);
            OrnamentLabel.AutoSize = true;

            form.Controls.Add(OrnamentLabel);
            currentShape.OrnamentList.Add(OrnamentLabel);      
            //form.Refresh();
            this.Close();
        }
    }
}
