﻿using System;
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
        private bool captureMode = true;//捕捉状态
        private bool capture = false;//是否捕捉到点
        private bool darkTheme = false;
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
        private Color boardColor = Color.LightGray;//边框颜色
        private Color backgroundColor = Color.White;//背景颜色
        private Color axisAndLabelColor = Color.Black;//坐标线及标签文本颜色
        private Color lineColor = Color.Blue;//默认图线颜色
        private System.Drawing.Drawing2D.DashStyle lineDashStyle =System.Drawing.Drawing2D.DashStyle.Solid;//默认为实线
        private List<Matrix> phisicsX = new List<Matrix>();
        private List<Matrix> phisicsY = new List<Matrix>();
        private List<Matrix> phisicsXReorder = new List<Matrix>();
        private List<Matrix> phisicsYReorder = new List<Matrix>();
        private Point capturePoint = new Point(0, 0);
        private PointF capturePointActual = new PointF(0, 0);
        private int sensitivity = 30;//表示捕捉点的敏感度,默认为半径20像素以内
        //绘图数据
        private List<Matrix> xVal = new List<Matrix>();
        private List<Matrix> yVal = new List<Matrix>();
        private List<Matrix> xValReorder = new List<Matrix>();
        private List<Matrix> yValReorder = new List<Matrix>();
        private double xStep;
        private double yStep;
        private double xMin;
        private double xMax;
        private double yMin;
        private double yMax;
        private double originX;
        private double originY;
        #endregion

        #region 访问器
        //在调用Plot函数前,任何属性都是不可改的//即没有画函数图前改属性没有意义
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
                    Invalidate(); 
                }
            }
        }
        /// <summary>
        /// x轴是否取对数坐标
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
                }
            }
        }
        /// <summary>
        /// y轴是否取对数坐标
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
                            if (yLog!=value)
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
                    boardColor = Color.Black;
                    backgroundColor = Color.Black;
                    axisAndLabelColor = Color.Yellow;
                }
                else
                {
                    boardColor = Color.LightGray;
                    backgroundColor = Color.White;
                    axisAndLabelColor = Color.Black;
                }
                if (isPlotted)
                {
                    Invalidate();
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
                    xMin = value;
                    refreshParameter();
                    Invalidate();
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
                    xMax = value;
                    refreshParameter();
                    Invalidate();
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
                    yMin = value;
                    refreshParameter();
                    Invalidate();
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
                    yMax = value;
                    refreshParameter();
                    Invalidate();
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
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return boardColor; }
            set
            {
                if (isPlotted)
                {
                    boardColor = value;
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
        ///         在AxisBox上画出默认的y=x图像(x范围0-10,步距为1,,11个数据点)
        /// </summary>
        public void Plot()
        {
            Matrix xValue = Matrix.RangeVector(0, 10);
            Matrix yValue = Matrix.RangeVector(0, 10);
            this.Plot(xValue, yValue);
        }
        /// <summary>
        ///         在AxisBox上由给出x、y坐标画出曲线
        /// </summary>
        /// <param name="x">x行向量</param>
        /// <param name="y">y行向量</param>
        public void Plot(Matrix x, Matrix y)
        {
            isPlotted = true;
            if (holdOn)//是否追加画图还是覆盖画图
            {
                xVal.Add(x);
                yVal.Add(y);
            }
            else
            {
                if (xVal.Count == 0)
                {
                    xVal.Add(x);
                    yVal.Add(y);
                }
                else
                {
                    xVal.RemoveRange(0, xVal.Count);
                    yVal.RemoveRange(0, yVal.Count);
                    xVal.Add(x);
                    yVal.Add(y);
                }
            }
            this.XToLinear();//默认线性坐标
            this.YToLinear();
            this.getReorderedVersion();//为了提高鼠标移动捕捉点的效率,需要获得重新排列后的坐标点,方便查找
            refreshParameter();
        }
        /// <summary>
        ///         在AxisBox上由给出x、y坐标画出曲线
        /// </summary>
        /// <param name="xArray">一维数组</param>
        /// <param name="yArray">一维数组</param>
        public void Plot(double[] xArray, double[] yArray)
        {
            Matrix x = new Matrix(xArray);
            Matrix y = new Matrix(yArray);
            this.Plot(x, y);
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
            while ((yMax - yMin) / yStep <= 4)
            {
                yStep = (yMax - yMin) / 8;
                yStep = Math.Pow(10, Math.Round(yStep));
            }
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
            //物理坐标原点重置
            originPoint = new Point((int)this.ToPhysicsX(originX), (int)this.ToPhysicsY(originY));
            Invalidate();//窗口刷新
        }
        #endregion

        private void AxisBox_Paint(object sender, PaintEventArgs e)
        {
            if (isPlotted)//画函数图的时候
            {
                //画背景
                Graphics g = e.Graphics;
                SolidBrush backgroundBrush = new SolidBrush(boardColor);
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
                if (phisicsX.Count != 0)
                {
                    Pen funcPen = new Pen(this.lineColor, 1);
                    funcPen.DashStyle = lineDashStyle;
                    for (int i = 0; i < phisicsX.Count; i++)
                    {
                        for (int j = 0; j < phisicsX.ElementAt(i).Columns - 1; j++)
                        {
                            g.DrawLine(funcPen, (int)phisicsX.ElementAt(i)[0, j], (int)phisicsY.ElementAt(i)[0, j],
                                            (int)phisicsX.ElementAt(i)[0, j + 1], (int)phisicsY.ElementAt(i)[0, j + 1]);
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
