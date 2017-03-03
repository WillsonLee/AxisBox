using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatrixTool;

namespace AxisBox
{
    public partial class GenerateXY : Form
    {
        public Matrix x = null;
        public Matrix y = null;
        public GenerateXY()
        {
            InitializeComponent();
            matrixGenerator1.SetInitializeFunc(5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (xValComboBox.SelectedIndex != -1 && yValComboBox.SelectedIndex != -1)
            {
                x = matrixGenerator1.Workspace.ElementAt(xValComboBox.SelectedIndex);
                y = matrixGenerator1.Workspace.ElementAt(yValComboBox.SelectedIndex);
            }
            DialogResult = DialogResult.OK;
        }

        private void xValComboBox_DropDown(object sender, EventArgs e)
        {
            ComboBox sender_obj = (ComboBox)sender;
            int count_combo = sender_obj.Items.Count;
            for (int i = 0; i < count_combo; i++)
            {
                sender_obj.Items.RemoveAt(0);
            }
            if (matrixGenerator1.Workspace.Count != 0)
            {
                for (int i = 0; i < matrixGenerator1.Workspace.Count; i++)
                {
                    sender_obj.Items.Add(matrixGenerator1.varNameList[i]);
                }
            }
        }
    }
}
