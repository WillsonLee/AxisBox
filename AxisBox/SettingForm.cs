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
    /// <summary>
    /// 更多设置弹出窗口
    /// </summary>
    public partial class SettingForm : Form
    {
        /// <summary>
        /// 边框颜色设置
        /// </summary>
        public Color borderColor;
        /// <summary>
        /// 背景色设置
        /// </summary>
        public Color background;
        /// <summary>
        /// 轴网及标签颜色设置
        /// </summary>
        public Color axisAndLabelColor;
        /// <summary>
        /// 曲线颜色
        /// </summary>
        public Color curveColor;
        /// <summary>
        /// 离散点颜色
        /// </summary>
        public Color discretePointColor;
        /// <summary>
        /// 曲线线型
        /// </summary>
        public System.Drawing.Drawing2D.DashStyle curveDashStyle;
        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingForm()
        {
            InitializeComponent();
        }

        public void initializeCurveSelection()
        {
            switch (curveDashStyle)
            {
                case System.Drawing.Drawing2D.DashStyle.Dash: dashRadioButton.Checked = true; break;
                case System.Drawing.Drawing2D.DashStyle.DashDot: dashDotRadioButton.Checked = true; break;
                case System.Drawing.Drawing2D.DashStyle.DashDotDot: dashDotDotRadioButton.Checked = true; break;
                case System.Drawing.Drawing2D.DashStyle.Dot: dotRadioButton.Checked = true; break;
                case System.Drawing.Drawing2D.DashStyle.Solid: solidRadioButton.Checked = true; break;
            }
        }

        private void SettingForm_Deactivate(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void titleTrackBar_ValueChanged(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(titleTrackBar, titleTrackBar.Value.ToString());
        }

        private void xLabelSizeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            toolTip2.SetToolTip(xLabelSizeTrackBar, xLabelSizeTrackBar.Value.ToString());
        }

        private void yLabelSizeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            toolTip3.SetToolTip(yLabelSizeTrackBar, yLabelSizeTrackBar.Value.ToString());
        }

        private void colorDesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (colorDesComboBox.SelectedIndex)
            {
                case 0: colorSetPanel.BackColor = borderColor; break;
                case 1: colorSetPanel.BackColor = background; break;
                case 2: colorSetPanel.BackColor = axisAndLabelColor; break;
                case 3: colorSetPanel.BackColor = curveColor; break;
                case 4: colorSetPanel.BackColor = discretePointColor; break;
            }
        }

        private void colorSetPanel_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                colorSetPanel.BackColor = colorDialog1.Color;
                switch (colorDesComboBox.SelectedIndex)
                {
                    case 0: borderColor = colorSetPanel.BackColor; break;
                    case 1: background = colorSetPanel.BackColor; break;
                    case 2: axisAndLabelColor = colorSetPanel.BackColor; break;
                    case 3: curveColor = colorSetPanel.BackColor; break;
                    case 4: discretePointColor = colorSetPanel.BackColor; break;
                }
            }
        }

        private void dashRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            curveDashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        private void dashDotRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            curveDashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
        }

        private void dashDotDotRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            curveDashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
        }

        private void dotRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            curveDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }

        private void solidRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            curveDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen lineExpPen = new Pen(new SolidBrush(Color.Black));
            lineExpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawLine(lineExpPen, 20, 10, 100, 10);
            lineExpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            g.DrawLine(lineExpPen, 20, 40, 100, 40);
            lineExpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            g.DrawLine(lineExpPen, 20, 70, 100, 70);
            lineExpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawLine(lineExpPen, 20, 100, 100, 100);
            lineExpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            g.DrawLine(lineExpPen, 20, 130, 100, 130);
        }

        private void xMinTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            int input = (int)e.KeyChar;
            //ASCII码表48到57是数字0~9,46是小数点，8是退格
            if ((input < 48 || input > 57) && input != 8 && input != 46 && input != 45)
            {
                e.Handled = true;
            }
            //小数点不能在第一位,第一位只能是数字或负号
            if (((TextBox)sender).Text.Length == 0)
            {
                if (input < 48 || input > 57)
                {
                    if (input != 45)
                    {
                        e.Handled = true;
                    }
                }
            }
            //负号只能出现在第一位
            if (((TextBox)sender).Text.Length != 0)
            {
                if (input == 45)
                {
                    e.Handled = true;
                }
            }
            //之前已经有小数点
            if (input == 46)
            {
                bool dotFlag = false;
                string text = ((TextBox)sender).Text;
                foreach (char one in text)
                {
                    if (one == '.')
                        dotFlag = true;
                }
                e.Handled = dotFlag;
            } 
        }
    }
}
