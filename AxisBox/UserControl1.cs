using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MatrixTool;

namespace AxisBox
{
    /// <summary>
    /// 函数画图板控件
    /// </summary>
    [ToolboxBitmap(typeof(AxisBox),"UserDefinedControl.bmp")]
    public partial class AxisBox: UserControl
    {
        #region 私有字段
        //开关字段
        private bool isPlotted = false;
        private bool xAxis = true;
        private bool yAxis = true;
        private bool xLog = false;
        private bool yLog = false;
        private bool gridOn = true;
        private bool holdOn = false;
        private bool titleOn = false;
        private bool xTickOn = true;
        private bool yTickOn = true;
        private bool xLabelOn = true;
        private bool yLabelOn = true;
        private bool xTickLabelOn = true;
        private bool yTickLabelOn = true;
        private bool continuousOn = true;
        private bool discreteOn = false;
        private bool axisEqual = false;//刻度等比例
        private bool refit = false;//是否把数据点重新拟合过
        private bool captureMode = true;//捕捉状态
        private bool capture = false;//是否捕捉到点
        private bool darkTheme = false;
        private bool systemContextMenuOn = true;//决定是否启用在这里定义的右键菜单还是自定义右键菜单
        //绘图属性
        private string xLabel = "x";
        private string yLabel = "y";
        private int xLabelSize = 12;
        private int yLabelSize = 12;
        private string plotTitle = "新的绘图";
        private int plotTitleSize = 9;
        private Point originPoint;
        private Rectangle plotArea;
        private int marginWidth;
        private int marginHeight;
        private Color borderColor = Color.LightGray;//边框颜色
        private Color backgroundColor = Color.White;//背景颜色
        private Color axisAndLabelColor = Color.Black;//坐标线及标签文本颜色
        private Color lineColor = Color.Blue;//默认图线颜色
        private List<Color> lineColorList = new List<Color>();//图线颜色列表
        private Color samplePointColor = Color.Blue;//默认离散数据点颜色
        private int radiusOfSamplePoint = 4;//默认离散点半径(大小)
        private int fitTimes = 1;//拟合次数
        private System.Drawing.Drawing2D.DashStyle lineDashStyle =System.Drawing.Drawing2D.DashStyle.Solid;//默认为实线
        private List<System.Drawing.Drawing2D.DashStyle> lineDashStyleList = new List<System.Drawing.Drawing2D.DashStyle>();
        private List<Matrix> phisicsX = new List<Matrix>();
        private List<Matrix> phisicsY = new List<Matrix>();
        private List<Matrix> phisicsXReorder = new List<Matrix>();
        private List<Matrix> phisicsYReorder = new List<Matrix>();
        private List<Matrix> phisicsXRepository = new List<Matrix>();//如果重新拟合过数据点可能改变
        private List<Matrix> phisicsYRepository = new List<Matrix>();//用这两个来保存源数据点的数据
        private Point capturePoint = new Point(0, 0);
        private PointF capturePointActual = new PointF(0, 0);
        private int sensitivity = 30;//表示捕捉点的敏感度,默认为半径30像素以内
        private List<int> targetsBeenFitted = new List<int>();//表示哪些曲线已经被拟合过了
        //绘图数据
        private List<Matrix> xVal = new List<Matrix>();
        private List<Matrix> yVal = new List<Matrix>();
        private List<Matrix> xValReorder = new List<Matrix>();
        private List<Matrix> yValReorder = new List<Matrix>();
        private List<Matrix> xValRepository = new List<Matrix>();
        private List<Matrix> yValRepository = new List<Matrix>();
        private double xStep;
        private double yStep;
        private double xMin;
        private double xMax;
        private double yMin;
        private double yMax;
        private double originX;
        private double originY;
        private double xMinRepository;
        private double xMaxRepository;
        private double yMinRepository;
        private double yMaxRepository;
        private double xStepRepository;
        private double yStepRepository;
        #endregion

        #region 访问器
        //在调用Plot函数前,部分属性是不可改的//即尽量任何属性的更改都在使用Plot函数之后
        /// <summary>
        /// x轴是否显示
        /// </summary>
        public bool XAxisOn
        {
            get { return xAxis; }
            set 
            {
                if (isPlotted)
                {
                    xAxis = value;
                    xAxisOnToolStripMenuItem.Checked = XAxisOn;//右键菜单处理
                    if (XAxisOn)
                    {
                        xTickOnToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        xTickOnToolStripMenuItem.Enabled = false;
                    }
                    if (XAxisOn && XTickOn)
                    {
                        xTickLabelOnToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        xTickLabelOnToolStripMenuItem.Enabled = false;
                    }
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// y轴是否显示
        /// </summary>
        public bool YAxisOn
        {
            get { return yAxis; }
            set
            {
                if (isPlotted)
                {
                    yAxis = value;
                    yAxisOnToolStripMenuItem.Checked = YAxisOn;//右键菜单处理
                    if (!yAxisOnToolStripMenuItem.Checked)
                    {
                        yTickOnToolStripMenuItem.Enabled = false;
                        yTickLabelOnToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        yTickOnToolStripMenuItem.Enabled = true;
                        yTickLabelOnToolStripMenuItem.Enabled = true;
                    }
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// x轴是否取对数坐标(注意只有x数据不越过数据0才能转换成对数坐标)
        /// </summary>
        public bool XLog
        {
            get { return xLog; }
            set
            {
                if (isPlotted)
                {
                    if (value)//改成对数坐标
                    {
                        if (!xLog)
                        {
                            if (xMax * xMin > 0)//如果正负坐标都有不能转化为对数坐标
                            {
                                xLog = value;
                                this.XToLog();//更新坐标原点及步距
                                this.refreshParameter();//更新物理坐标参数
                                Invalidate();
                            }
                        }
                    }
                    else//改回线性坐标
                    {
                        if (xLog)
                        {
                            xLog = value;
                            this.XToLinear();
                            this.refreshParameter();
                            Invalidate(); 
                        }
                    }
                    xLogToolStripMenuItem.Checked = XLog;//右键菜单处理
                }
            }
        }
        /// <summary>
        /// y轴是否取对数坐标(注意只有y数据不越过数据0才能转换成对数坐标)
        /// </summary>
        public bool YLog
        {
            get { return yLog; }
            set
            {
                if (isPlotted)
                {
                    if (value)//改成对数坐标
                    {
                        if (yMax * yMin > 0)//如果正负坐标都有不能转化为对数坐标
                        {
                            if (yLog != value)
                            {
                                yLog = value;
                                this.YToLog();
                                this.refreshParameter();
                                Invalidate();
                            }
                        }
                    }
                    else//改回线性坐标
                    {
                        if (yLog!=value)
                        {
                            yLog = value;
                            this.YToLinear();
                            this.refreshParameter();
                            Invalidate(); 
                        }
                    }
                    yLogToolStripMenuItem.Checked = YLog;
                }
            }
        }
        /// <summary>
        /// 网格是否打开
        /// </summary>
        public bool GridOn
        {
            get { return gridOn; }
            set
            {
                if (isPlotted)
                {
                    gridOn = value;
                    gridOnToolStripMenuItem.Checked = GridOn;//右键菜单处理
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 是否允许多图形绘制
        /// </summary>
        public bool HoldOn
        {
            get { return holdOn; }
            set
            {
                if (isPlotted)
                {
                    holdOn = value;
                    holdOnToolStripMenuItem.Checked = HoldOn;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 是否显示标题
        /// </summary>
        public bool TitleOn
        {
            get { return titleOn; }
            set
            {
                if (isPlotted)
                {
                    titleOn = value;
                    titleOnToolStripMenuItem.Checked = TitleOn;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 是否显示x轴标签
        /// </summary>
        public bool XLabelOn
        {
            get { return xLabelOn; }
            set
            {
                if (isPlotted)
                {
                    xLabelOn = value;
                    xLabelOnToolStripMenuItem.Checked = XLabelOn;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 是否显示y轴标签
        /// </summary>
        public bool YLabelOn
        {
            get { return yLabelOn; }
            set
            {
                if (isPlotted)
                {
                    yLabelOn = value;
                    yLabelOnToolStripMenuItem.Checked = YLabelOn;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 是否显示x轴刻度
        /// </summary>
        public bool XTickOn
        {
            get { return xTickOn; }
            set
            {
                xTickOn = value;
                if (isPlotted)
                {
                    Invalidate();
                }
                xTickOnToolStripMenuItem.Checked = XTickOn;//右键菜单处理
                if (XTickOn && XAxisOn)
                {
                    xTickLabelOnToolStripMenuItem.Enabled = true;
                }
                else
                {
                    xTickLabelOnToolStripMenuItem.Enabled = false;
                }
            }
        }
        /// <summary>
        /// 是否显示y轴刻度
        /// </summary>
        public bool YTickOn
        {
            get { return yTickOn; }
            set
            {
                yTickOn = value;
                if (isPlotted)
                {
                    Invalidate();
                }
                yTickOnToolStripMenuItem.Checked = YTickOn;//右键菜单处理
                if (YTickOn && YAxisOn)
                {
                    yTickLabelOnToolStripMenuItem.Enabled = true;
                }
                else
                {
                    yTickLabelOnToolStripMenuItem.Enabled = false;
                }
            }
        }
        /// <summary>
        /// 是否显示x刻度读数
        /// </summary>
        public bool XTickLabelOn
        {
            get { return xTickLabelOn; }
            set
            {
                xTickLabelOn = value;
                if (isPlotted)
                {
                    Invalidate();
                }
                xTickLabelOnToolStripMenuItem.Checked = XTickLabelOn;
            }
        }
        /// <summary>
        /// 是否显示y轴刻度读数
        /// </summary>
        public bool YTickLabelOn
        {
            get { return yTickLabelOn; }
            set
            {
                yTickLabelOn = value;
                if (isPlotted)
                {
                    Invalidate();
                }
                yTickLabelOnToolStripMenuItem.Checked = YTickLabelOn;
            }
        }
        /// <summary>
        /// 是否打开坐标点捕捉模式
        /// </summary>
        public bool CaptureMode
        {
            get { return captureMode; }
            set
            {
                if (isPlotted)
                {
                    captureMode = value;
                    captrueModeToolStripMenuItem.Checked = CaptureMode;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 是否启用黑色主题模式
        /// </summary>
        public bool DarkTheme
        {
            get { return darkTheme; }
            set
            {
                darkTheme = value;
                if (value)
                {
                    borderColor = Color.Black;
                    backgroundColor = Color.Black;
                    axisAndLabelColor = Color.Yellow;
                }
                else
                {
                    borderColor = Color.LightGray;
                    backgroundColor = Color.White;
                    axisAndLabelColor = Color.Black;
                }
                if (isPlotted)
                {
                    Invalidate();
                }
                darkThemeToolStripMenuItem.Checked = DarkTheme;
            }
        }
        /// <summary>
        /// 是否画出连续曲线
        /// </summary>
        public bool ContinuousOn
        {
            get { return continuousOn; }
            set
            {
                if (isPlotted)
                {
                    continuousOn = value;
                    continuousOnToolStripMenuItem.Checked = ContinuousOn;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 是否画出离散数据点
        /// </summary>
        public bool DiscreteOn
        {
            get { return discreteOn; }
            set
            {
                if (isPlotted)
                {
                    discreteOn = value;
                    discreteOnToolStripMenuItem.Checked = DiscreteOn;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 是否使用控件自带右键菜单
        /// </summary>
        public bool UsingSystemContextMenu
        {
            get { return systemContextMenuOn; }
            set
            {
                systemContextMenuOn = value;
                if (value)
                {
                    ContextMenuStrip = contextMenuStrip1;
                }
                else
                {
                    ContextMenuStrip = null;
                }
            }
        }
        /// <summary>
        /// 是否横纵坐标等比例
        /// </summary>
        public bool AxisEqual
        {
            get { return axisEqual; }
            set
            {
                if (isPlotted)
                {
                    axisEqual = value;
                    if (value)//等比例
                    {
                        if (!xLog)//线性坐标才可以使x、y等比例
                        {
                            double xRange = xMaxRepository - xMinRepository;
                            double yRange = yMaxRepository - yMinRepository;
                            double plotRatio = (double)plotArea.Width / plotArea.Height;
                            if ((xRange / yRange) > plotRatio)
                            {
                                yRange = xRange / plotRatio;
                                yMax = yMin + yRange;
                                xStep = xStepRepository;
                                yStep = xStepRepository;
                            }
                            else
                            {
                                xRange = yRange * plotRatio;
                                xMax = xMin + xRange;
                                xStep = yStepRepository;
                                yStep = yStepRepository;
                            }
                            refreshParameter();
                            Invalidate();
                        }
                    }
                    else//非等比例
                    {
                        if (xLog)
                        {
                            XToLog();
                        }
                        else
                        {
                            XToLinear();
                        }
                        if (yLog)
                        {
                            YToLog();
                        }
                        else
                        {
                            YToLinear();
                        }
                        refreshParameter();
                        Invalidate();
                    } 
                }
            }
        }
        /// <summary>
        /// x轴标签内容
        /// </summary>
        public string XLabel
        {
            get { return xLabel; }
            set
            {
                if (isPlotted)
                {
                    xLabel = value;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// y轴标签内容
        /// </summary>
        public string YLabel
        {
            get { return yLabel; }
            set
            {
                if (isPlotted)
                {
                    yLabel = value;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 标题内容
        /// </summary>
        public string PlotTitle
        {
            get { return plotTitle; }
            set
            {
                if (isPlotted)
                {
                    plotTitle = value;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// x轴标签文本字体大小
        /// </summary>
        public int XLabelSize
        {
            get { return xLabelSize; }
            set
            {
                if (isPlotted)
                {
                    xLabelSize = value;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// y标签文本字体大小
        /// </summary>
        public int YLabelSize
        {
            get { return yLabelSize; }
            set
            {
                if (isPlotted)
                {
                    yLabelSize = value;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 标题文本字体大小
        /// </summary>
        public int PlotTitleSize
        {
            get { return plotTitleSize; }
            set
            {
                if (isPlotted)
                {
                    plotTitleSize = value;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 离散数据点绘制半径
        /// </summary>
        public int RadiusOfSamplePoint
        {
            get { return radiusOfSamplePoint; }
            set
            {
                if (isPlotted)
                {
                    radiusOfSamplePoint = value;
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// 进行曲线拟合的拟合次数
        /// </summary>
        public int FitTimes
        {
            get { return fitTimes; }
            set
            {
                if (isPlotted)
                {
                    fitTimes = value;
                }
            }
        }
        /// <summary>
        /// x轴分度值
        /// </summary>
        public double XStep
        {
            get { return xStep; }
            set
            {
                if (isPlotted)
                {
                    xStep = value;
                    refreshParameter();
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// y轴分度值
        /// </summary>
        public double YStep
        {
            get { return yStep; }
            set
            {
                if (isPlotted)
                {
                    yStep = value;
                    refreshParameter();
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// x最小值
        /// </summary>
        public double XMin
        {
            get { return xMin; }
            set
            {
                if (isPlotted)
                {
                    if (value<xMax)
                    {
                        xMin = value;
                        originX = xMin;
                        refreshParameter();
                        Invalidate(); 
                    }
                }
            }
        }
        /// <summary>
        /// x最大值
        /// </summary>
        public double XMax
        {
            get { return xMax; }
            set
            {
                if (isPlotted)
                {
                    if (value>xMin)
                    {
                        xMax = value;
                        refreshParameter();
                        Invalidate(); 
                    }
                }
            }
        }
        /// <summary>
        /// y最小值
        /// </summary>
        public double YMin
        {
            get { return yMin; }
            set
            {
                if (isPlotted)
                {
                    if (value<yMax)
                    {
                        yMin = value;
                        originY = yMin;
                        refreshParameter();
                        Invalidate(); 
                    }
                }
            }
        }
        /// <summary>
        /// y最大值
        /// </summary>
        public double YMax
        {
            get { return yMax; }
            set
            {
                if (isPlotted)
                {
                    if (value>yMin)
                    {
                        yMax = value;
                        refreshParameter();
                        Invalidate(); 
                    }
                }
            }
        }
        /// <summary>
        /// 函数曲线颜色
        /// </summary>
        public Color CurveColor
        {
            get { return lineColor; }
            set
            {
                if (isPlotted)
                {
                    lineColor = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 离散数据点的绘制颜色
        /// </summary>
        public Color SamplePointColor
        {
            get { return samplePointColor; }
            set
            {
                if (isPlotted)
                {
                    samplePointColor = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (isPlotted)
                {
                    borderColor = value;
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                if (isPlotted)
                {
                    backgroundColor = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 坐标轴、文本标签和网格颜色
        /// </summary>
        public Color AxisAndLabelColor
        {
            get { return axisAndLabelColor; }
            set
            {
                if (isPlotted)
                {
                    axisAndLabelColor = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// 函数曲线线型
        /// </summary>
        public System.Drawing.Drawing2D.DashStyle CurveDashStyle
        {
            get { return lineDashStyle; }
            set
            {
                if (isPlotted)
                {
                    lineDashStyle = value;
                    Invalidate();
                }
            }
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 构造器
        /// </summary>
        public AxisBox()
        {
            InitializeComponent();
            marginWidth = (int)(this.Width / 20);
            marginHeight = (int)(this.Height / 20);
            plotArea = new Rectangle(marginWidth, marginHeight, 
                                        this.Width - 2 * marginWidth, this.Height - 2 * marginHeight);
        }
        /// <summary>
        /// 在AxisBox上画出默认的y=x图像(x范围0-10,步距为1,,11个数据点)
        /// </summary>
        public void Plot()
        {
            Matrix xValue = Matrix.RangeVector(1, 10);
            Matrix yValue = Matrix.RangeVector(1, 10);
            this.Plot(xValue, yValue);
        }
        /// <summary>
        /// 在AxisBox上由给出x、y坐标画出曲线
        /// </summary>
        /// <param name="x">x行向量</param>
        /// <param name="y">y行向量</param>
        public void Plot(Matrix x, Matrix y)
        {
            isPlotted = true;
            System.Drawing.Drawing2D.DashStyle dashTemp;
            switch (lineDashStyle)
            {
                case System.Drawing.Drawing2D.DashStyle.Dash: dashTemp = System.Drawing.Drawing2D.DashStyle.Dash; break;
                case System.Drawing.Drawing2D.DashStyle.DashDot: dashTemp = System.Drawing.Drawing2D.DashStyle.DashDot; break;
                case System.Drawing.Drawing2D.DashStyle.DashDotDot: dashTemp = System.Drawing.Drawing2D.DashStyle.DashDotDot; break;
                case System.Drawing.Drawing2D.DashStyle.Dot: dashTemp = System.Drawing.Drawing2D.DashStyle.Dot; break;
                case System.Drawing.Drawing2D.DashStyle.Solid: dashTemp = System.Drawing.Drawing2D.DashStyle.Solid; break;
                default: dashTemp = System.Drawing.Drawing2D.DashStyle.Solid; break;
            }
            if (holdOn)//是否追加画图还是覆盖画图
            {
                xVal.Add(x);
                yVal.Add(y);
                lineColorList.Add(Color.FromArgb(lineColor.ToArgb()));
                lineDashStyleList.Add(dashTemp);
            }
            else
            {
                if (xVal.Count == 0)
                {
                    xVal.Add(x);
                    yVal.Add(y);
                    lineColorList.Add(Color.FromArgb(lineColor.ToArgb()));
                    lineDashStyleList.Add(dashTemp);
                }
                else
                {
                    xVal.RemoveRange(0, xVal.Count);
                    yVal.RemoveRange(0, yVal.Count);
                    lineColorList.RemoveRange(0, lineColorList.Count);
                    lineDashStyleList.RemoveRange(0, lineDashStyleList.Count);
                    xVal.Add(x);
                    yVal.Add(y);
                    lineColorList.Add(Color.FromArgb(lineColor.ToArgb()));
                    lineDashStyleList.Add(dashTemp);
                }
            }
            if (!xLog)
            {
                this.XToLinear();//默认线性坐标
            }
            else
            {
                this.XToLog();
            }
            if (!yLog)
            {
                this.YToLinear();
            }
            else
            {
                this.YToLog();
            }
            xMinRepository = xMin;
            xMaxRepository = xMax;
            yMinRepository = yMin;
            yMaxRepository = yMax;
            xStepRepository = xStep;
            yStepRepository = yStep;
            this.getReorderedVersion();//为了提高鼠标移动捕捉点的效率,需要获得重新排列后的坐标点,方便查找
            if (!refit)
            {
                xValRepository.RemoveRange(0, xValRepository.Count);
                yValRepository.RemoveRange(0, yValRepository.Count);
                for (int i = 0; i < xValReorder.Count; i++)
                {
                    xValRepository.Add(new Matrix(xValReorder.ElementAt(i)));
                    yValRepository.Add(new Matrix(yValReorder.ElementAt(i)));
                }
            }
            refreshParameter();
            #region 激活右键菜单相应功能
            fitToolStripMenuItem.Enabled = true;
            removeAllFncToolStripMenuItem.Enabled = true;
            gridOnToolStripMenuItem.Enabled = true;
            holdOnToolStripMenuItem.Enabled = true;
            discreteOnToolStripMenuItem.Enabled = true;
            continuousOnToolStripMenuItem.Enabled = true;
            captrueModeToolStripMenuItem.Enabled = true;
            titleOnToolStripMenuItem.Enabled = true;
            xAxisOnToolStripMenuItem.Enabled = true;
            yAxisOnToolStripMenuItem.Enabled = true;
            xLogToolStripMenuItem.Enabled = true;
            yLogToolStripMenuItem.Enabled = true;
            xLabelOnToolStripMenuItem.Enabled = true;
            yLabelOnToolStripMenuItem.Enabled = true;
            xTickLabelOnToolStripMenuItem.Enabled = true;
            yTickLabelOnToolStripMenuItem.Enabled = true;
            xTickOnToolStripMenuItem.Enabled = true;
            yTickOnToolStripMenuItem.Enabled = true;
            darkThemeToolStripMenuItem.Enabled = true;
            //拟合菜单项设置
            int fitTargetItemsCount=fitTargetToolStripComboBox.Items.Count;
            for (int i = 0; i < fitTargetItemsCount; i++)
            {
                fitTargetToolStripComboBox.Items.RemoveAt(0);
            }
            for (int i = 0; i < xVal.Count; i++)
            {
                fitTargetToolStripComboBox.Items.Add(i.ToString());
            }
            fitTimesToolStripComboBox.SelectedIndex = 0;
            fitTargetToolStripComboBox.SelectedIndex = 0;
            #endregion
        }
        /// <summary>
        /// 在AxisBox上由给出x、y坐标画出曲线
        /// </summary>
        /// <param name="xArray">一维数组</param>
        /// <param name="yArray">一维数组</param>
        public void Plot(double[] xArray, double[] yArray)
        {
            Matrix x = new Matrix(xArray);
            Matrix y = new Matrix(yArray);
            this.Plot(x, y);
        }
        /// <summary>
        /// 在AxisBox上由给出x、y坐标画出曲线
        /// </summary>
        /// <param name="x">x行向量</param>
        /// <param name="y">y行向量</param>
        /// <param name="curveColor">曲线颜色</param>
        public void Plot(Matrix x, Matrix y, Color curveColor)
        {
            this.Plot(x, y);
            this.ChangeColor(xVal.Count - 1, curveColor);
        }
        /// <summary>
        /// 在AxisBox上由给出x、y坐标画出曲线
        /// </summary>
        /// <param name="x">x行向量</param>
        /// <param name="y">y行向量</param>
        /// <param name="curveStyle">曲线线型</param>
        public void Plot(Matrix x, Matrix y, System.Drawing.Drawing2D.DashStyle curveStyle)
        {
            this.Plot(x, y);
            this.ChangeLineStyle(xVal.Count - 1, curveStyle);
        }
        /// <summary>
        /// 在AxisBox上由给出x、y坐标画出曲线
        /// </summary>
        /// <param name="x">x行向量</param>
        /// <param name="y">y行向量</param>
        /// <param name="curveColor">曲线颜色</param>
        /// <param name="curveStyle">曲线线型</param>
        public void Plot(Matrix x, Matrix y, Color curveColor, System.Drawing.Drawing2D.DashStyle curveStyle)
        {
            this.Plot(x, y);
            this.ChangeColor(xVal.Count - 1, curveColor);
            this.ChangeLineStyle(xVal.Count - 1, curveStyle);
        }
        /// <summary>
        /// 移去所有已画函数
        /// </summary>
        public void RemoveAllFnc()
        {
            xVal.RemoveRange(0, xVal.Count);
            yVal.RemoveRange(0, yVal.Count);
            xValReorder.RemoveRange(0, xValReorder.Count);
            yValReorder.RemoveRange(0, yValReorder.Count);
            xValRepository.RemoveRange(0, xValRepository.Count);
            yValRepository.RemoveRange(0, yValRepository.Count);
            lineColorList.RemoveRange(0, lineColorList.Count);
            lineDashStyleList.RemoveRange(0, lineDashStyleList.Count);
            isPlotted = false;
            refit = false;
            refreshParameter();
            #region 使右键菜单相应功能失效
            fitToolStripMenuItem.Enabled = false;
            removeAllFncToolStripMenuItem.Enabled = false;
            gridOnToolStripMenuItem.Enabled = false;
            holdOnToolStripMenuItem.Enabled = false;
            discreteOnToolStripMenuItem.Enabled = false;
            continuousOnToolStripMenuItem.Enabled = false;
            captrueModeToolStripMenuItem.Enabled = false;
            titleOnToolStripMenuItem.Enabled = false;
            xAxisOnToolStripMenuItem.Enabled = false;
            yAxisOnToolStripMenuItem.Enabled = false;
            xLogToolStripMenuItem.Enabled = false;
            yLogToolStripMenuItem.Enabled = false;
            xLabelOnToolStripMenuItem.Enabled = false;
            yLabelOnToolStripMenuItem.Enabled = false;
            xTickLabelOnToolStripMenuItem.Enabled = false;
            yTickLabelOnToolStripMenuItem.Enabled = false;
            xTickOnToolStripMenuItem.Enabled = false;
            yTickOnToolStripMenuItem.Enabled = false;
            darkThemeToolStripMenuItem.Enabled = false;
            #endregion
            plotToolStripMenuItem.Enabled = true;
            targetsBeenFitted.RemoveRange(0, targetsBeenFitted.Count);
        }
        /// <summary>
        /// 拟合曲线
        /// 如果要画多曲线需要先把所有曲线画出来然后再拟合;
        /// 每条曲线拟合一次,不要对已经拟合过的曲线再次拟合,否则可能有不可预知错误
        /// </summary>
        /// <param name="index">拟合的是第几组数据(默认是第一组)</param>
        /// <returns></returns>
        public bool Fit(int index = 0)
        {
            if (fitTimes <= 0)
                return false;
            if (fitTimes >= 3)
                return false;
            if (index < 0 || index >= xVal.Count)
                return false;
            if (index < 0 || index >= phisicsXRepository.Count)
                return false;
            refit = true;//表示已经被重新拟合过了
            double xMinTemp = xValReorder.ElementAt(index)[0, 0];
            double xMaxTemp = xValReorder.ElementAt(index)[0, xValReorder.ElementAt(index).Columns - 1];
            double increment = Math.Pow(10, Math.Round(Math.Log10(xMaxTemp - xMinTemp), MidpointRounding.AwayFromZero) - 3);
            #region 一次拟合
            //其实是线性回归
            if (fitTimes == 1)
            {
                if (xVal.ElementAt(index).Columns < 2)//一次拟合起码要有两个点
                    return false;
                Matrix xToReplace = Matrix.RangeVector(xValReorder.ElementAt(index)[0, 0], increment,
                                     xValReorder.ElementAt(index)[0, xValReorder.ElementAt(index).Columns - 1]);
                Matrix curvePara = LinearFit(index);
                Matrix yToReplace = curvePara[0, 1] * xToReplace + curvePara[0, 0];
                xVal[index] = xToReplace;
                yVal[index] = yToReplace;
                getReorderedVersion();
                refreshParameter();
            }
            #endregion
            #region 非常规二次拟合部分
            //其实更类似三次拟合,因为二次函数的参数都根据各阶段自变量来调整过了
            else
            {
                if (xVal.ElementAt(index).Columns < 3)//二次拟合起码要有三个点
                    return false;
                Matrix curvePara = this.SquareFit(index);
                Matrix xToReplace = null;
                Matrix yToReplace = null;
                for (int i = 0; i < xValReorder.ElementAt(index).Columns - 1; i++)
                {
                    Matrix xPhaseTemp = Matrix.RangeVector(xValReorder.ElementAt(index)[0, i], 
                                                            increment, xValReorder.ElementAt(index)[0, i + 1]);
                    Matrix yPhaseTemp;
                    if (i == 0)
                    {
                        yPhaseTemp = curvePara[i, 0] * Matrix.Pow(xPhaseTemp, 2) + 
                                        curvePara[i, 1] * xPhaseTemp + curvePara[i, 2];
                    }
                    else if (i == xValReorder.ElementAt(index).Columns - 2)
                    {
                        yPhaseTemp = curvePara[i - 1, 0] * Matrix.Pow(xPhaseTemp, 2) +
                                        curvePara[i - 1, 1] * xPhaseTemp + curvePara[i - 1, 2];
                    }
                    else
                    {
                        Matrix ratioLeft;
                        Matrix ratioRight;
                        ratioLeft = (xValReorder.ElementAt(index)[0, i + 1] - xPhaseTemp) /
                                        (xValReorder.ElementAt(index)[0, i + 1] - xValReorder.ElementAt(index)[0, i]);
                        ratioRight = 1 - ratioLeft;
                        Matrix paraA = curvePara[i - 1, 0] * ratioLeft + curvePara[i, 0] * ratioRight;
                        Matrix paraB = curvePara[i - 1, 1] * ratioLeft + curvePara[i, 1] * ratioRight;
                        Matrix paraC = curvePara[i - 1, 2] * ratioLeft + curvePara[i, 2] * ratioRight;
                        yPhaseTemp = Matrix.DotMultiple(paraA, Matrix.Pow(xPhaseTemp, 2)) +
                                                Matrix.DotMultiple(paraB, xPhaseTemp) + paraC;
                    }
                    xToReplace = Matrix.ConcatenateByRow(xToReplace, xPhaseTemp);
                    yToReplace = Matrix.ConcatenateByRow(yToReplace, yPhaseTemp);
                }
                xVal[index] = xToReplace;
                yVal[index] = yToReplace;
                getReorderedVersion();
                refreshParameter();
            }
            #endregion
            //关于右键菜单的处理
            plotToolStripMenuItem.Enabled = false;
            targetsBeenFitted.Add(index);
            if (fitTargetToolStripComboBox.SelectedIndex==index)
            {
                fitActionToolStripMenuItem.Enabled = false; 
            }
            //finish
            return true;
        }
        /// <summary>
        /// 设置曲线颜色
        /// </summary>
        /// <param name="dstCurve">要设置的目标曲线序号</param>
        /// <param name="colorToChange">要设置成的颜色</param>
        public void ChangeColor(int dstCurve, Color colorToChange)
        {
            if (dstCurve >= lineColorList.Count)
                return;
            else if (dstCurve < 0)
                throw new Exception("要更改颜色的目标曲线序号小于0越界");
            lineColorList[dstCurve] = colorToChange;
            lineColor = Color.FromArgb(colorToChange.ToArgb());//同时改变默认颜色偏好
        }
        /// <summary>
        /// 设置曲线线型
        /// </summary>
        /// <param name="dstCurve">要设置的目标曲线序号</param>
        /// <param name="dashStyleToChange">要设置成的线型</param>
        public void ChangeLineStyle(int dstCurve, System.Drawing.Drawing2D.DashStyle dashStyleToChange)
        {
            if (dstCurve >= lineColorList.Count)
                return;
            else if (dstCurve < 0)
                throw new Exception("要更改颜色的目标曲线序号小于0越界");
            lineDashStyleList[dstCurve] = dashStyleToChange;
            switch (dashStyleToChange)//同时改变默认线性偏好
            {
                case System.Drawing.Drawing2D.DashStyle.Dash:
                    lineDashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                case System.Drawing.Drawing2D.DashStyle.DashDot:
                    lineDashStyle = System.Drawing.Drawing2D.DashStyle.DashDot; break;
                case System.Drawing.Drawing2D.DashStyle.DashDotDot:
                    lineDashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot; break;
                case System.Drawing.Drawing2D.DashStyle.Dot:
                    lineDashStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
                case System.Drawing.Drawing2D.DashStyle.Solid:
                    lineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                default:
                    lineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
            } 
        }
        #endregion

        #region 辅助函数
        private Matrix ToPhysicsX(Matrix xCoordinate)
        {
            Matrix result = Matrix.Zeros(xCoordinate.Rows, xCoordinate.Columns);
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    result[i, j] = this.ToPhysicsX(xCoordinate[i, j]);
                }
            }
            return result;
        }
        private Matrix ToPhysicsY(Matrix yCoordinate)
        {
            Matrix result = Matrix.Zeros(yCoordinate.Rows, yCoordinate.Columns);
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    result[i, j] = this.ToPhysicsY(yCoordinate[i, j]);
                }
            }
            return result;
        }
        private double ToPhysicsX(double xCoordinate)
        {
            double result;
            if (!xLog)
            {
                result = (xCoordinate - xMin) / (xMax - xMin) * plotArea.Width + marginWidth;
            }
            else
            {
                double logRange = Math.Log10(xMax) - Math.Log10(xMin);
                double leftLength = Math.Log10(xCoordinate) - Math.Log10(xMin);
                result = leftLength / logRange * plotArea.Width + marginWidth;
            }
            return result;
        }
        private double ToPhysicsY(double yCoordinate)
        {
            double result;
            if (!yLog)
            {
                result = (yMax - yCoordinate) / (yMax - yMin) * plotArea.Height + marginHeight;
            }
            else
            {
                double logRange = Math.Log10(yMax) - Math.Log10(yMin);
                double upHeight = Math.Log10(yMax) - Math.Log10(yCoordinate);
                result = upHeight / logRange * plotArea.Height + marginHeight;
            }
            return result;
        }
        private double ToActualX(int xCoordinate)
        {
            double result;
            if (!xLog)
            {
                result = (xCoordinate - marginWidth) / plotArea.Width * (xMax - xMin) + xMin;
            }
            else
            {
                double logRange = Math.Log10(xMax) - Math.Log10(xMin);
                result = Math.Pow(10, (xCoordinate - marginWidth) / plotArea.Width * logRange + Math.Log10(xMin));
            }
            return result;
        }
        private double ToActualY(int yCoordinate)
        {
            double result;
            if (!yLog)
            {
                result = (this.Height - yCoordinate - marginHeight) / plotArea.Height * (yMax - yMin) + yMin;
            }
            else
            {
                double logRange = Math.Log10(yMax) - Math.Log10(yMin);
                result = Math.Pow(10, (this.Height - yCoordinate) / plotArea.Height * logRange + Math.Log10(yMin));
            }
            return result;
        }
        private int[] FindIndex(Matrix x, double y)
        {
            //返回两个表示y在x中的下标范围的index
            int left = 0;
            int right = x.Columns - 1;
            while (right > (left + 1))
            {
                if (x[0, left] == y)
                    return new int[2] { left, left };
                if (x[0, right] == y)
                    return new int[2] { right, right };
                int middle = (int)((left + right) / 2);
                if ((x[0, left] - y) * (x[0, middle] - y) < 0)
                {
                    right = middle;
                }
                else
                {
                    left = middle;
                }
            }
            return new int[2] { left, right };
        }
        private void getReorderedVersion()//按照x从小到大排列,同时相应的改变y的排列
        {
            xValReorder.RemoveRange(0, xValReorder.Count);
            yValReorder.RemoveRange(0, yValReorder.Count);
            for (int i = 0; i < xVal.Count; i++)
            {
                xValReorder.Add(new Matrix(xVal.ElementAt(i)));
                yValReorder.Add(new Matrix(yVal.ElementAt(i)));
                for (int j = 1; j < xValReorder.ElementAt(i).Columns; j++)
                {
                    for (int k = 0; k < xValReorder.ElementAt(i).Columns - j; k++)
                    {
                        if (xValReorder.ElementAt(i)[0, k] > xValReorder.ElementAt(i)[0, k + 1])
                        {
                            xValReorder.ElementAt(i)[0, k] = xValReorder.ElementAt(i)[0, k + 1] +
                                                                xValReorder.ElementAt(i)[0, k];
                            xValReorder.ElementAt(i)[0, k + 1] = xValReorder.ElementAt(i)[0, k] -
                                                                xValReorder.ElementAt(i)[0, k + 1];
                            xValReorder.ElementAt(i)[0, k] = xValReorder.ElementAt(i)[0, k] -
                                                                xValReorder.ElementAt(i)[0, k + 1];
                            yValReorder.ElementAt(i)[0, k] = yValReorder.ElementAt(i)[0, k] +
                                                                yValReorder.ElementAt(i)[0, k + 1];
                            yValReorder.ElementAt(i)[0, k + 1] = yValReorder.ElementAt(i)[0, k] -
                                                                    yValReorder.ElementAt(i)[0, k + 1];
                            yValReorder.ElementAt(i)[0, k] = yValReorder.ElementAt(i)[0, k] -
                                                                yValReorder.ElementAt(i)[0, k + 1];
                        }
                    }
                }
            }
        }
        //对数坐标时只要改变一下原点和步距再刷新下数据就可以
        private void XToLog()
        {
            xMin = Matrix.MinOfRow(xVal.ElementAt(0))[0, 0];
            xMax = Matrix.MaxOfRow(xVal.ElementAt(0))[0, 0];
            if (xVal.Count != 1)
            {
                for (int i = 1; i < xVal.Count; i++)
                {
                    double min1 = Matrix.MinOfRow(xVal.ElementAt(i))[0, 0];
                    double max1 = Matrix.MaxOfRow(xVal.ElementAt(i))[0, 0];
                    if (xMin > min1)
                        xMin = min1;
                    if (xMax < max1)
                        xMax = max1;
                }
            }
            double xMinReal = xMin;
            double xMaxReal = xMax;
            xMin = Math.Pow(10, Math.Floor(Math.Log10(xMin)));
            xMax = Math.Pow(10, Math.Ceiling(Math.Log10(xMax)));
            if (xMin == xMax)
            {
                xMin = Math.Pow(10, Math.Log10(xMin) - 1);
                xMax = Math.Pow(10, Math.Log10(xMax) + 1);
            }
            double xRange = xMax - xMin;
            xStep = Math.Pow(10, Math.Round(Math.Log10(xRange / 10), MidpointRounding.AwayFromZero));
            while (xMin <= (xMinReal - xStep))
            {
                xMin = xMin + xStep;
            }
            while (xMax >= (xMaxReal + xStep))
            {
                xMax = xMax - xStep;
            }
            if ((xMax - xMin) / xStep < 4)
                xStep = xStep / 10;
            while (xMin <= (xMinReal - xStep))
            {
                xMin = xMin + xStep;
            }
            while (xMax >= (xMaxReal + xStep))
            {
                xMax = xMax - xStep;
            }
            originX = xMin;
        }
        private void YToLog()
        {
            yMin = Matrix.MinOfRow(yVal.ElementAt(0))[0, 0];
            yMax = Matrix.MaxOfRow(yVal.ElementAt(0))[0, 0];
            if (yVal.Count != 1)
            {
                for (int i = 1; i < yVal.Count; i++)
                {
                    double min2 = Matrix.MinOfRow(yVal.ElementAt(i))[0, 0];
                    double max2 = Matrix.MaxOfRow(yVal.ElementAt(i))[0, 0];
                    if (yMin > min2)
                        yMin = min2;
                    if (yMax < max2)
                        yMax = max2;
                }
            }
            double yMinReal = yMin;
            double yMaxReal = yMax;
            yMin = Math.Pow(10, Math.Floor(Math.Log10(yMin)));
            yMax = Math.Pow(10, Math.Ceiling(Math.Log10(yMax)));
            if (yMin == yMax)//说明在画平行于x轴的直线
            {
                yMin = Math.Pow(10, Math.Log10(yMin) - 1);
                yMax = Math.Pow(10, Math.Log10(yMax) + 1);
            }
            double yRange = yMax - yMin;
            yStep = Math.Pow(10, Math.Round(Math.Log10(yRange / 10), MidpointRounding.AwayFromZero));
            while (yMin <= (yMinReal - yStep))
            {
                yMin = yMin + yStep;
            }
            while (yMax >= (yMaxReal + yStep))
            {
                yMax = yMax - yStep;
            }
            if ((yMax - yMin) / yStep < 4)
                yStep = yStep / 10;
            while (yMin <= (yMinReal - yStep))
            {
                yMin = yMin + yStep;
            }
            while (yMax >= (yMaxReal + yStep))
            {
                yMax = yMax - yStep;
            }
            originY = yMin;
        }
        private void XToLinear()
        {
            xMin = Matrix.MinOfRow(xVal.ElementAt(0))[0, 0];
            xMax = Matrix.MaxOfRow(xVal.ElementAt(0))[0, 0];
            if (xVal.Count != 1)
            {
                for (int i = 1; i < xVal.Count; i++)
                {
                    double min1 = Matrix.MinOfRow(xVal.ElementAt(i))[0, 0];
                    double max1 = Matrix.MaxOfRow(xVal.ElementAt(i))[0, 0];
                    if (xMin > min1)
                        xMin = min1;
                    if (xMax < max1)
                        xMax = max1;
                }
            }
            double xMinReal = xMin;
            double xMaxReal = xMax;
            if (xMin > 0)
            {
                xMin = Math.Pow(10, Math.Floor(Math.Log10(xMin)));
            }
            else if (xMin < 0)
            {
                xMin = -Math.Pow(10, Math.Ceiling(Math.Log10(-xMin)));
            }
            if (xMax > 0)
            {
                xMax = Math.Pow(10, Math.Ceiling(Math.Log10(xMax)));
            }
            else if (xMax < 0)
            {
                xMax = -Math.Pow(10, Math.Floor(Math.Log10(-xMax)));
            }
            if (xMin == xMax)//说明在画平行于y轴的直线
            {
                xMin = xMin - 3;
                xMax = xMax + 3;
            }
            double xRange = xMax - xMin;
            xStep = Math.Pow(10, Math.Round(Math.Log10(xRange / 10), MidpointRounding.AwayFromZero));
            while (xMin <= (xMinReal - xStep))
            {
                xMin = xMin + xStep;
            }
            while (xMax >= (xMaxReal + xStep))
            {
                xMax = xMax - xStep;
            }
            if ((xMax - xMin) / xStep < 4)
                xStep = xStep / 10;
            while (xMin <= (xMinReal - xStep))
            {
                xMin = xMin + xStep;
            }
            while (xMax >= (xMaxReal + xStep))
            {
                xMax = xMax - xStep;
            }
            originX = xMin;
        }
        private void YToLinear()
        {
            yMin = Matrix.MinOfRow(yVal.ElementAt(0))[0, 0];
            yMax = Matrix.MaxOfRow(yVal.ElementAt(0))[0, 0];
            if (yVal.Count != 1)
            {
                for (int i = 1; i < yVal.Count; i++)
                {
                    double min2 = Matrix.MinOfRow(yVal.ElementAt(i))[0, 0];
                    double max2 = Matrix.MaxOfRow(yVal.ElementAt(i))[0, 0];
                    if (yMin > min2)
                        yMin = min2;
                    if (yMax < max2)
                        yMax = max2;
                }
            }
            double yMinReal = yMin;
            double yMaxReal = yMax;
            if (yMin > 0)
            {
                yMin = Math.Pow(10, Math.Floor(Math.Log10(yMin)));
            }
            else if (yMin < 0)
            {
                yMin = -Math.Pow(10, Math.Ceiling(Math.Log10(-yMin)));
            }
            if (yMax > 0)
            {
                yMax = Math.Pow(10, Math.Ceiling(Math.Log10(yMax)));
            }
            else if (yMax < 0)
            {
                yMax = -Math.Pow(10, Math.Floor(Math.Log10(-yMax)));
            }
            if (yMin == yMax)//说明在画平行于x轴的直线
            {
                yMax = yMax + 3;
                yMin = yMin - 3;
            }
            double yRange = yMax - yMin;
            yStep = Math.Pow(10, Math.Round(Math.Log10(yRange / 10), MidpointRounding.AwayFromZero));
            while (yMin <= (yMinReal - yStep))
            {
                yMin = yMin + yStep;
            }
            while (yMax >= (yMaxReal + yStep))
            {
                yMax = yMax - yStep;
            }
            if ((yMax - yMin) / yStep < 4)
                yStep = yStep / 10;
            while (yMin <= (yMinReal - yStep))
            {
                yMin = yMin + yStep;
            }
            while (yMax >= (yMaxReal + yStep))
            {
                yMax = yMax - yStep;
            }
            originY = yMin;
        }
        private void refreshParameter()
        {
            //以下作用为刷新曲线数据点、坐标原点所对应的物理坐标点
            marginWidth = (int)(this.Width / 20);
            marginHeight = (int)(this.Height / 20);
            plotArea = new Rectangle(marginWidth, marginHeight,
                                        this.Width - 2 * marginWidth, this.Height - 2 * marginHeight);
            //移除画图物理坐标并重置
            phisicsX.RemoveRange(0, phisicsX.Count);
            phisicsY.RemoveRange(0, phisicsY.Count);
            for (int i = 0; i < xVal.Count; i++)
            {
                phisicsX.Add(this.ToPhysicsX(xVal.ElementAt(i)));
                phisicsY.Add(this.ToPhysicsY(yVal.ElementAt(i)));
            }
            //重置重排后的物理坐标
            phisicsXReorder.RemoveRange(0, phisicsXReorder.Count);
            phisicsYReorder.RemoveRange(0, phisicsYReorder.Count);
            for (int i = 0; i < xValReorder.Count; i++)
            {
                phisicsXReorder.Add(this.ToPhysicsX(xValReorder.ElementAt(i)));
                phisicsYReorder.Add(this.ToPhysicsY(yValReorder.ElementAt(i)));
            }
            //保留源数据点的物理坐标
            phisicsXRepository.RemoveRange(0, phisicsXRepository.Count);
            phisicsYRepository.RemoveRange(0, phisicsYRepository.Count);
            for (int i = 0; i < xValRepository.Count; i++)
            {
                phisicsXRepository.Add(this.ToPhysicsX(xValRepository.ElementAt(i)));
                phisicsYRepository.Add(this.ToPhysicsY(yValRepository.ElementAt(i)));
            }
            //物理坐标原点重置
            originPoint = new Point((int)this.ToPhysicsX(originX), (int)this.ToPhysicsY(originY));
            Invalidate();//窗口刷新
        }
        private Matrix LinearFit(int index)
        {
            //辅助一次拟合函数,返回系数a,b
            double a, b;
            b = (Matrix.SumVector(Matrix.DotMultiple(xVal.ElementAt(index), yVal.ElementAt(index))) -
                Matrix.SumVector(xVal.ElementAt(index)) * Matrix.SumVector(yVal.ElementAt(index)) / 
                xVal.ElementAt(index).Columns);
            b = b / (Matrix.SumVector(Matrix.Pow(xVal.ElementAt(index), 2)) -
                Math.Pow(Matrix.SumVector(xVal.ElementAt(index)), 2) / xVal.ElementAt(index).Columns);
            a = Matrix.SumVector(yVal.ElementAt(index)) / yVal.ElementAt(index).Columns -
                b * Matrix.SumVector(xVal.ElementAt(index)) / xVal.ElementAt(index).Columns;
            return new Matrix(new double[2] { a, b });
        }
        private Matrix SquareFit(int index)
        {
            //辅助抛物线拟合函数,返回系数a,b,c
            Matrix result = Matrix.Zeros(xVal.ElementAt(index).Columns - 2, 3);
            for (int i = 0; i < result.Rows; i++)
            {
                Matrix xPoint_Temp = new Matrix(new double[3] { xVal.ElementAt(index)[0, i], 
                                        xVal.ElementAt(index)[0, i + 1], xVal.ElementAt(index)[0, i + 2] });
                xPoint_Temp = Matrix.Transfer(xPoint_Temp);
                Matrix yPoint_Temp = new Matrix(new double[3] { yVal.ElementAt(index)[0, i],
                                        yVal.ElementAt(index)[0, i + 1], yVal.ElementAt(index)[0, i + 2] });
                yPoint_Temp = Matrix.Transfer(yPoint_Temp);
                result = Matrix.SetMatrixRow(result, i, Matrix.Transfer(Matrix.FitCurve(xPoint_Temp, yPoint_Temp)));
            }
            return result;
        }
        #endregion

        #region 右键菜单响应

        private void plotDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Plot();
        }

        private void plotDefineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateXY generateXY1 = new GenerateXY();
            if (generateXY1.ShowDialog() == DialogResult.OK)
            {
                if (generateXY1.x != null && generateXY1.y != null)
                {
                    if (generateXY1.x.Columns == 1 && generateXY1.y.Columns == 1)
                    {
                        if (generateXY1.x.Rows == generateXY1.y.Rows)
                        {
                            try
                            {
                                Plot(Matrix.Transfer(generateXY1.x), Matrix.Transfer(generateXY1.y));
                            }
                            catch
                            {
                                MessageBox.Show("按给出数据绘图时出现不可知错误!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("设置的x,y变量个数不同!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("设置的x,y变量只能是列向量!");
                    }
                }
                else
                {
                    MessageBox.Show("没有设置x,y变量的值");
                }
            }
        }

        private void fitTargetToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fitActionToolStripMenuItem.Enabled = true;
            for (int i = 0; i < targetsBeenFitted.Count; i++)
            {
                if (fitTargetToolStripComboBox.SelectedIndex == targetsBeenFitted.ElementAt(i))
                {
                    fitActionToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void fitActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FitTimes = fitTimesToolStripComboBox.SelectedIndex + 1;
            this.Fit(fitTargetToolStripComboBox.SelectedIndex);
        }

        private void removeAllFncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllFnc();
        }

        private void gridOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridOn = !GridOn;
        }

        private void holdOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HoldOn = !HoldOn;
        }

        private void discreteOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiscreteOn = !DiscreteOn;
        }

        private void continuousOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContinuousOn = !ContinuousOn;
        }

        private void captrueModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureMode = !CaptureMode;
        }

        private void titleOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TitleOn = !TitleOn;
        }

        private void xAxisOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XAxisOn = !XAxisOn;
        }

        private void yAxisOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YAxisOn = !YAxisOn;
        }

        private void xLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XLog = !XLog;
        }

        private void yLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YLog = !YLog;
        }

        private void xLabelOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XLabelOn = !XLabelOn;
        }

        private void yLabelOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YLabelOn = !YLabelOn;
        }

        private void xTickOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XTickOn = !XTickOn;
        }

        private void yTickOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YTickOn = !YTickOn;
        }

        private void xTickLabelOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XTickLabelOn = !XTickLabelOn;
        }

        private void yTickLabelOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YTickLabelOn = !YTickLabelOn;
        }

        private void darkThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DarkTheme = !DarkTheme;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm settings = new SettingForm();
            #region 设置对话框中的初始值
            settings.titleTextBox.Text = PlotTitle;
            settings.xLabelTextBox.Text = XLabel;
            settings.yLabelTextBox.Text = YLabel;
            settings.titleTrackBar.Value = PlotTitleSize;
            settings.xLabelSizeTrackBar.Value = XLabelSize;
            settings.yLabelSizeTrackBar.Value = YLabelSize;

            settings.borderColor = BorderColor;
            settings.background = BackgroundColor;
            settings.axisAndLabelColor = AxisAndLabelColor;
            settings.curveColor = CurveColor;
            settings.discretePointColor = SamplePointColor;
            settings.colorDesComboBox.SelectedIndex = 0;
            settings.colorSetPanel.BackColor = BorderColor;

            settings.lineColorSet = this.lineColorList;
            settings.lineStyleSet = this.lineDashStyleList;
            settings.initializeLists();

            settings.xMinTextBox.Text = XMin.ToString();
            settings.xMaxTextBox.Text = XMax.ToString();
            settings.xStepTextBox.Text = XStep.ToString();
            settings.yMinTextBox.Text = YMin.ToString();
            settings.yMaxTextBox.Text = YMax.ToString();
            settings.yStepTextBox.Text = YStep.ToString();
            settings.curveDashStyle = CurveDashStyle;
            settings.initializeCurveSelection();
            #endregion
            #region 根据对话框设置属性值
            if (settings.ShowDialog() == DialogResult.OK)
            {
                PlotTitle = settings.titleTextBox.Text;
                XLabel = settings.xLabelTextBox.Text;
                YLabel = settings.yLabelTextBox.Text;
                PlotTitleSize = settings.titleTrackBar.Value;
                XLabelSize = settings.xLabelSizeTrackBar.Value;
                YLabelSize = settings.yLabelSizeTrackBar.Value;
                BorderColor = settings.borderColor;
                BackgroundColor = settings.background;
                AxisAndLabelColor = settings.axisAndLabelColor;
                CurveColor = settings.curveColor;
                SamplePointColor = settings.discretePointColor;

                if (settings.xMinTextBox.Text != "")
                {
                    XMin = Convert.ToDouble(settings.xMinTextBox.Text);
                }
                if (settings.xMaxTextBox.Text != "")
                {
                    XMax = Convert.ToDouble(settings.xMaxTextBox.Text);
                }
                if (settings.xStepTextBox.Text != "")
                {
                    XStep = Convert.ToDouble(settings.xStepTextBox.Text);
                }
                if (settings.yMinTextBox.Text != "")
                {
                    YMin = Convert.ToDouble(settings.yMinTextBox.Text);
                }
                if (settings.yMaxTextBox.Text != "")
                {
                    YMax = Convert.ToDouble(settings.yMaxTextBox.Text);
                }
                if (settings.yStepTextBox.Text != "")
                {
                    YStep = Convert.ToDouble(settings.yStepTextBox.Text);
                }
                CurveDashStyle = settings.curveDashStyle;
            }
            #endregion
        }
        #endregion

        private void AxisBox_Paint(object sender, PaintEventArgs e)
        {
            if (isPlotted)//画函数图的时候
            {
                //画背景
                Graphics g = e.Graphics;
                SolidBrush backgroundBrush = new SolidBrush(borderColor);
                g.FillRectangle(backgroundBrush, new Rectangle(0, 0, this.Width, this.Height));
                backgroundBrush = new SolidBrush(backgroundColor);
                g.FillRectangle(backgroundBrush, plotArea);
                #region 画x轴
                if (xAxis)
                {
                    Pen axisPen = new Pen(axisAndLabelColor, 1);
                    g.DrawLine(axisPen, marginWidth, originPoint.Y, marginWidth + plotArea.Width, originPoint.Y);
                    if (xTickOn)
                    {
                        decimal xTemp = Convert.ToDecimal(originX);//之所以用decimal是为了不产生精度损失
                        while (true)//画x正方向刻度
                        {
                            int physicsXTemp = (int)this.ToPhysicsX(Convert.ToDouble(xTemp));
                            g.DrawLine(axisPen, physicsXTemp, originPoint.Y - 4, physicsXTemp, originPoint.Y);
                            if (xTickLabelOn)
                            {
                                g.DrawString(Convert.ToString(xTemp), new Font("宋体", 9), new SolidBrush(axisAndLabelColor),
                                                                    new PointF(physicsXTemp, originPoint.Y + 2));
                            }
                            xTemp = xTemp + Convert.ToDecimal(xStep);
                            if (xTemp > Convert.ToDecimal(xMax))
                                break;
                        }
                        xTemp = Convert.ToDecimal(originX);
                        while (true)//画x反方向刻度
                        {
                            xTemp = xTemp - Convert.ToDecimal(xStep);
                            if (xTemp < Convert.ToDecimal(xMin))
                                break;
                            int physicsXTemp = (int)this.ToPhysicsX(Convert.ToDouble(xTemp));
                            g.DrawLine(axisPen, physicsXTemp, originPoint.Y - 4, physicsXTemp, originPoint.Y);
                            if (xTickLabelOn)
                            {
                                g.DrawString(Convert.ToString(xTemp), new Font("宋体", 9), new SolidBrush(axisAndLabelColor),
                                                                    new PointF(physicsXTemp, originPoint.Y + 2)); 
                            }
                        } 
                    }
                    //画xLabel
                    if (xLabelOn)
                    {
                        SizeF sizeOfXLabel = g.MeasureString(xLabel, new Font("宋体", xLabelSize));
                        int xLabelWidth = (int)sizeOfXLabel.Width;
                        int xLabelHeight = (int)sizeOfXLabel.Height;
                        g.DrawString(xLabel, new Font("宋体", xLabelSize), new SolidBrush(axisAndLabelColor),
                            new PointF(this.Width - marginWidth - 2 * xLabelWidth, originPoint.Y - xLabelHeight - 4));
                    }
                }
                #endregion
                #region 画y轴
                if (yAxis)
                {
                    Pen axisPen = new Pen(axisAndLabelColor, 1);
                    g.DrawLine(axisPen, originPoint.X, marginHeight + plotArea.Height, originPoint.X, marginHeight);
                    if (yTickOn)
                    {
                        decimal yTemp = Convert.ToDecimal(originY);//之所以用decimal是为了不产生精度损失
                        while (true)//画y正方向刻度
                        {
                            int physicsYTemp = (int)this.ToPhysicsY(Convert.ToDouble(yTemp));
                            g.DrawLine(axisPen, originPoint.X, physicsYTemp, originPoint.X + 4, physicsYTemp);
                            SizeF sizeOfYTick = g.MeasureString(Convert.ToString(yTemp), new Font("宋体", 9));
                            int yTickWidth = (int)sizeOfYTick.Width;
                            if (yTickLabelOn)
                            {
                                g.DrawString(Convert.ToString(yTemp), new Font("宋体", 9), new SolidBrush(axisAndLabelColor),
                                                                    new PointF(originPoint.X - yTickWidth, physicsYTemp)); 
                            }
                            yTemp = yTemp + Convert.ToDecimal(yStep);
                            if (yTemp > Convert.ToDecimal(yMax))
                                break;
                        }
                        yTemp = Convert.ToDecimal(originY);
                        while (true)//画y反方向刻度
                        {
                            yTemp = yTemp - Convert.ToDecimal(yStep);
                            if (yTemp < Convert.ToDecimal(yMin))
                                break;
                            int physicsYTemp = (int)this.ToPhysicsY(Convert.ToDouble(yTemp));
                            g.DrawLine(axisPen, originPoint.X, physicsYTemp, originPoint.X + 4, physicsYTemp);
                            SizeF sizeOfYTick = g.MeasureString(Convert.ToString(yTemp), new Font("宋体", 9));
                            int yTickWidth = (int)sizeOfYTick.Width;
                            if (yTickLabelOn)
                            {
                                g.DrawString(Convert.ToString(yTemp), new Font("宋体", 9), new SolidBrush(axisAndLabelColor),
                                                                    new PointF(originPoint.X - yTickWidth, physicsYTemp)); 
                            }
                        } 
                    }
                    if (yLabelOn)
                    {
                        SizeF sizeOfYLabel = g.MeasureString(Convert.ToString(yLabel), new Font("宋体", yLabelSize));
                        int yLabelWidth = (int)sizeOfYLabel.Width;
                        int yLabelHeight = (int)sizeOfYLabel.Height;
                        g.DrawString(yLabel, new Font("宋体", yLabelSize), new SolidBrush(axisAndLabelColor),
                            new PointF(originPoint.X + 4, marginHeight + 3));
                    }
                }
                #endregion
                #region 画网格
                if (gridOn)
                {
                    Pen gridPen = new Pen(new SolidBrush(axisAndLabelColor));
                    gridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    double xTemp = originX;
                    double yTemp = originY;
                    //画平行x轴网线
                    while (true)
                    {
                        xTemp = xTemp + xStep;
                        if (xTemp > xMax)
                            break;
                        int physicsXTemp = (int)ToPhysicsX(xTemp);
                        g.DrawLine(gridPen, physicsXTemp, marginHeight, physicsXTemp, this.Height - marginHeight);
                    }
                    xTemp = originX;
                    while (true)
                    {
                        xTemp = xTemp - xStep;
                        if (xTemp < xMin)
                            break;
                        int physicsXTemp = (int)ToPhysicsX(xTemp);
                        g.DrawLine(gridPen, physicsXTemp, marginHeight, physicsXTemp, this.Height - marginHeight);
                    }
                    //画平行y轴网线
                    while (true)
                    {
                        yTemp = yTemp + yStep;
                        if (yTemp > yMax)
                            break;
                        int physicsYTemp = (int)ToPhysicsY(yTemp);
                        g.DrawLine(gridPen, marginWidth, physicsYTemp, this.Width - marginWidth, physicsYTemp);
                    }
                    yTemp = originY;
                    while (true)
                    {
                        yTemp = yTemp - yStep;
                        if (yTemp < yMin)
                            break;
                        int physicsYTemp = (int)ToPhysicsY(yTemp);
                        g.DrawLine(gridPen, marginWidth, physicsYTemp, this.Width - marginWidth, physicsYTemp);
                    }
                }
                #endregion
                #region 画函数
                if (phisicsX.Count != 0)//有数据点才画函数图
                {
                    if (continuousOn)//是否画出连续曲线
                    {
                        Pen funcPen;
                        for (int i = 0; i < phisicsX.Count; i++)
                        {
                            funcPen = new Pen(lineColorList.ElementAt(i));
                            funcPen.DashStyle = lineDashStyleList.ElementAt(i);
                            for (int j = 0; j < phisicsX.ElementAt(i).Columns - 1; j++)
                            {
                                if (xVal.ElementAt(i)[0, j] >= xMin && xVal.ElementAt(i)[0, j + 1] <= xMax &&
                                    yVal.ElementAt(i)[0, j] >= yMin && yVal.ElementAt(i)[0, j + 1] <= yMax)
                                {
                                    g.DrawLine(funcPen, (int)phisicsX.ElementAt(i)[0, j], (int)phisicsY.ElementAt(i)[0, j],
                                        (int)phisicsX.ElementAt(i)[0, j + 1], (int)phisicsY.ElementAt(i)[0, j + 1]);
                                }
                            }
                        } 
                    }
                    if (discreteOn)//是否画出离散点
                    {
                        if (!refit)//未重新拟合
                        {
                            SolidBrush sampleBrush = new SolidBrush(samplePointColor);
                            for (int i = 0; i < phisicsX.Count; i++)
                            {
                                for (int j = 0; j < phisicsX.ElementAt(i).Columns; j++)
                                {
                                    g.FillEllipse(sampleBrush, (int)phisicsX.ElementAt(i)[0, j] - radiusOfSamplePoint,
                                                    (int)phisicsY.ElementAt(i)[0, j] - radiusOfSamplePoint,
                                                    2 * radiusOfSamplePoint, 2 * radiusOfSamplePoint);
                                }
                            }
                        }
                        else//重新拟合过
                        {
                            SolidBrush sampleBrush = new SolidBrush(samplePointColor);
                            for (int i = 0; i < phisicsXRepository.Count; i++)
                            {
                                for (int j = 0; j < phisicsXRepository.ElementAt(i).Columns; j++)
                                {
                                    g.FillEllipse(sampleBrush, 
                                                    (int)phisicsXRepository.ElementAt(i)[0, j] - radiusOfSamplePoint,
                                                    (int)phisicsYRepository.ElementAt(i)[0, j] - radiusOfSamplePoint,
                                                    2 * radiusOfSamplePoint, 2 * radiusOfSamplePoint);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region 画标题
                if (titleOn)
                {
                    SizeF sizeOfTitle = g.MeasureString(plotTitle, new Font("宋体", plotTitleSize));
                    int stringWidth = (int)sizeOfTitle.Width;
                    int stringHeight = (int)sizeOfTitle.Height;
                    Pen titlePen = new Pen(Color.Black, 1);
                    g.DrawString(plotTitle, new Font("宋体", plotTitleSize), new SolidBrush(axisAndLabelColor),
                                    new PointF((this.Width - stringWidth) / 2, 0));
                }
                #endregion
                #region 画动态捕捉点
                if (captureMode)//捕捉模式已打开
                {
                    if (capture)//捕捉到曲线上点
                    {
                        Pen capturePen = new Pen(Color.Red, 1);
                        g.DrawLine(capturePen, capturePoint.X - 35, capturePoint.Y, capturePoint.X + 35, capturePoint.Y);
                        g.DrawLine(capturePen, capturePoint.X, capturePoint.Y - 35, capturePoint.X, capturePoint.Y + 35);
                        g.DrawEllipse(capturePen, capturePoint.X - 4, capturePoint.Y - 4, 8, 8);
                        string showText="(" + Convert.ToString(capturePointActual.X) + "," +
                                            Convert.ToString(capturePointActual.Y) + ")";
                        SizeF sizeOfText = g.MeasureString(showText, new Font("宋体", 9));
                        int locX=capturePoint.X + 5;
                        int locY=capturePoint.Y + 5;
                        if ((locX + sizeOfText.Width) >= this.Width)
                            locX = locX - (int)sizeOfText.Width - 10;
                        if ((locY + sizeOfText.Height) >= this.Height)
                            locY = locY - (int)sizeOfText.Height - 10;
                        g.DrawString(showText, new Font("宋体", 9),new SolidBrush(Color.Red), new PointF(locX, locY)); 
                    }
                }
                #endregion
            }
            else//没有函数可画的情况
            {
                Graphics g = e.Graphics;
                SolidBrush background = new SolidBrush(Color.LightGray);
                g.FillRectangle(background, new Rectangle(0, 0, this.Width, this.Height));
                background = new SolidBrush(Color.White);
                g.FillRectangle(background, plotArea);
            }
        }

        private void AxisBox_MouseMove(object sender, MouseEventArgs e)//鼠标移动可以自动捕捉坐标点
        {
            capture = false;//一移动就设capture为false,只有当捕捉到点才设为true
            if (captureMode)//捕捉模式已打开
            {
                if (e.X <= (marginWidth + plotArea.Width) && e.X >= marginWidth && 
                    e.Y >= marginHeight && e.Y <= (marginHeight + plotArea.Height))//事件发生在有效区
                {
                    if (phisicsX.Count != 0)//有绘图数据
                    {
                        if (capturePointActual==null)
                        {
                            capturePointActual = new PointF((float)xVal.ElementAt(0)[0, 0],
                                                                                (float)yVal.ElementAt(0)[0, 0]); 
                        }
                        if (capturePoint==null)
                        {
                            capturePoint = new Point((int)phisicsX.ElementAt(0)[0, 0],
                                                                        (int)phisicsY.ElementAt(0)[0, 0]);
                        }
                        double r = Math.Abs(e.X - capturePoint.X) ^ 2 + Math.Abs(e.Y - capturePoint.Y) ^ 2;
                        for (int i = 0; i < phisicsX.Count; i++)
                        {
                            int[] rangeOfCatch = this.FindIndex(phisicsXReorder.ElementAt(i), e.X);
                            int temp = rangeOfCatch[0];
                            while ((temp>=0&&temp<phisicsXReorder.ElementAt(i).Columns)
                                            &&Math.Abs(e.X - phisicsXReorder.ElementAt(i)[0, temp]) <= sensitivity)
                            {
                                double tempA = Math.Pow(Math.Abs(e.X - phisicsXReorder.ElementAt(i)[0, temp]), 2);
                                double tempB = Math.Pow(Math.Abs(e.Y - phisicsYReorder.ElementAt(i)[0, temp]), 2);
                                double rNext = tempA + tempB;
                                if (r > rNext)
                                {
                                    capturePoint = new Point((int)phisicsXReorder.ElementAt(i)[0, temp],
                                                                (int)phisicsYReorder.ElementAt(i)[0, temp]);
                                    capturePointActual = new PointF((float)xValReorder.ElementAt(i)[0, temp],
                                                                        (float)yValReorder.ElementAt(i)[0, temp]);
                                    r = rNext;
                                }
                                temp--;
                            }
                            temp = rangeOfCatch[0];
                            while ((temp>=0&&temp<phisicsXReorder.ElementAt(i).Columns)&&
                                        Math.Abs(e.X - phisicsXReorder.ElementAt(i)[0, temp]) <= sensitivity)
                            {
                                double tempA = Math.Pow(Math.Abs(e.X - phisicsXReorder.ElementAt(i)[0, temp]), 2);
                                double tempB = Math.Pow(Math.Abs(e.Y - phisicsYReorder.ElementAt(i)[0, temp]), 2);
                                double rNext = tempA + tempB;
                                if (r > rNext)
                                {
                                    capturePoint = new Point((int)phisicsXReorder.ElementAt(i)[0, temp],
                                                                (int)phisicsYReorder.ElementAt(i)[0, temp]);
                                    capturePointActual = new PointF((float)xValReorder.ElementAt(i)[0, temp],
                                                                        (float)yValReorder.ElementAt(i)[0, temp]);
                                    r = rNext;
                                }
                                temp++;
                            }
                        }
                        if (r < (sensitivity ^ 2))
                            capture = true;
                        Invalidate();
                    }
                }
            }
        }

        private void AxisBox_SizeChanged(object sender, EventArgs e)
        {
            refreshParameter();
        }
    }
}
