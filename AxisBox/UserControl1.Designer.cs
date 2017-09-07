namespace AxisBox
{
    partial class AxisBox
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.plotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plotDefineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitTimesToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.fitTargetToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.fitActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllFncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.titleOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.holdOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discreteOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continuousOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.captrueModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.xAxisOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yAxisOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xLabelOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yLabelOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xTickOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yTickOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xTickLabelOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yTickLabelOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.darkThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plotToolStripMenuItem,
            this.fitToolStripMenuItem,
            this.removeAllFncToolStripMenuItem,
            this.toolStripMenuItem1,
            this.titleOnToolStripMenuItem,
            this.gridOnToolStripMenuItem,
            this.holdOnToolStripMenuItem,
            this.discreteOnToolStripMenuItem,
            this.continuousOnToolStripMenuItem,
            this.captrueModeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.xAxisOnToolStripMenuItem,
            this.yAxisOnToolStripMenuItem,
            this.xLogToolStripMenuItem,
            this.yLogToolStripMenuItem,
            this.xLabelOnToolStripMenuItem,
            this.yLabelOnToolStripMenuItem,
            this.xTickOnToolStripMenuItem,
            this.yTickOnToolStripMenuItem,
            this.xTickLabelOnToolStripMenuItem,
            this.yTickLabelOnToolStripMenuItem,
            this.toolStripMenuItem3,
            this.darkThemeToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 484);
            this.contextMenuStrip1.Text = "快捷开关";
            // 
            // plotToolStripMenuItem
            // 
            this.plotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plotDefaultToolStripMenuItem,
            this.plotDefineToolStripMenuItem});
            this.plotToolStripMenuItem.Name = "plotToolStripMenuItem";
            this.plotToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.plotToolStripMenuItem.Text = "函数绘制";
            // 
            // plotDefaultToolStripMenuItem
            // 
            this.plotDefaultToolStripMenuItem.Name = "plotDefaultToolStripMenuItem";
            this.plotDefaultToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.plotDefaultToolStripMenuItem.Text = "画默认函数";
            this.plotDefaultToolStripMenuItem.Click += new System.EventHandler(this.plotDefaultToolStripMenuItem_Click);
            // 
            // plotDefineToolStripMenuItem
            // 
            this.plotDefineToolStripMenuItem.Name = "plotDefineToolStripMenuItem";
            this.plotDefineToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.plotDefineToolStripMenuItem.Text = "按给出数据绘制...";
            this.plotDefineToolStripMenuItem.Click += new System.EventHandler(this.plotDefineToolStripMenuItem_Click);
            // 
            // fitToolStripMenuItem
            // 
            this.fitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fitTimesToolStripComboBox,
            this.fitTargetToolStripComboBox,
            this.toolStripMenuItem4,
            this.fitActionToolStripMenuItem});
            this.fitToolStripMenuItem.Enabled = false;
            this.fitToolStripMenuItem.Name = "fitToolStripMenuItem";
            this.fitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.fitToolStripMenuItem.Text = "函数拟合";
            // 
            // fitTimesToolStripComboBox
            // 
            this.fitTimesToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitTimesToolStripComboBox.Items.AddRange(new object[] {
            "线性回归",
            "曲线拟合"});
            this.fitTimesToolStripComboBox.Name = "fitTimesToolStripComboBox";
            this.fitTimesToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            // 
            // fitTargetToolStripComboBox
            // 
            this.fitTargetToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fitTargetToolStripComboBox.Name = "fitTargetToolStripComboBox";
            this.fitTargetToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.fitTargetToolStripComboBox.Tag = "";
            this.fitTargetToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.fitTargetToolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(178, 6);
            // 
            // fitActionToolStripMenuItem
            // 
            this.fitActionToolStripMenuItem.Name = "fitActionToolStripMenuItem";
            this.fitActionToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.fitActionToolStripMenuItem.Text = "确定拟合";
            this.fitActionToolStripMenuItem.Click += new System.EventHandler(this.fitActionToolStripMenuItem_Click);
            // 
            // removeAllFncToolStripMenuItem
            // 
            this.removeAllFncToolStripMenuItem.Enabled = false;
            this.removeAllFncToolStripMenuItem.Name = "removeAllFncToolStripMenuItem";
            this.removeAllFncToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.removeAllFncToolStripMenuItem.Text = "移除所有函数";
            this.removeAllFncToolStripMenuItem.Click += new System.EventHandler(this.removeAllFncToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(151, 6);
            // 
            // titleOnToolStripMenuItem
            // 
            this.titleOnToolStripMenuItem.Enabled = false;
            this.titleOnToolStripMenuItem.Name = "titleOnToolStripMenuItem";
            this.titleOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.titleOnToolStripMenuItem.Text = "显示标题";
            this.titleOnToolStripMenuItem.Click += new System.EventHandler(this.titleOnToolStripMenuItem_Click);
            // 
            // gridOnToolStripMenuItem
            // 
            this.gridOnToolStripMenuItem.Checked = true;
            this.gridOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gridOnToolStripMenuItem.Enabled = false;
            this.gridOnToolStripMenuItem.Name = "gridOnToolStripMenuItem";
            this.gridOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.gridOnToolStripMenuItem.Text = "显示网格";
            this.gridOnToolStripMenuItem.Click += new System.EventHandler(this.gridOnToolStripMenuItem_Click);
            // 
            // holdOnToolStripMenuItem
            // 
            this.holdOnToolStripMenuItem.Enabled = false;
            this.holdOnToolStripMenuItem.Name = "holdOnToolStripMenuItem";
            this.holdOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.holdOnToolStripMenuItem.Text = "打开多图模式";
            this.holdOnToolStripMenuItem.Click += new System.EventHandler(this.holdOnToolStripMenuItem_Click);
            // 
            // discreteOnToolStripMenuItem
            // 
            this.discreteOnToolStripMenuItem.Enabled = false;
            this.discreteOnToolStripMenuItem.Name = "discreteOnToolStripMenuItem";
            this.discreteOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.discreteOnToolStripMenuItem.Text = "画离散点";
            this.discreteOnToolStripMenuItem.Click += new System.EventHandler(this.discreteOnToolStripMenuItem_Click);
            // 
            // continuousOnToolStripMenuItem
            // 
            this.continuousOnToolStripMenuItem.Checked = true;
            this.continuousOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.continuousOnToolStripMenuItem.Enabled = false;
            this.continuousOnToolStripMenuItem.Name = "continuousOnToolStripMenuItem";
            this.continuousOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.continuousOnToolStripMenuItem.Text = "画连续线";
            this.continuousOnToolStripMenuItem.Click += new System.EventHandler(this.continuousOnToolStripMenuItem_Click);
            // 
            // captrueModeToolStripMenuItem
            // 
            this.captrueModeToolStripMenuItem.Checked = true;
            this.captrueModeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.captrueModeToolStripMenuItem.Enabled = false;
            this.captrueModeToolStripMenuItem.Name = "captrueModeToolStripMenuItem";
            this.captrueModeToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.captrueModeToolStripMenuItem.Text = "打开捕捉模式";
            this.captrueModeToolStripMenuItem.Click += new System.EventHandler(this.captrueModeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(151, 6);
            // 
            // xAxisOnToolStripMenuItem
            // 
            this.xAxisOnToolStripMenuItem.Checked = true;
            this.xAxisOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xAxisOnToolStripMenuItem.Enabled = false;
            this.xAxisOnToolStripMenuItem.Name = "xAxisOnToolStripMenuItem";
            this.xAxisOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.xAxisOnToolStripMenuItem.Text = "显示x轴";
            this.xAxisOnToolStripMenuItem.Click += new System.EventHandler(this.xAxisOnToolStripMenuItem_Click);
            // 
            // yAxisOnToolStripMenuItem
            // 
            this.yAxisOnToolStripMenuItem.Checked = true;
            this.yAxisOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.yAxisOnToolStripMenuItem.Enabled = false;
            this.yAxisOnToolStripMenuItem.Name = "yAxisOnToolStripMenuItem";
            this.yAxisOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.yAxisOnToolStripMenuItem.Text = "显示y轴";
            this.yAxisOnToolStripMenuItem.Click += new System.EventHandler(this.yAxisOnToolStripMenuItem_Click);
            // 
            // xLogToolStripMenuItem
            // 
            this.xLogToolStripMenuItem.Enabled = false;
            this.xLogToolStripMenuItem.Name = "xLogToolStripMenuItem";
            this.xLogToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.xLogToolStripMenuItem.Text = "x轴对数";
            this.xLogToolStripMenuItem.Click += new System.EventHandler(this.xLogToolStripMenuItem_Click);
            // 
            // yLogToolStripMenuItem
            // 
            this.yLogToolStripMenuItem.Enabled = false;
            this.yLogToolStripMenuItem.Name = "yLogToolStripMenuItem";
            this.yLogToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.yLogToolStripMenuItem.Text = "y轴对数";
            this.yLogToolStripMenuItem.Click += new System.EventHandler(this.yLogToolStripMenuItem_Click);
            // 
            // xLabelOnToolStripMenuItem
            // 
            this.xLabelOnToolStripMenuItem.Checked = true;
            this.xLabelOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xLabelOnToolStripMenuItem.Enabled = false;
            this.xLabelOnToolStripMenuItem.Name = "xLabelOnToolStripMenuItem";
            this.xLabelOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.xLabelOnToolStripMenuItem.Text = "显示x标签";
            this.xLabelOnToolStripMenuItem.Click += new System.EventHandler(this.xLabelOnToolStripMenuItem_Click);
            // 
            // yLabelOnToolStripMenuItem
            // 
            this.yLabelOnToolStripMenuItem.Checked = true;
            this.yLabelOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.yLabelOnToolStripMenuItem.Enabled = false;
            this.yLabelOnToolStripMenuItem.Name = "yLabelOnToolStripMenuItem";
            this.yLabelOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.yLabelOnToolStripMenuItem.Text = "显示y标签";
            this.yLabelOnToolStripMenuItem.Click += new System.EventHandler(this.yLabelOnToolStripMenuItem_Click);
            // 
            // xTickOnToolStripMenuItem
            // 
            this.xTickOnToolStripMenuItem.Checked = true;
            this.xTickOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xTickOnToolStripMenuItem.Enabled = false;
            this.xTickOnToolStripMenuItem.Name = "xTickOnToolStripMenuItem";
            this.xTickOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.xTickOnToolStripMenuItem.Text = "显示x刻度";
            this.xTickOnToolStripMenuItem.Click += new System.EventHandler(this.xTickOnToolStripMenuItem_Click);
            // 
            // yTickOnToolStripMenuItem
            // 
            this.yTickOnToolStripMenuItem.Checked = true;
            this.yTickOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.yTickOnToolStripMenuItem.Enabled = false;
            this.yTickOnToolStripMenuItem.Name = "yTickOnToolStripMenuItem";
            this.yTickOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.yTickOnToolStripMenuItem.Text = "显示y刻度";
            this.yTickOnToolStripMenuItem.Click += new System.EventHandler(this.yTickOnToolStripMenuItem_Click);
            // 
            // xTickLabelOnToolStripMenuItem
            // 
            this.xTickLabelOnToolStripMenuItem.Checked = true;
            this.xTickLabelOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xTickLabelOnToolStripMenuItem.Enabled = false;
            this.xTickLabelOnToolStripMenuItem.Name = "xTickLabelOnToolStripMenuItem";
            this.xTickLabelOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.xTickLabelOnToolStripMenuItem.Text = "显示x刻度读数";
            this.xTickLabelOnToolStripMenuItem.Click += new System.EventHandler(this.xTickLabelOnToolStripMenuItem_Click);
            // 
            // yTickLabelOnToolStripMenuItem
            // 
            this.yTickLabelOnToolStripMenuItem.Checked = true;
            this.yTickLabelOnToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.yTickLabelOnToolStripMenuItem.Enabled = false;
            this.yTickLabelOnToolStripMenuItem.Name = "yTickLabelOnToolStripMenuItem";
            this.yTickLabelOnToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.yTickLabelOnToolStripMenuItem.Text = "显示y刻度读数";
            this.yTickLabelOnToolStripMenuItem.Click += new System.EventHandler(this.yTickLabelOnToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(151, 6);
            // 
            // darkThemeToolStripMenuItem
            // 
            this.darkThemeToolStripMenuItem.Enabled = false;
            this.darkThemeToolStripMenuItem.Name = "darkThemeToolStripMenuItem";
            this.darkThemeToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.darkThemeToolStripMenuItem.Text = "黑色主题";
            this.darkThemeToolStripMenuItem.Click += new System.EventHandler(this.darkThemeToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Enabled = false;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.settingsToolStripMenuItem.Text = "其他设置...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // AxisBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(1, 1);
            this.Name = "AxisBox";
            this.Size = new System.Drawing.Size(311, 247);
            this.SizeChanged += new System.EventHandler(this.AxisBox_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AxisBox_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AxisBox_MouseMove);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem plotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gridOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem holdOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discreteOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continuousOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem captrueModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem xAxisOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yAxisOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xLabelOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yLabelOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xTickOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yTickOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xTickLabelOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yTickLabelOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem darkThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAllFncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem titleOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plotDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plotDefineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox fitTimesToolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox fitTargetToolStripComboBox;
        private System.Windows.Forms.ToolStripMenuItem fitActionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;


    }
}
