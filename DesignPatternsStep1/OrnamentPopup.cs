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
    public partial class OrnamentPopup : Form
    {
        Shape currentShape;
        Composite currentGroup;
        bool group;
        private Form1 form = Application.OpenForms.Cast<Form>().Last() as Form1;

        public OrnamentPopup(Shape s, Composite c)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            this.currentShape = s;
            this.currentGroup = c;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (ornamentInput.Text.Equals(""))
            {
                MessageBox.Show("Voer het ornament velt in.");
            }
            else
            {
                switch (comboBox1.SelectedItem.ToString())
                {
                    case "Above":
                        {
                            AboveOrnament aOrnament = new AboveOrnament(currentShape, currentGroup, ornamentInput.Text.ToString());
                            CreateLabel(aOrnament, "Above");
                            break;
                        }
                    case "Below":
                        {
                            BelowOrnament bOrnament = new BelowOrnament(currentShape, currentGroup, ornamentInput.Text.ToString());
                            CreateLabel(bOrnament, "Below");
                            break;
                        }
                    case "Left":
                        {
                            LeftOrnament lOrnament = new LeftOrnament(currentShape, currentGroup, ornamentInput.Text.ToString());
                            CreateLabel(lOrnament, "Left");
                            break;
                        }
                    case "Right":
                        {
                            RightOrnament rOrnament = new RightOrnament(currentShape, currentGroup, ornamentInput.Text.ToString());
                            CreateLabel(rOrnament, "Right");
                            break;
                        }
                }
            }
        }

        private void CreateLabel(DecoratorPattern typeOrnament, string type)
        {
            Label OrnamentLabel = new Label();
            OrnamentLabel.Location = typeOrnament.OrnamentLocation;
            OrnamentLabel.Name = type;
            OrnamentLabel.Text = ornamentInput.Text.ToString();
            OrnamentLabel.Size = new Size(30, 15);
            OrnamentLabel.AutoSize = true;

            form.Controls.Add(OrnamentLabel);
            if (currentShape != null)
                currentShape.OrnamentList.Add(OrnamentLabel);
            else if (currentGroup != null)
                currentGroup.groupOrnaments.Add(OrnamentLabel);
            //form.Refresh();
            this.Close();
        }
    }
}
