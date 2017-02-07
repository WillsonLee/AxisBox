namespace AxisBox
{
    partial class GenerateXY
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateXY));
            this.matrixGenerator1 = new MatrixGenerator.MatrixGenerator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.yValComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.xValComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // matrixGenerator1
            // 
            this.matrixGenerator1.CurrentMatrix = null;
            this.matrixGenerator1.Dock = System.Windows.Forms.DockStyle.Top;
            this.matrixGenerator1.Location = new System.Drawing.Point(0, 0);
            this.matrixGenerator1.MinimumSize = new System.Drawing.Size(352, 252);
            this.matrixGenerator1.Name = "matrixGenerator1";
            this.matrixGenerator1.SelectedIndex = -1;
            this.matrixGenerator1.Size = new System.Drawing.Size(352, 252);
            this.matrixGenerator1.TabIndex = 0;
            this.matrixGenerator1.Workspace = ((System.Collections.Generic.List<MatrixTool.Matrix>)(resources.GetObject("matrixGenerator1.Workspace")));
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.yValComboBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.xValComboBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 258);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(346, 44);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(276, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // yValComboBox
            // 
            this.yValComboBox.FormattingEnabled = true;
            this.yValComboBox.Location = new System.Drawing.Point(154, 12);
            this.yValComboBox.Name = "yValComboBox";
            this.yValComboBox.Size = new System.Drawing.Size(103, 20);
            this.yValComboBox.TabIndex = 3;
            this.yValComboBox.DropDown += new System.EventHandler(this.xValComboBox_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(131, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "y:";
            // 
            // xValComboBox
            // 
            this.xValComboBox.FormattingEnabled = true;
            this.xValComboBox.Location = new System.Drawing.Point(22, 12);
            this.xValComboBox.Name = "xValComboBox";
            this.xValComboBox.Size = new System.Drawing.Size(103, 20);
            this.xValComboBox.TabIndex = 1;
            this.xValComboBox.DropDown += new System.EventHandler(this.xValComboBox_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "x:";
            // 
            // GenerateXY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 302);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.matrixGenerator1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GenerateXY";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成横纵坐标";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MatrixGenerator.MatrixGenerator matrixGenerator1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox yValComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox xValComboBox;
        private System.Windows.Forms.Label label1;
    }
}