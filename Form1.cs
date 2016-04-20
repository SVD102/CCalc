using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using System.IO;
using System.Data;

namespace CoefficientsCalculation
{
    public partial class Form1 : Form, IView, IModelObserver
    {
        IController controller;

        List<ZedGraph.ZedGraphControl> _zedGraph = new List<ZedGraph.ZedGraphControl>();
        List<System.Windows.Forms.TabPage> _tabPages = new List<System.Windows.Forms.TabPage>();
        List<System.Windows.Forms.Label> labelsOfAxis = new List<System.Windows.Forms.Label>();

        GraphPane paneStart = new GraphPane();

        public void setController(IController cont) 
        {
            controller = cont;
        }

        public Form1()
        {
            InitializeComponent();

            checkedListBoxAxis.CheckOnClick = true;
        }

        public bool fileOpen = false;

        int majorStepX;
        int minorStepX;
        int YAxisStep = 2;

        string xAxisFormat;

        string fileCatalog = "c:";
        string fileCatalog1 = "c:";
        string savefileCatalog2 = "c:";
        string _pathFileName;
        List<string> pathFileName = new List<string>();

        int countOfGraphs = 0;

        int countOfLabel = 0;

        int countOfVisibleGraphs = 0;

        int numDeletedGraph = 0;

        string axisAcurracy = "F3";
        decimal aproxChangeInt = 500;

        bool doubleClickCheck;
        bool reCalcCheck = false;
        bool reCalcAxisCheck = false;
        bool TempAproxDensityCheck = false;
        bool TempAproxTicCheck = false;
        bool FlowRateCoeffCheck = false;
        bool PressureCoeffCheck = false;
        bool DensityCoeffCheck = false;
        bool newGraphsPainted = false;
        bool AllCoeffCheck = false;
        bool xmlFileAdded = false;

        int selectedGraph;

        List<bool> visibleOfGraph = new List<bool>();

        DataTable _DtPressTic = new DataTable();
        DataTable _DtRoTic = new DataTable();
        DataTable _DtPressADC = new DataTable();
        DataTable _DtRoRo0 = new DataTable();
        DataTable _DtqQ = new DataTable();

        List<GraphFile> paneList = new List<GraphFile>();

        double[] _aa6 = new double[6];
        double[] _aa3 = new double[3];
        List<string> enterDensity = new List<string>();
        int countListDt = 0;
        List<DataTable> _listDt = new List<DataTable>();
        int num36 = 0;

        List<int> countOfAxis = new List<int>();
        //List<List<PointPairList>> arrList = new List<List<PointPairList>>();
        List<List<PointPairList>> filtredarrList = new List<List<PointPairList>>();
        List<List<string>> colorOfAxis = new List<List<string>>();
        List<List<string>> nameOfAxis = new List<List<string>>();
        List<int> minScaleOfMassives = new List<int>();
        List<List<bool>> visibleOfAxis = new List<List<bool>>();
        List<double> dateOfFiles = new List<double>();
        List<string> nameOfFiles = new List<string>();
        List<double> filesTimeStart = new List<double>();
        List<double> filesTimeFinish = new List<double>();
        List<int> leftBorders = new List<int>();
        List<int> rightBorders = new List<int>();
        List<List<double>> maxLists = new List<List<double>>();
        List<List<double>> minLists = new List<List<double>>();
        List<List<double>> startMaxLists = new List<List<double>>();
        List<List<double>> startMinLists = new List<List<double>>();
        List<double> nowTimeStartList = new List<double>();
        List<double> nowTimeFinishList = new List<double>();

        List<List<int>> ArrAxis = new List<List<int>>();
        List<List<double>> zeroInterval = new List<List<double>>();
 
        public void valueStarted(IModel m, ModelEventArgs e) //начальное считывание данных
        {
            _tabPages.Add(new System.Windows.Forms.TabPage());
            _tabPages[countOfGraphs - 1].Location = new System.Drawing.Point(4, 22);
            _tabPages[countOfGraphs - 1].Name = "_tabPages" + countOfGraphs;
            _tabPages[countOfGraphs - 1].Padding = new System.Windows.Forms.Padding(3);
            _tabPages[countOfGraphs - 1].Size = new System.Drawing.Size(930, 440);
            _tabPages[countOfGraphs - 1].TabIndex = countOfGraphs;
            _tabPages[countOfGraphs - 1].Text = pathFileName[countOfGraphs - 1];
            _tabPages[countOfGraphs - 1].UseVisualStyleBackColor = true;
            _tabPages[countOfGraphs - 1].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
          | System.Windows.Forms.AnchorStyles.Left)
          | System.Windows.Forms.AnchorStyles.Right)));
            tabControl2.Controls.Add(_tabPages[countOfGraphs - 1]);

            int countOfAx;
            //List<PointPairList> Arr = new List<PointPairList>();
            List<PointPairList> filtredArr = new List<PointPairList>();
            List<string> colorOfAx = new List<string>();
            List<string> nameOfAx = new List<string>();
            int minScaleOfMassive;
            List<bool> visibleOfAx = new List<bool>();
            double dateOfFile;
            string nameOfFile;
            double fileTimeStart;
            double fileTimeFinish;
            int leftBorder;
            int rightBorder;
            List<double> maxList = new List<double>();
            List<double> minList = new List<double>();
            List<double> startMaxList = new List<double>();
            List<double> startMinList = new List<double>();
            List<List<double>> approxValues = new List<List<double>>();
            double nowTimeStart;
            double nowTimeFinish;

            countOfAx = e.numAxis1;
            minScaleOfMassive = e.minScaleOfMassive1;
            dateOfFile = e.dateOfFile1;
            nameOfFile = e.nameOfFile1;
            fileTimeStart = e.fileTimeStart1;
            fileTimeFinish = e.fileTimeFinish1;
            leftBorder = 0;
            rightBorder = minScaleOfMassive;
            nowTimeStart = e.fileTimeStart1;
            nowTimeFinish = e.fileTimeFinish1;
 
            for (int i = 0; i < countOfAx; i++)
            {
                //Arr.Add(e.newArrList1[i]);
                filtredArr.Add(e.newfiltredArrList1[i]);
                colorOfAx.Add(e.numColor1[i]);
                nameOfAx.Add(e.nameOfAxis1[i]);
                visibleOfAx.Add(e.visibleAxis1[i]);
                maxList.Add(e.maxList1[i]);
                minList.Add(e.minList1[i]);
                startMaxList.Add(e.startMaxList1[i]);
                startMinList.Add(e.startMinList1[i]);
            }

            countOfAxis.Add(countOfAx);
            //arrList.Add(Arr);
            filtredarrList.Add(filtredArr);
            colorOfAxis.Add(colorOfAx);
            nameOfAxis.Add(nameOfAx);
            minScaleOfMassives.Add(minScaleOfMassive);
            visibleOfAxis.Add(visibleOfAx);
            dateOfFiles.Add(dateOfFile);
            nameOfFiles.Add(nameOfFile);
            filesTimeStart.Add(fileTimeStart);
            filesTimeFinish.Add(fileTimeFinish);
            leftBorders.Add(leftBorder);
            rightBorders.Add(rightBorder);
            maxLists.Add(maxList);
            minLists.Add(minList);
            startMaxLists.Add(startMaxList);
            startMinLists.Add(startMinList);
            nowTimeStartList.Add(nowTimeStart);
            nowTimeFinishList.Add(nowTimeFinish);

            fillPaneList();

            if (countOfGraphs - 1 > 0)
            {
                paneList[countOfGraphs - 1].visibleAxis = paneList[0].visibleAxis;
                paneList[countOfGraphs - 1].colorOfAxis = paneList[0].colorOfAxis;
                paneList[countOfGraphs - 1].visibleOfXaxis = paneList[0].visibleOfXaxis;
            }

            for (int i = 0; i < paneList[countOfGraphs - 1].countOfAxis; i++)
            {
                if (paneList[countOfGraphs - 1].visibleAxis[i] == true)
                {
                    paneList[countOfGraphs - 1].countOfVisibleAxis1++;
                    paneList[countOfGraphs - 1].countOfVisibleAxis2++;
                }
            }

            majorStepX = 1200;
            minorStepX = 1200;

            xAxisFormat = "HH:mm";

            visibleOfGraph.Add(true);

            DrawGraphMaster();

            labelsCreate();

            countOfLabel++;

            labelsOnDesk(labelsOfAxis);

            checkedListBoxAxis.Items.Clear();

            comboBoxStatis.Items.Clear();

            for (int i = 0; i < paneList[0].countOfAxis; i++)
            {
                checkedListBoxAxis.Items.Add(paneList[0].nameOfAxis[i]);
                comboBoxStatis.Items.Add(paneList[0].nameOfAxis[i]);
            }

            for (int i = 0; i < checkedListBoxAxis.Items.Count; i++)
                checkedListBoxAxis.SetItemChecked(i, paneList[0].visibleAxis[i]);


            checkedListBoxFiles.Items.Add(openFileDialog1.SafeFileName);
            comboBoxPressure.Items.Add(openFileDialog1.SafeFileName);
            comboBoxDensity.Items.Add(openFileDialog1.SafeFileName);
            comboBoxFlowRate.Items.Add(openFileDialog1.SafeFileName);
            comboBoxRate.Items.Add(openFileDialog1.SafeFileName);
            checkedListBoxFiles.SetItemChecked(countOfGraphs - 1, true);

            countOfVisibleGraphs++;

            labelDateOfFile.Text = DateTime.FromOADate(paneList[selectedGraph].dateOfFile).ToString("d");
            labelNameOfFile.Text = paneList[selectedGraph].nameOfFile;

            textBoxTimeStart.Text = DateTime.FromOADate(paneList[selectedGraph].fileTimeStart).ToString();
            textBoxTimeFinish.Text = DateTime.FromOADate(paneList[selectedGraph].fileTimeFinish).ToString();

            labelBackGround.SendToBack();
        }

        private void fillPaneList()
        {
            paneList.Add(new GraphFile());
            zeroInterval.Add(new List<double>());
            paneList[countOfGraphs - 1].countOfAxis = countOfAxis[countOfGraphs - 1];
            //paneList[countOfGraphs - 1].drawArrlist = arrList[countOfGraphs - 1];
            paneList[countOfGraphs - 1].filtredArrlist = filtredarrList[countOfGraphs - 1];
            paneList[countOfGraphs - 1].colorOfAxis = colorOfAxis[countOfGraphs - 1];
            paneList[countOfGraphs - 1].nameOfAxis = nameOfAxis[countOfGraphs - 1];
            paneList[countOfGraphs - 1].minScaleOfMassive = minScaleOfMassives[countOfGraphs - 1];
            paneList[countOfGraphs - 1].visibleAxis = visibleOfAxis[countOfGraphs - 1];
            paneList[countOfGraphs - 1].dateOfFile = dateOfFiles[countOfGraphs - 1];
            paneList[countOfGraphs - 1].nameOfFile = nameOfFiles[countOfGraphs - 1];
            paneList[countOfGraphs - 1].fileTimeStart = filesTimeStart[countOfGraphs - 1];
            paneList[countOfGraphs - 1].fileTimeFinish = filesTimeFinish[countOfGraphs - 1];
            paneList[countOfGraphs - 1].leftBorder = leftBorders[countOfGraphs - 1];
            paneList[countOfGraphs - 1].rightBorder = filtredarrList[countOfGraphs - 1][0].Count;
            paneList[countOfGraphs - 1].maxList = maxLists[countOfGraphs - 1];
            paneList[countOfGraphs - 1].minList = minLists[countOfGraphs - 1];
            paneList[countOfGraphs - 1].startMaxList = startMaxLists[countOfGraphs - 1];
            paneList[countOfGraphs - 1].startMinList = startMinLists[countOfGraphs - 1];
            paneList[countOfGraphs - 1].nowTimeStart = nowTimeStartList[countOfGraphs - 1];
            paneList[countOfGraphs - 1].nowTimeFinish = nowTimeFinishList[countOfGraphs - 1];
            paneList[countOfGraphs - 1].filterOfMassive = true;

            _zedGraph.Add(new ZedGraph.ZedGraphControl());
            _zedGraph[countOfGraphs - 1].Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            _zedGraph[countOfGraphs - 1].Location = new System.Drawing.Point(0, 0);
            _zedGraph[countOfGraphs - 1].Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            _zedGraph[countOfGraphs - 1].Name = "_zedGraph" + countOfGraphs;
            _zedGraph[countOfGraphs - 1].ScrollGrace = 0D;
            _zedGraph[countOfGraphs - 1].ScrollMaxX = 0D;
            _zedGraph[countOfGraphs - 1].ScrollMaxY = 0D;
            _zedGraph[countOfGraphs - 1].ScrollMaxY2 = 0D;
            _zedGraph[countOfGraphs - 1].ScrollMinX = 0D;
            _zedGraph[countOfGraphs - 1].ScrollMinY = 0D;
            _zedGraph[countOfGraphs - 1].ScrollMinY2 = 0D;
            _zedGraph[countOfGraphs - 1].ContextMenuStrip.Dispose(); // отключение меню правой кнопки
 
            if (vkl == false)
                _zedGraph[countOfGraphs - 1].Size = new System.Drawing.Size(_zedGraph_X, _zedGraph_Y1);
            if (vkl == true)
                _zedGraph[countOfGraphs - 1].Size = new System.Drawing.Size(_zedGraph_X, _zedGraph_Y2);
            _zedGraph[countOfGraphs - 1].TabIndex = 0;

            _zedGraph[countOfGraphs - 1].MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(zedGraph_DoubleMouseClick);
            _zedGraph[countOfGraphs - 1].ZoomEvent += new ZedGraphControl.ZoomEventHandler(zedGraph_ZoomEvent);
            _zedGraph[countOfGraphs - 1].MouseWheel += new MouseEventHandler(zedGraph_MouseWheel);

            paneList[countOfGraphs - 1].pane = _zedGraph[countOfGraphs - 1].GraphPane;
            _tabPages[countOfGraphs - 1].Controls.Add(_zedGraph[countOfGraphs - 1]);
        }

        public void zedGraph_MouseWheel(object sender, MouseEventArgs e)
        {

        }

        public void zedGraph_ZoomEvent(object sender, ZoomState oldState, ZoomState newState) // зумирование
        {
            if (fileOpen == true)
            {
                double zoomFirst;
                double zoomSecond;

                zoomFirst = Math.Abs(paneList[selectedGraph].pane.XAxis.Scale.Min);

                if (doubleClickCheck == true)
                {
                    paneList[selectedGraph].pane.XAxis.Scale.Max = paneList[selectedGraph].filtredArrlist[0].Count;
                }

                zoomSecond = Math.Abs(paneList[selectedGraph].pane.XAxis.Scale.Max);

                controller.zoomGraph(zoomFirst, zoomSecond, doubleClickCheck, selectedGraph);

                doubleClickCheck = false;
            }
        }
   

        public void zedGraph_DoubleMouseClick(object sender, MouseEventArgs e)//возвращает первоначальный масштаб при нажании на ЛКМ
        {
            if (fileOpen == true)
            {
                paneList[selectedGraph].maxList = paneList[selectedGraph].startMaxList;
                paneList[selectedGraph].minList = paneList[selectedGraph].startMinList;

                paneList[selectedGraph].nowTimeStart = paneList[selectedGraph].fileTimeStart;
                paneList[selectedGraph].nowTimeFinish = paneList[selectedGraph].fileTimeFinish;

                majorStepX = 1200;
                minorStepX = 1200;
                xAxisFormat = "HH:mm";

                if (comboBoxStatis.SelectedIndex >= 0)
                {
                    labelMax.Text = "Максимум:" + paneList[selectedGraph].startMaxList[comboBoxStatis.SelectedIndex];
                    labelMin.Text = "Минимум:" + paneList[selectedGraph].startMinList[comboBoxStatis.SelectedIndex];
                    labelMiddle.Text = "Среднее:" + (paneList[selectedGraph].startMinList[comboBoxStatis.SelectedIndex] + paneList[selectedGraph].startMaxList[comboBoxStatis.SelectedIndex]) / 2;
                }

                doubleClickCheck = true;

                _zedGraph[selectedGraph].ZoomOutAll(paneList[selectedGraph].pane);

                _zedGraph[selectedGraph].AxisChange();
                _zedGraph[selectedGraph].Invalidate();
            }
        }

        public void axisChangedVisible(IModel m, ModelEventArgsVisible e) // включить выключить отображение графиков при шелчке на галочку
        {
            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].visibleAxis[e.numOfVisibleAxis1] = e.visibleStatus;
            }

            if (paneList[selectedGraph].visibleAxis.Find(visibleAxisEl => visibleAxisEl == true))
                paneList[selectedGraph].visibleOfXaxis = true;
            else
                paneList[selectedGraph].visibleOfXaxis = false;

            if(paneList[selectedGraph].countOfAxis >= 17)
            {
                reCalcAxisCheck = paneList[0].visibleAxis[paneList[0].countOfAxis - 1];
            }

            DrawGraphMasterChange();

            labelsOnDesk(labelsOfAxis);
        }

        private void DrawGraphStart() // рисование начальной доски рисования до считывания файла
        {
            paneStart.CurveList.Clear();

            paneStart.XAxis.Scale.FontSpec.Size = 10;// Установим размеры шрифтов для меток вдоль осей
            //paneStart.YAxis.Scale.FontSpec.Size = 10;

            paneStart.YAxisList.Clear();// очищает экран
            paneStart.Title.Text = "";
            paneStart.XAxis.Title.Text = "";
            paneStart.XAxis.IsVisible = false;

        }

        private void DrawGraphOnes(GraphFile paneFile)
        {
            _zedGraph[countOfGraphs - 1].IsEnableZoom = true;
            _zedGraph[countOfGraphs - 1].IsEnableVZoom = false;
            _zedGraph[countOfGraphs - 1].ZoomStepFraction = 0;

            List<LineItem> ArrCurve = new List<LineItem>();

            paneFile.pane.CurveList.Clear();
            paneFile.pane.Title.Text = "";
            paneFile.pane.XAxis.Title.Text = "";
            paneFile.pane.YAxisList.Clear();
            paneFile.pane.XAxis.Scale.FontSpec.Size = 10;
            paneFile.pane.XAxis.Type = AxisType.DateAsOrdinal;
            paneFile.pane.XAxis.Scale.Format = xAxisFormat;
            paneFile.pane.XAxis.Scale.Min = 0;
            paneFile.pane.XAxis.Scale.Max = paneFile.rightBorder;
            paneFile.pane.IsFontsScaled = false;

            ArrAxis.Add(new List<int>());

            for (int i = 0; i < paneFile.countOfAxis; i++)
            {
                ArrAxis[countOfGraphs - 1].Add(paneFile.pane.AddYAxis(paneFile.nameOfAxis[i]));
                ArrCurve.Add(paneFile.pane.AddCurve("", paneFile.filtredArrlist[i], Color.FromName(paneFile.colorOfAxis[i]), SymbolType.None));
                ArrCurve[i].YAxisIndex = ArrAxis[countOfGraphs - 1][i];
                paneFile.pane.YAxisList[i].Title.FontSpec.FontColor = Color.FromName(paneFile.colorOfAxis[i]);
                paneFile.pane.YAxisList[i].Scale.FontSpec.FontColor = Color.FromName(paneFile.colorOfAxis[i]);
            }

            for (int i = 0; i < paneFile.countOfAxis; i++)
            {
                paneFile.pane.YAxisList[i].MajorGrid.IsZeroLine = false;//Включим показ оси на уровне X = 0 и Y = 0,
                paneFile.pane.YAxisList[i].MajorGrid.DashOn = paneFile.pane.XAxis.MajorGrid.DashOn;
                ArrCurve[i].Line.Width = 1;

                paneFile.pane.CurveList[i].IsVisible = paneFile.visibleAxis[i];// удалении  i кривой на графике
                paneFile.pane.YAxisList[i].IsVisible = paneFile.visibleAxis[i];// удаляет подпись этой линии по оси Y

                paneFile.pane.YAxisList[i].Title.Text = "";
                paneFile.pane.YAxisList[i].Title.IsOmitMag = true;

                paneFile.pane.YAxisList[i].Scale.Max = paneFile.maxList[i];
                paneFile.pane.YAxisList[i].Scale.Min = paneFile.minList[i];

                paneFile.pane.YAxisList[i].Scale.FontSpec.Size = 10;

                paneFile.pane.YAxisList[i].AxisGap = 1;

                paneFile.pane.YAxisList[i].Scale.Format = axisAcurracy;
                paneFile.pane.YAxisList[i].Scale.MajorStep = (paneFile.maxList[i] - paneFile.minList[i]) / (5*YAxisStep);
                paneFile.pane.YAxisList[i].Scale.MinorStep = (paneFile.maxList[i] - paneFile.minList[i]) / (25*YAxisStep);
            }

            if (paneFile.countOfAxis >= 16)
            {
                paneFile.pane.YAxisList[5].Scale.Mag = 0;
                paneFile.pane.YAxisList[6].Scale.Mag = 0;
                paneFile.pane.YAxisList[12].Scale.Mag = 0;
            }

            if (paneFile.visibleAxis.Find(visibleAxisEl => visibleAxisEl == true))
            {
                int qq = paneFile.visibleAxis.FindIndex(0, visibleAxisEl => visibleAxisEl == true);
                paneFile.pane.YAxisList[qq].MajorGrid.IsVisible = true;// вкл сетку на графике
                paneFile.pane.YAxisList[qq].MajorGrid.DashOn = paneFile.pane.XAxis.MajorGrid.DashOn;
                paneFile.pane.XAxis.IsVisible = true;
            }

            paneFile.pane.XAxis.MajorGrid.IsVisible = true; // вкл сетку по Х

            paneFile.pane.XAxis.Title.IsOmitMag = true;

            textBoxNowTimeStart.Text = DateTime.FromOADate(paneFile.nowTimeStart).ToString();
            textBoxNowTimeFinish.Text = DateTime.FromOADate(paneFile.nowTimeFinish).ToString();

            paneFile.pane.XAxis.Scale.MajorStep = majorStepX;
            paneFile.pane.XAxis.Scale.MinorStep = minorStepX;

            fileOpen = true;

            _zedGraph[countOfGraphs - 1].AxisChange();
            _zedGraph[countOfGraphs - 1].Invalidate();

            Console.WriteLine(selectedGraph);
        }

        private void labelsCreate()
        {
            if (countOfLabel > 0)
            {

            }
            else
            {
                labelsOfAxis.Clear();

                for (int j = 0; j < paneList[0].countOfVisibleAxis1; j++)
                {
                    labelsOfAxis.Add(new System.Windows.Forms.Label());
                }
            }
        }

        private void DrawGraphMaster()
        {
            for (int i = countOfGraphs - 1; i < countOfGraphs; i++)
            {
                DrawGraphOnes(paneList[i]);
            }

            _zedGraph[countOfGraphs - 1].AxisChange();
            _zedGraph[countOfGraphs - 1].Invalidate();
        }

        private void DrawGraphMasterChange()
        {
            countOfLabel = 0;

            for (int i = 0; i < countOfGraphs; i++)
            {
                DrawGraph(paneList[i]);

                countOfLabel++;
            }
        }

        private void DrawGraph(GraphFile paneFile) // рисование графиков
        {
            paneFile.pane.XAxis.Scale.Min = paneFile.leftBorder;
            paneFile.pane.XAxis.Scale.Max = paneFile.rightBorder;
            paneFile.pane.XAxis.Scale.Format = xAxisFormat;

            List<LineItem> ArrCurve = new List<LineItem>();

            paneFile.pane.CurveList.Clear();

            ArrCurve.Clear();

            for (int i = 0; i < paneFile.countOfAxis; i++)
            {
                ArrCurve.Add(paneFile.pane.AddCurve("", paneFile.filtredArrlist[i], Color.FromName(paneFile.colorOfAxis[i]), SymbolType.None));
                ArrCurve[i].YAxisIndex = ArrAxis[countOfGraphs - 1][i];
                ArrCurve[i].Line.Width = 1;

                paneFile.pane.CurveList[i].IsVisible = paneFile.visibleAxis[i];// удалении  i кривой на графике
                paneFile.pane.YAxisList[i].IsVisible = paneFile.visibleAxis[i];// удаляет подпись этой линии по оси Y
                paneFile.pane.YAxisList[i].MajorGrid.IsVisible = false;// вкл сетку на графике

                paneFile.pane.CurveList[i].Color = Color.FromName(paneFile.colorOfAxis[i]);
                paneFile.pane.YAxisList[i].Title.FontSpec.FontColor = Color.FromName(paneFile.colorOfAxis[i]);
                paneFile.pane.YAxisList[i].Scale.FontSpec.FontColor = Color.FromName(paneFile.colorOfAxis[i]);

                paneFile.pane.YAxisList[i].Scale.Max = paneFile.maxList[i];
                paneFile.pane.YAxisList[i].Scale.Min = paneFile.minList[i];
                paneFile.pane.YAxisList[i].Scale.Format = axisAcurracy;

                paneFile.pane.YAxisList[i].Scale.MajorStep = (paneFile.maxList[i] - paneFile.minList[i]) / (5 * YAxisStep);
                paneFile.pane.YAxisList[i].Scale.MinorStep = (paneFile.maxList[i] - paneFile.minList[i]) / (25 * YAxisStep);
            }

            if (paneFile.countOfAxis >= 16)
            {
                paneFile.pane.YAxisList[5].Scale.Mag = 0;
                paneFile.pane.YAxisList[6].Scale.Mag = 0;
                paneFile.pane.YAxisList[12].Scale.Mag = 0;
            }

            textBoxNowTimeStart.Text = DateTime.FromOADate(paneFile.nowTimeStart).ToString();
            textBoxNowTimeFinish.Text = DateTime.FromOADate(paneFile.nowTimeFinish).ToString();

            paneFile.pane.XAxis.Scale.MajorStep = majorStepX;
            paneFile.pane.XAxis.Scale.MinorStep = minorStepX;

            if (paneFile.visibleAxis.Find(visibleAxisEl => visibleAxisEl == true))
            {
                int qq = paneFile.visibleAxis.FindIndex(0, visibleAxisEl => visibleAxisEl == true);
                paneFile.pane.YAxisList[qq].MajorGrid.IsVisible = true;// вкл сетку на графике
                paneFile.pane.XAxis.IsVisible = true;
            }
            else
            {
                paneFile.pane.XAxis.IsVisible = false;
            }

            if (countOfLabel > 0)
            {

            }
            else
            {
                for (int i = 0; i < paneFile.countOfVisibleAxis2; i++)
                {
                    Controls.Remove(labelsOfAxis[i]);
                }

                labelsOfAxis.Clear();

                for (int i = 0; i < paneFile.countOfVisibleAxis1; i++)
                {
                    labelsOfAxis.Add(new System.Windows.Forms.Label());
                }
            }

            _zedGraph[selectedGraph].AxisChange();
            _zedGraph[selectedGraph].Invalidate();
        }

        public void zoomedGraph(IModel m, ModelEventZoom e) // зумирование
        {
            paneList[selectedGraph].leftBorder = e.leftBorder1;
            paneList[selectedGraph].rightBorder = e.rightBorder1;
            paneList[selectedGraph].maxList = e.maxList1;
            paneList[selectedGraph].minList = e.minList1;
            paneList[selectedGraph].nowTimeStart = e.nowTimeStart1;
            paneList[selectedGraph].nowTimeFinish = e.nowTimeFinish1;
            paneList[selectedGraph].filterOfMassive = e.filterOfMassives1;
            doubleClickCheck = e.doubleClickCheck1;
            if (comboBoxStatis.SelectedIndex >= 0)
            {
                if (paneList[selectedGraph].maxList[comboBoxStatis.SelectedIndex] != 1)
                {
                    labelMax.Text = "Максимум:" + paneList[selectedGraph].maxList[comboBoxStatis.SelectedIndex];
                    labelMin.Text = "Минимум:" + paneList[selectedGraph].minList[comboBoxStatis.SelectedIndex];
                    labelMiddle.Text = "Среднее:" + (paneList[selectedGraph].minList[comboBoxStatis.SelectedIndex] + paneList[selectedGraph].maxList[comboBoxStatis.SelectedIndex]) / 2;
                }
                else
                {
                    labelMax.Text = "Максимум:" + (paneList[selectedGraph].maxList[comboBoxStatis.SelectedIndex] - 1);
                    labelMin.Text = "Минимум:" + paneList[selectedGraph].minList[comboBoxStatis.SelectedIndex];
                    labelMiddle.Text = "Среднее:" + (paneList[selectedGraph].minList[comboBoxStatis.SelectedIndex] + (paneList[selectedGraph].maxList[comboBoxStatis.SelectedIndex] - 1)) / 2;
                }
            }

            if (paneList[selectedGraph].rightBorder - paneList[selectedGraph].leftBorder <= 10000)
            {
                if (paneList[selectedGraph].rightBorder - paneList[selectedGraph].leftBorder <= 5000)
                {
                    if (paneList[selectedGraph].rightBorder - paneList[selectedGraph].leftBorder <= 2500)
                    {
                        if (paneList[selectedGraph].rightBorder - paneList[selectedGraph].leftBorder <= 800)
                        {
                            if (paneList[selectedGraph].rightBorder - paneList[selectedGraph].leftBorder <= 80)
                            {
                                majorStepX = 1;
                                minorStepX = 1;
                                xAxisFormat = "ss";
                            }
                            else
                            {
                                majorStepX = 20;
                                minorStepX = 10;
                                xAxisFormat = "mm:ss";
                            }
                        }
                        else
                        {
                            majorStepX = 120;
                            minorStepX = 120;
                            xAxisFormat = "mm:ss";
                        }
                    }
                    else
                    {
                        majorStepX = 600;
                        minorStepX = 600;
                        xAxisFormat = "HH:mm";
                    }
                }
            }

            DrawGraph(paneList[selectedGraph]);

            labelsOnDesk(labelsOfAxis);
        }

        private void checkedListBoxAxis_SelectedIndexChanged(object sender, EventArgs e) // галочки с именами
        {
            if (checkedListBoxAxis.SelectedIndex != -1)
            {
                if (checkedListBoxAxis.GetItemChecked(checkedListBoxAxis.SelectedIndex) == true)
                {
                    for (int i = 0; i < countOfGraphs; i++)
                    {
                        paneList[i].countOfVisibleAxis1++;
                    }
                    controller.axisIsVisible(checkedListBoxAxis.SelectedIndex, true);
                    for (int i = 0; i < countOfGraphs; i++)
                    {
                        paneList[i].countOfVisibleAxis2++;
                    }
                }
                else
                {
                    for (int i = 0; i < countOfGraphs; i++)
                    {
                        paneList[i].countOfVisibleAxis1--;
                    }
                    controller.axisIsVisible(checkedListBoxAxis.SelectedIndex, false);
                    for (int i = 0; i < countOfGraphs; i++)
                    {
                        paneList[i].countOfVisibleAxis2--;
                    }
                }
            }

        }

        private void checkedListBoxAxis_DoubleMouseClick(object sender, MouseEventArgs e)
        {
            if (checkedListBoxAxis.GetItemChecked(checkedListBoxAxis.SelectedIndex) == true)
            {
                checkedListBoxAxis.SetItemCheckState(checkedListBoxAxis.SelectedIndex, CheckState.Unchecked);
            }
            else
            {
                checkedListBoxAxis.SetItemCheckState(checkedListBoxAxis.SelectedIndex, CheckState.Checked);
            }
        }

        private void buttonShowAll_Click(object sender, EventArgs e) // показать всё
        {
            if(fileOpen == true)
                controller.showAll();
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e) // очистить
        {
            if (fileOpen == true)
                controller.deleteAll();
        }

        public void showedAllAxis(IModel m, ModelEventShowDelete e) // показать всё
        {
            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].visibleAxis = e.showDeleteAxis1;
            }

            for (int i = 0; i < checkedListBoxAxis.Items.Count; i++)
                checkedListBoxAxis.SetItemChecked(i, paneList[0].visibleAxis[i]);

            paneList[0].visibleOfXaxis = true;

            for(int i =0; i< countOfGraphs; i++)
            {
                paneList[i].countOfVisibleAxis1 = paneList[i].countOfAxis;
            }

            DrawGraphMasterChange();

            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].countOfVisibleAxis2 = paneList[i].countOfAxis;
            }

            labelsOnDesk(labelsOfAxis);
        }

        public void deletedAllAxis(IModel m, ModelEventShowDelete e) // очистить
        {
            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].visibleAxis = e.showDeleteAxis1;
            }

            for (int i = 0; i < checkedListBoxAxis.Items.Count; i++)
                checkedListBoxAxis.SetItemChecked(i, paneList[0].visibleAxis[i]);

            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].countOfVisibleAxis1 = 0;
            }

            DrawGraphMasterChange();

            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].countOfVisibleAxis2 = 0;
            }

            labelsOnDesk(labelsOfAxis);
        }

        private void comboBoxStatis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxStatis.SelectedIndex >= 0)
            {
                labelMax.Text = "Максимум:" + paneList[selectedGraph].startMaxList[comboBoxStatis.SelectedIndex];
                labelMin.Text = "Минимум:" + paneList[selectedGraph].startMinList[comboBoxStatis.SelectedIndex];
                labelMiddle.Text = "Среднее:" + (paneList[selectedGraph].startMinList[comboBoxStatis.SelectedIndex] + paneList[selectedGraph].startMaxList[comboBoxStatis.SelectedIndex]) / 2;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e) // выход
        { 
            DialogResult rsl = MessageBox.Show("Вы действительно хотите выйти?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rsl == DialogResult.Yes)
            {
                Application.Exit();

            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void LoadRPTFile(bool rpt)
        {
            openFileDialog1.InitialDirectory = fileCatalog;

            openFileDialog1.Filter = "file (RPT) files (*.rpt)|*.rpt|file (RPT2) files (*.rpt2)|*.rpt2";

            this.ResumeLayout();
        }

        //public void buttonOpenFile_Click(object sender, EventArgs e)
        //{
        //    if (countOfGraphs < 10)
        //    {
        //        controller.getTextFile();

        //        LoadRPTFile(true);

        //        if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
        //        {
        //            countOfGraphs++;
        //        }
        //        int rpt = 0;
        //        FileName.PathFile = openFileDialog1.FileName;

        //        //_pathFileName = null;
        //        _pathFileName = Path.GetFileName(openFileDialog1.FileName);
        //        if (_pathFileName.IndexOf("Ex") > -1)
        //            pathFileName.Add(_pathFileName.Substring(2, _pathFileName.Length - 6));
        //        else
        //            pathFileName.Add(_pathFileName.Substring(0, _pathFileName.Length - 4));

        //        string format = null;
        //        format = Path.GetExtension(openFileDialog1.FileName);
        //        if (format == ".rpt")
        //            rpt = 1;
        //        else
        //            rpt = 2;

        //        this.buttonCalc1.Enabled = true;
        //        this.buttonCalc2.Enabled = true;

        //        fileCatalog = Path.GetDirectoryName(openFileDialog1.FileName);

        //        controller.getFile(FileName.PathFile, openFileDialog1.SafeFileName, rpt);

        //        controller.saveTextFile(fileCatalog, fileCatalog1);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Количество одновременно открытых графиков не больше 10!");
        //    }
        //}

        //public void открытьФайлToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    if (countOfGraphs < 10)
        //    {
        //        if (fileOpen == true)
        //        {
        //            controller.getTextFile(true);
        //        }
        //        else
        //        {
        //            controller.getTextFile(false);
        //        }

        //        LoadRPTFile(true);

        //        if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
        //        {
        //            countOfGraphs++;
        //        }
        //        int rpt = 0;
        //        FileName.PathFile = openFileDialog1.FileName;

        //        //_pathFileName = null;
        //        _pathFileName = Path.GetFileName(openFileDialog1.FileName);
        //        pathFileName.Add(_pathFileName);
        //        //if (_pathFileName.IndexOf("Ex") > -1)
        //        //    pathFileName.Add(_pathFileName.Substring(2, _pathFileName.Length - 6));
        //        //else
        //        //    pathFileName.Add(_pathFileName.Substring(0, _pathFileName.Length - 4));

        //        string format = null;
        //        format = Path.GetExtension(openFileDialog1.FileName);
        //        if (format == ".rpt")
        //            rpt = 1;
        //        else
        //            rpt = 2;

        //        this.buttonCalc1.Enabled = true;
        //        this.buttonCalc2.Enabled = true;

        //        fileCatalog = Path.GetDirectoryName(openFileDialog1.FileName);

        //        controller.getFile(FileName.PathFile, openFileDialog1.SafeFileName, rpt);

        //        controller.saveTextFile(fileCatalog, fileCatalog1);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Количество одновременно открытых графиков не больше 10!");
        //    }

        //}

        private void tabControl2_Select(object sender, TabControlEventArgs e)
        {
            selectedGraph = e.TabPageIndex;

            labelDateOfFile.Text = DateTime.FromOADate(paneList[e.TabPageIndex].dateOfFile).ToString("d");
            labelNameOfFile.Text = paneList[e.TabPageIndex].nameOfFile;
            if (zeroInterval[selectedGraph].Count != 0)
            {
                labelSaveZero.Text = DateTime.FromOADate(zeroInterval[selectedGraph][0]).ToString("HH:mm:ss") + " - " + DateTime.FromOADate(zeroInterval[selectedGraph][1]).ToString("HH:mm:ss");
            }
            else
            {
                labelSaveZero.Text = "не задан";
            }

            textBoxTimeStart.Text = DateTime.FromOADate(paneList[e.TabPageIndex].fileTimeStart).ToString();
            textBoxTimeFinish.Text = DateTime.FromOADate(paneList[e.TabPageIndex].fileTimeFinish).ToString();

            paneList[selectedGraph].maxList = paneList[selectedGraph].startMaxList;
            paneList[selectedGraph].minList = paneList[selectedGraph].startMinList;

            paneList[selectedGraph].nowTimeStart = paneList[selectedGraph].fileTimeStart;
            paneList[selectedGraph].nowTimeFinish = paneList[selectedGraph].fileTimeFinish;

            paneList[selectedGraph].pane.XAxis.Scale.Max = paneList[selectedGraph].minScaleOfMassive;
            paneList[selectedGraph].leftBorder = 0;
            //paneList.paneList[selectedGraph].rightBorder = filtredarrList[selectedGraph][0].Count;

            xAxisFormat = "HH:mm";
            majorStepX = 1200;
            minorStepX = 1200;

            DrawGraph(paneList[selectedGraph]);
        }

        private void comboBoxColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nameOfLabel1 = contextMenuStripColors.SourceControl.Name;
            string[] nameOfLabel2 = new string[paneList[0].countOfAxis];

            for (int i = 0; i < paneList[0].countOfAxis; i++)
            {
                nameOfLabel2[i] = "labelOfAxis" + i;
                if (nameOfLabel1 == nameOfLabel2[i])
                {
                    for (int j = 0; j < paneList[0].countOfAxis; j++)
                        if (contextMenuStripColors.SourceControl.Text == paneList[0].nameOfAxis[j])
                        {
                            controller.axisColor(j, comboBoxColors.SelectedIndex);
                        }
                }
            }
        }

        private void comboBoxColors_Enter(object sender, EventArgs e)
        {
            string nameOfLabel1 = contextMenuStripColors.SourceControl.Name;
            string[] nameOfLabel2 = new string[paneList[0].countOfAxis];
            int startIndex = 0;

            for (int i = 0; i < paneList[0].countOfAxis; i++)
            {
                nameOfLabel2[i] = "labelOfAxis" + i;
                if (nameOfLabel1 == nameOfLabel2[i])
                {
                    for (int j = 0; j < paneList[0].countOfAxis; j++)
                        if (contextMenuStripColors.SourceControl.Text == paneList[0].nameOfAxis[j])
                        {
                            startIndex = j;
                        }
                }
            }

            comboBoxColors.SelectedItem = paneList[0].colorOfAxis[startIndex];

        }

        public void colorGraphChanged(IModel m, ModelEventColor e)
        {
            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].colorOfAxis[e.numberOfAxis1] = e.colorOfAxis1;
            }

            controller.saveTextFile(fileCatalog, fileCatalog1);

            DrawGraphMasterChange();

            labelsOnDesk(labelsOfAxis);
        }

        public void labelsOnDesk(List<System.Windows.Forms.Label> labelOfAxis1)
        {
            if (paneList[0].visibleAxis.Find(visibleAxisEl => visibleAxisEl == true))
            {
                SizeF[] legthOfstring = new SizeF[paneList[0].countOfVisibleAxis1];

                int counter = 0;

                for (int j = 0; j < paneList[0].countOfAxis; j++)
                {
                    if (paneList[0].visibleAxis[j] == true)
                    {
                        labelOfAxis1[counter].Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                        labelOfAxis1[counter].AutoSize = true;
                        labelOfAxis1[counter].Text = paneList[0].nameOfAxis[j];
                        legthOfstring[counter] = TextRenderer.MeasureText(labelOfAxis1[counter].Text, labelOfAxis1[counter].Font);
                        labelOfAxis1[0].Location = new System.Drawing.Point(labelBackGround.Location.X + 3, labelBackGround.Location.Y + 4);
                        labelOfAxis1[counter].Name = String.Format("labelOfAxis{0}", counter.ToString());
                        labelOfAxis1[counter].TabIndex = 12 + counter;
                        labelOfAxis1[counter].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        labelOfAxis1[counter].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        labelOfAxis1[counter].BringToFront();
                        labelOfAxis1[counter].ForeColor = Color.FromName(paneList[0].colorOfAxis[j]);
                        labelOfAxis1[counter].ContextMenuStrip = contextMenuStripColors;
                        Controls.Add(labelOfAxis1[counter]);
                        counter++;
                    }
                }
                if (counter >= 1)
                {
                    for (int j = 1; j < counter; j++)
                    {
                        labelOfAxis1[j].Location = new System.Drawing.Point(labelOfAxis1[j - 1].Location.X + Convert.ToInt32(legthOfstring[j - 1].Width + 9), labelBackGround.Location.Y + 4);
                    }
                }

                labelBackGround.SendToBack();
            }
        }

        private void checkedListBoxFiles_SelectedIndexChanged(object sender, ItemCheckEventArgs e) // галочки с именами файлов
        {
            if (e.CurrentValue == CheckState.Unchecked)
            {
                visibleOfGraph[e.Index] = true;
            }
            else
            {
                visibleOfGraph[e.Index] = false;
            }
        }

        private void buttonCalc1_Click(object sender, EventArgs e) // расчет 6 коэффициентов
        {
            if (fileOpen == true)
            {
                if (paneList[0].countOfAxis < 16)
                {
                    MessageBox.Show("В файле не хватает данных!");
                }
                else
                {
                    controller.aproximation_6(visibleOfGraph);
                    listBoxCoefficients.Items.Add("a0 = " + String.Format("{0:0.000000000000000}", _aa6[0]));
                    listBoxCoefficients.Items.Add("a1 = " + String.Format("{0:0.000000000000000}", _aa6[1]));
                    listBoxCoefficients.Items.Add("a2 = " + String.Format("{0:0.000000000000000}", _aa6[2]));
                    listBoxCoefficients.Items.Add("a11 = " + String.Format("{0:0.000000000000000}", _aa6[3]));
                    listBoxCoefficients.Items.Add("a22 = " + String.Format("{0:0.000000000000000}", _aa6[4]));
                    listBoxCoefficients.Items.Add("a12 = " + String.Format("{0:0.000000000000000}", _aa6[5]));

                    listBoxCoefficients.Items.Add("");

                    TempAproxDensityCheck = true;
                }
            }
        }

        private void buttonCalc2_Click(object sender, EventArgs e) // расчет 3 коэффициентов
        {
            if (fileOpen == true)
            {
                if (paneList[0].countOfAxis < 16)
                {
                    MessageBox.Show("В файле не хватает данных!");
                }
                else
                {
                    controller.aproximation_3(visibleOfGraph);
                    listBoxCoefficients.Items.Add("a0 = " + _aa3[0]);
                    listBoxCoefficients.Items.Add("a1 = " + _aa3[1]);
                    listBoxCoefficients.Items.Add("a2 = " + _aa3[2]);

                    listBoxCoefficients.Items.Add("");

                    TempAproxTicCheck = true;
                }
            }
        }

        private void listBoxCoefficients_SelectedIndexChanged(object sender, EventArgs e) //список самих коэффициентов
        {

        }

        public void aproximation_Axis_6(IModel m, ModelEventAprox_6 e)// расчет коэфициентов
        {
            _aa6 = e.aa6;
            num36 = e._num6;
        }

        public void aproximation_Axis_3(IModel m, ModelEventAprox_3 e)// расчет коэфициентов
        {
            _aa3 = e.aa3;
            num36 = e._num3;
        }

        public int buttonHide_X, buttonHide_Y1, buttonHide_Y2;
        public int labelBackGround_X, labelBackGround_Y1, labelBackGround_Y2;
        public int tabControl2_X, tabControl2_Y1, tabControl2_Y2;
        public int labelOfAxis0_X1, labelOfAxis0_X2, labelOfAxis0_Y1, labelOfAxis0_Y2;
        public int _zedGraph_X, _zedGraph_Y1, _zedGraph_Y2;

        private void Form1_Load_1(object sender, EventArgs e)
        {
            buttonHide_X = buttonHide.Location.X;
            buttonHide_Y1 = buttonHide.Location.Y;
            buttonHide_Y2 = buttonHide.Location.Y + (720 - 512);
            labelBackGround_X = labelBackGround.Location.X;
            labelBackGround_Y1 = labelBackGround.Location.Y;
            labelBackGround_Y2 = labelBackGround.Location.Y + (720 - 512);
            tabControl2_X = tabControl2.Size.Width;
            tabControl2_Y1 = tabControl2.Size.Height;
            tabControl2_Y2 = tabControl2.Size.Height + (680 - 471);
            _zedGraph_X = tabControl2.Size.Width - 12;
            _zedGraph_Y1 = tabControl2.Size.Height - 26;
            _zedGraph_Y2 = tabControl2.Size.Height + (680 - 471);
        }

        private void SizeChange(object sender, EventArgs e)
        {

            if ((buttonHide.Text == "Показать"))
            {

                buttonHide_Y1 = buttonHide.Location.Y - (720 - 510);
                labelBackGround_Y1 = labelBackGround.Location.Y - (720 - 510);
                tabControl2_Y1 = tabControl2.Size.Height - (680 - 469);
                buttonHide_Y2 = buttonHide.Location.Y;
                labelBackGround_Y2 = labelBackGround.Location.Y;
                tabControl2_Y2 = tabControl2.Size.Height;

                if (countOfGraphs >= 0)
                {
                    for (int i = 0; i <= countOfGraphs; i++)
                    {
                        _zedGraph_Y2 = this.tabControl2.Size.Height - 26;
                    }
                }
            }
            else
            {

                buttonHide_Y2 = buttonHide.Location.Y + (720 - 510);
                labelBackGround_Y2 = labelBackGround.Location.Y + (720 - 510);
                tabControl2_Y2 = tabControl2.Size.Height + (680 - 469);
                buttonHide_Y1 = buttonHide.Location.Y;
                labelBackGround_Y1 = labelBackGround.Location.Y;
                tabControl2_Y1 = tabControl2.Size.Height;

                if (countOfGraphs >= 0)
                {
                    for (int i = 0; i <= countOfGraphs; i++)
                    {
                        _zedGraph_Y1 = tabControl2.Size.Height - 26;
                    }
                }
            }

            buttonHide_X = buttonHide.Location.X;
            labelBackGround_X = labelBackGround.Location.X;
            tabControl2_X = tabControl2.Size.Width;

            if (countOfGraphs >= 0)
                _zedGraph_X = tabControl2.Size.Width - 12;

        }

        bool vkl = false;
        private void buttonHide_Click(object sender, EventArgs e)
        {

            if (vkl == false)
            {

                buttonHide.Text = "Показать";
                buttonHide.Location = new System.Drawing.Point(buttonHide_X, buttonHide_Y2);
                tabControl1.Visible = false;
                panel1.Visible = false;
                labelTimeStart.Visible = false;
                textBoxTimeStart.Visible = false;
                labelTimeFinish.Visible = false;
                textBoxTimeFinish.Visible = false;
                labelBackGround.Location = new System.Drawing.Point(labelBackGround_X, labelBackGround_Y2);
                tabControl2.Size = new System.Drawing.Size(tabControl2_X, tabControl2_Y2);
                _zedGraph_X = tabControl2.Size.Width - 12;
                _zedGraph_Y2 = tabControl2.Size.Height - 26;
                for (int i = 0; i < countOfGraphs; i++)
                {
                    _zedGraph[i].Size = new System.Drawing.Size(_zedGraph_X, _zedGraph_Y2);
                }
                if (countOfGraphs > 0)
                {
                    labelsOfAxis[0].Location = new System.Drawing.Point(labelBackGround_X + 3, labelBackGround_Y2 + 4);
                    SizeF[] legthOfstring1 = new SizeF[paneList[0].countOfVisibleAxis1];
                    for (int j = 0; j < paneList[0].countOfVisibleAxis1; j++)
                        legthOfstring1[j] = TextRenderer.MeasureText(labelsOfAxis[j].Text, labelsOfAxis[j].Font);
                    for (int k = 1; k < paneList[0].countOfVisibleAxis1; k++)
                    {
                        labelsOfAxis[k].Location = new System.Drawing.Point(labelsOfAxis[k - 1].Location.X + Convert.ToInt32(legthOfstring1[k - 1].Width + 9), labelBackGround.Location.Y + 4);
                    }

                }
                vkl = true;

            }
            else
            {

                buttonHide.Location = new System.Drawing.Point(buttonHide_X, buttonHide_Y1);
                buttonHide.Text = "Cкрыть";
                tabControl1.Visible = true;
                panel1.Visible = true;
                labelTimeStart.Visible = true;
                textBoxTimeStart.Visible = true;
                labelTimeFinish.Visible = true;
                textBoxTimeFinish.Visible = true;
                labelBackGround.Location = new System.Drawing.Point(labelBackGround_X, labelBackGround_Y1);
                tabControl2.Size = new System.Drawing.Size(tabControl2_X, tabControl2_Y1);
                _zedGraph_X = tabControl2.Size.Width - 12;
                _zedGraph_Y1 = tabControl2.Size.Height - 26;
                for (int i = 0; i < countOfGraphs; i++)
                {
                    _zedGraph[i].Size = new System.Drawing.Size(_zedGraph_X, _zedGraph_Y1);
                }
                if (countOfGraphs > 0)
                {
                    labelsOfAxis[0].Location = new System.Drawing.Point(labelBackGround_X + 3, labelBackGround_Y1 + 4);
                    SizeF[] legthOfstring1 = new SizeF[paneList[0].countOfVisibleAxis1];
                    for (int j = 0; j < paneList[0].countOfVisibleAxis1; j++)
                        legthOfstring1[j] = TextRenderer.MeasureText(labelsOfAxis[j].Text, labelsOfAxis[j].Font);
                    for (int k = 1; k < paneList[0].countOfVisibleAxis1; k++)
                    {
                        labelsOfAxis[k].Location = new System.Drawing.Point(labelsOfAxis[k - 1].Location.X + Convert.ToInt32(legthOfstring1[k - 1].Width + 9), labelBackGround.Location.Y + 4);
                    }

                }

                vkl = false;
            }
        }

        private void buttonCut_Click(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                double leftCut = paneList[selectedGraph].pane.XAxis.Scale.Min;
                double rightCut = paneList[selectedGraph].pane.XAxis.Scale.Max;
                if (rightCut - leftCut >= paneList[selectedGraph].minScaleOfMassive)
                {
                    MessageBox.Show("Выбирите интервал для удаления!");
                }
                else
                {
                    controller.cutAxisNow(leftCut, rightCut, selectedGraph);
                    zeroInterval[selectedGraph].Clear();
                    labelSaveZero.Text = "не задан";
                }
            }
        }

        public void cutedAxis(IModel m, ModelEventCut e)
        {
            //paneList[selectedGraph].drawArrlist = e.ArrList1;
            paneList[selectedGraph].filtredArrlist = e.filtredArrList1;
            paneList[selectedGraph].nowTimeStart = e.nowTimeStart1;
            paneList[selectedGraph].nowTimeFinish = e.nowTimeFinish1;
            paneList[selectedGraph].leftBorder = 0;
            paneList[selectedGraph].rightBorder = e.filtredArrList1[0].Count;
            paneList[selectedGraph].maxList = e.maxList1;
            paneList[selectedGraph].minList = e.minList1;
            paneList[selectedGraph].startMaxList = e.startMaxValue1;
            paneList[selectedGraph].startMinList = e.startMinValue1;
            paneList[selectedGraph].minScaleOfMassive = e.minScaleOfMassive1;
            paneList[selectedGraph].fileTimeStart = e.nowTimeStart1;
            paneList[selectedGraph].fileTimeFinish = e.nowTimeFinish1;

            majorStepX = 1200;
            minorStepX = 1200;
            xAxisFormat = "HH:mm";

            DrawGraph(paneList[selectedGraph]);

            textBoxTimeStart.Text = DateTime.FromOADate(paneList[selectedGraph].fileTimeStart).ToString();
            textBoxTimeFinish.Text = DateTime.FromOADate(paneList[selectedGraph].fileTimeFinish).ToString();
        }

        bool vBool = false;
        int pCount = 0;

        public void Read_Xml(IModel m, ModelEventReadXml e)// расчет коэфициентов
        {
            vBool = false;
            pCount = 0;
            countListDt = e.listDt1.Count;
            _listDt.Clear();
            _listDt = e.listDt1;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = e.dt1;

            for (int i = 0; i < e._p1.Count; i++)
            {
                pCount++;
                dataGridView1.Rows[i].HeaderCell.Value = e._p1[i];// работает
            }

            if (e._p1.Count == pCount)
                vBool = true;
            for (int i = 0; i < e._p1.Count + 1; i++)
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            labelTitleXML.Text = e._sumNameXml1;

            listBoxTar.Items.Clear();

            foreach (string i in e.attribyte1)
                listBoxTar.Items.Add(i);

            xmlFileAdded = true;

        }

        private void buttonClearCoeff_Click(object sender, EventArgs e)
        {
            listBoxCoefficients.Items.Clear();
        }

        private void LoadXml(bool xml)
        {
            openFileDialog2.InitialDirectory = fileCatalog1;

            openFileDialog2.Filter = "file (XML) files (*.xml)|*.xml";

            this.ResumeLayout();
        }
        
        private void buttonOpenXML_Click(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                controller.getTextFile(true);
            }
            else
            {
                controller.getTextFile(false);
            }

            LoadXml(true);
            if (openFileDialog2.ShowDialog() != DialogResult.OK) return;
            {
            }

            FileName.PathFile = openFileDialog2.FileName;

            fileCatalog1 = Path.GetDirectoryName(openFileDialog2.FileName);

            controller.getXml(FileName.PathFile);

            controller.saveTextFile(fileCatalog, fileCatalog1);
        }

        private void listBoxTar_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = _listDt[listBoxTar.SelectedIndex];
        }

        public void error_Count(IModel m, ModelEventErrorCount e)
        {
            int errorNumber = e.errorNumber1;
            switch(errorNumber)
            {
                case 1:
                    {
                        MessageBox.Show("Количество осей открываемого файла и уже открытого не совпадают!");
                        countOfGraphs--;
                        break;
                    }
                case 2:
                    {
                        MessageBox.Show("Запись не соответсвует тарировке!");
                        if (e.errorType1 == "pressure")
                        {
                            PressureCoeffCheck = false;
                        }
                        else if (e.errorType1 == "density")
                        {
                            DensityCoeffCheck = false;
                        }
                        else if(e.errorType1 == "rate")
                        {
                            AllCoeffCheck = false;
                        }
                        break;
                    }
                case 3:
                    {
                        MessageBox.Show("Введите число в пустое поле!");
                        break;
                    }
                case 4:
                    {
                        MessageBox.Show("График имеет полный размер!");
                        break;
                    }
                case 5:
                    {
                        MessageBox.Show("Выберите интервалы нуля для всех графиков!");
                        break;
                    }
                case 6:
                    {
                        MessageBox.Show("Ошибка копирования таблицы density.tar!");
                        break;
                    }
                case 7:
                    {
                        MessageBox.Show("Неверно выбран интервал!");
                        break;
                    }
             }
        }

        private void deleteThisFile(int currentNumber)
        {
            _zedGraph[currentNumber] = null;
            _zedGraph.RemoveAt(currentNumber);
            paneList[currentNumber] = null;
            paneList.RemoveAt(currentNumber);
            zeroInterval.RemoveAt(currentNumber);
            tabControl2.Controls.Remove(_tabPages[currentNumber]);
            checkedListBoxFiles.Items.RemoveAt(currentNumber);
            comboBoxPressure.Items.RemoveAt(currentNumber);
            comboBoxDensity.Items.RemoveAt(currentNumber);
            comboBoxFlowRate.Items.RemoveAt(currentNumber);
            comboBoxRate.Items.RemoveAt(currentNumber);
            controller.deleteFile(currentNumber);
            pathFileName.RemoveAt(currentNumber);
            countOfGraphs--;

            _tabPages.RemoveAt(currentNumber);
            countOfAxis.RemoveAt(currentNumber);
            //arrList.RemoveAt(currentNumber);
            filtredarrList.RemoveAt(currentNumber);
            colorOfAxis.RemoveAt(currentNumber);
            nameOfAxis.RemoveAt(currentNumber);
            minScaleOfMassives.RemoveAt(currentNumber);
            visibleOfAxis.RemoveAt(currentNumber);
            dateOfFiles.RemoveAt(currentNumber);
            nameOfFiles.RemoveAt(currentNumber);
            filesTimeStart.RemoveAt(currentNumber);
            filesTimeFinish.RemoveAt(currentNumber);
            leftBorders.RemoveAt(currentNumber);
            rightBorders.RemoveAt(currentNumber);
            maxLists.RemoveAt(currentNumber);
            minLists.RemoveAt(currentNumber);
            startMaxLists.RemoveAt(currentNumber);
            startMinLists.RemoveAt(currentNumber);
            nowTimeStartList.RemoveAt(currentNumber);
            nowTimeFinishList.RemoveAt(currentNumber);
            visibleOfGraph.RemoveAt(currentNumber);
            ArrAxis.RemoveAt(currentNumber);
        }

        private void buttonDeleteFile_Click(object sender, EventArgs e)
        {
            int numIndex = -1;
            var numIndexList = new List<int>();
            numIndexList.Clear();
            if (fileOpen == true)
            {
                if (checkedListBoxFiles.CheckedIndices.Count == 1)
                {
                    numIndex = checkedListBoxFiles.CheckedIndices[0];
                    DeleteCheckedFiles(numIndex);
                }
                else if (checkedListBoxFiles.CheckedIndices.Count == 0)
                {
                    MessageBox.Show("Файл не выбран!");
                }
                else
                {
                    //MessageBox.Show("Отметьте только один файл!");
                    for(int i = 0; i < checkedListBoxFiles.CheckedIndices.Count; i++)
                        numIndexList.Add(checkedListBoxFiles.CheckedIndices[i]);
                    foreach(var elem in numIndexList)
                        DeleteCheckedFiles(numIndexList[elem]);
                }
                
            }
        }
        //
        /// <summary>
        /// Метод удаляющий файл
        /// </summary>
        /// <param name="numIndex"></param>
        private void DeleteCheckedFiles(int numIndex)
        {
            if (countOfGraphs == 1)
            {
                tabControl2.Selected -= new System.Windows.Forms.TabControlEventHandler(tabControl2_Select);
                for (int i = 0; i < paneList[0].countOfVisibleAxis2; i++)
                {
                    Controls.Remove(labelsOfAxis[i]);
                }

                deleteThisFile(0);

                dataGridViewPressure.Columns.Clear();
                dataGridViewRate.Columns.Clear();
                dataGridViewDensity.Columns.Clear();

                listBoxCoefficients.Items.Clear();
                listBoxPressure1.Items.Clear();
                listBoxDensity.Items.Clear();
                listBoxFlowRate.Items.Clear();
                listBoxRate.Items.Clear();
                checkedListBoxAxis.Items.Clear();
                labelDateOfFile.Text = "";
                labelNameOfFile.Text = "";
                textBoxNowTimeStart.Text = "";
                textBoxNowTimeFinish.Text = "";
                textBoxTimeStart.Text = "";
                textBoxTimeFinish.Text = "";
                labelSaveZero.Text = "не задан";
                comboBoxStatis.Items.Clear();
                countOfLabel = 0;

                TempAproxDensityCheck = false;
                TempAproxTicCheck = false;
                FlowRateCoeffCheck = false;
                PressureCoeffCheck = false;
                DensityCoeffCheck = false;
                AllCoeffCheck = false;
                fileOpen = false;
            }
            else
            {
                //int currentNumber = checkedListBoxFiles.SelectedIndex;
                int currentNumber = numIndex;
                numDeletedGraph = currentNumber;

                deleteThisFile(currentNumber);

                for (int i = currentNumber; i < countOfGraphs; i++)
                {
                    _tabPages[i].Name = "_tabPages" + (i + 1);
                    _tabPages[i].TabIndex = (i + 1);
                    //_tabPages[i].Text = "Файл_" + (i+1);
                    _tabPages[i].Text = pathFileName[i];
                    _zedGraph[i].Name = "_zedGraph" + (i + 1);
                }

                selectedGraph = tabControl2.SelectedIndex;
            }

        }

        public void delete_File(IModel m, ModelEventDeleteFile e)
        {
        }

        public void reCalculate_Graph(IModel m, ModelEventReCalculate e)
        {

            if(reCalcCheck == true)
            {
                RemoveReCalc(
                    e.newMaxList1,
                    e.newMinList1,
                    e.newMaxError1,
                    e.newMinError1);

                RemoveReCalc(
                    e.newMaxList1,
                    e.newMinList1,
                    e.newMax1,
                    e.newMin1);
            }

            FillReCalc(
                //e.newArr1,
                e.newfiltredArr1,
                e.newName1,
                e.newMaxList1,
                e.newMinList1,
                e.newMax1,
                e.newMin1,
                e.newVisible1,
                e.newColor1);

            FillReCalc(
                //e.newArrError1,
                e.newFiltresArrError1,
                e.newNameError1,
                e.newMaxList1,
                e.newMinList1,
                e.newMaxError1,
                e.newMinError1,
                e.newVisible1,
                e.newColorError1);

            DrawGraphMasterChange();

            labelsOnDesk(labelsOfAxis);

            if(reCalcCheck == true)
            {
                if (reCalcAxisCheck == true)
                {
                    for (int i = 0; i < countOfGraphs; i++)
                    {
                        for (int j = 0; j < 2; j++)
                            paneList[i].countOfVisibleAxis2--;
                    }
                }
            }

            reCalcCheck = e.newReCalcCheck1;

            for (int i = 0; i < countOfGraphs; i++)
            {
                for (int j = 0; j < 2; j++)
                    paneList[i].countOfVisibleAxis2++;
            }
        }

        private void numericAxAcura_ValueChanged(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                string value = "F" + numericAxAcura.Value;
                axisAcurracy = value;
                DrawGraph(paneList[selectedGraph]);
            }
        }

        private void numericAxisStep_ValueChanged(object sender, EventArgs e)
        {
            if(fileOpen == true)
            {
                YAxisStep = Convert.ToInt32(numericAxisStep.Value);
                DrawGraph(paneList[selectedGraph]);
            }
        }

        public void FillReCalc(
            //List<PointPairList> newArr,
            List<PointPairList> newFiltredArr,
            string newName,
            List<double> newMaxList,
            List<double> newMinList,
            List<double> newMax,
            List<double> newMin,
            List<bool> newVisible,
            string newColor)
        {
            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].countOfAxis++;
                //paneList[i].drawArrlist.Add(newArr[i]);
                paneList[i].filtredArrlist.Add(newFiltredArr[i]);
                paneList[i].nameOfAxis.Add(newName);
                paneList[i].maxList = newMaxList;
                paneList[i].minList = newMinList;
                paneList[i].maxList[paneList[i].countOfAxis - 1] = newMax[i];
                paneList[i].minList[paneList[i].countOfAxis - 1] = newMin[i];
                paneList[i].startMaxList.Add(newMax[i]);
                paneList[i].startMinList.Add(newMin[i]);
                paneList[i].visibleAxis = newVisible;
                ArrAxis[i].Add(paneList[i].pane.AddYAxis(paneList[i].nameOfAxis[paneList[i].countOfAxis - 1]));
                paneList[i].countOfVisibleAxis1++;
                paneList[i].pane.YAxisList[paneList[i].countOfAxis - 1].MajorGrid.IsZeroLine = false;
                paneList[i].pane.YAxisList[paneList[i].countOfAxis - 1].Title.Text = "";
                paneList[i].pane.YAxisList[paneList[i].countOfAxis - 1].Title.IsOmitMag = true;
                paneList[i].pane.YAxisList[paneList[i].countOfAxis - 1].Scale.Max = paneList[i].maxList[paneList[i].countOfAxis - 1];
                paneList[i].pane.YAxisList[paneList[i].countOfAxis - 1].Scale.Min = paneList[i].minList[paneList[i].countOfAxis - 1];
                paneList[i].pane.YAxisList[paneList[i].countOfAxis - 1].Scale.FontSpec.Size = 10;
                paneList[i].pane.YAxisList[paneList[i].countOfAxis - 1].AxisGap = 1;
            }

            paneList[0].colorOfAxis.Add(newColor);
            checkedListBoxAxis.Items.Add(paneList[0].nameOfAxis[paneList[0].countOfAxis - 1]);
            checkedListBoxAxis.SetItemChecked(paneList[0].countOfAxis - 1, true);
        }

        public void RemoveReCalc(
            List<double> newMaxList,
            List<double> newMinList,
            List<double> newMax,
            List<double> newMin)
        {
            for (int i = 0; i < countOfGraphs; i++)
            {
                paneList[i].countOfAxis--;
                int lastNum = paneList[i].countOfAxis;
                //paneList[i].drawArrlist.RemoveAt(lastNum);
                paneList[i].filtredArrlist.RemoveAt(lastNum);
                paneList[i].nameOfAxis.RemoveAt(lastNum);

                paneList[i].maxList = newMaxList;
                paneList[i].minList = newMinList;

                paneList[i].maxList[paneList[i].countOfAxis] = newMax[i];
                paneList[i].minList[paneList[i].countOfAxis] = newMin[i];

                paneList[i].startMaxList.RemoveAt(lastNum);
                paneList[i].startMinList.RemoveAt(lastNum);

                ArrAxis[i].RemoveAt(lastNum);
                if (paneList[i].countOfVisibleAxis1 != 0)
                {
                    if (reCalcAxisCheck == true)
                    {
                        paneList[i].countOfVisibleAxis1--;
                    }
                }

                paneList[i].pane.CurveList.RemoveAt(lastNum);
                paneList[i].pane.YAxisList.RemoveAt(lastNum);
            }

            paneList[0].colorOfAxis.RemoveAt(paneList[0].countOfAxis);
            checkedListBoxAxis.Items.RemoveAt(paneList[0].countOfAxis);
        }

        int row = -1;
        int column = -1;
        string newValue = "";

        private void SaveXml(bool xml)
        {
            savefileCatalog2 = fileCatalog1;
            saveFileDialog2.InitialDirectory = savefileCatalog2;
            saveFileDialog2.Filter = "file (XML) files (*.xml)|*.xml";
            this.ResumeLayout();
        }

        private void buttonSaveXML_Click(object sender, EventArgs e)// сохранить xml
        {
            SaveXml(true);

            if (saveFileDialog2.ShowDialog() != DialogResult.OK) return;
            {

            }

            FileName.PathFile = saveFileDialog2.FileName;
            savefileCatalog2 = Path.GetDirectoryName(saveFileDialog2.FileName);

            controller.writeXml(FileName.PathFile);
        }

        public void Write_Xml(IModel m, ModelEventWriteXml e)// записать xml
        {

        }

        public void Change_Xml1(IModel m, ModelEventChangeXml e)// изменить xml
        {

        }

        public void Change_Xml2(IModel m, ModelEventChangeXml e)// изменить xml
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)//DataGridViewCellEventArgs e)
        {
            try
            {
                //double n;
                if (vBool == true)
                {
                    row = e.RowIndex;
                    column = e.ColumnIndex;
                    newValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (newValue != "")
                    {
                        buttonSaveXML.Enabled = true;
                        controller.changeXml1(row, column, newValue);
                    }
                    else
                    {
                        newValue = "";
                        buttonSaveXML.Enabled = false;
                        MessageBox.Show("Введите число!");
                        //controller.changeXml(row, column, newValue);
                    }

                }
                else
                    vBool = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)//DataGridViewCellEventArgs e)
        {
            try
            {
                //double n;
                if (vBool == true)
                {
                    row = e.RowIndex;
                    column = e.ColumnIndex;
                    newValue = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (newValue != "")
                    {
                        buttonSaveXML.Enabled = true;
                        controller.changeXml2(row, column, newValue, listBoxTar.SelectedIndex);
                    }
                    else
                    {
                        newValue = "";
                        buttonSaveXML.Enabled = false;
                        MessageBox.Show("Введите число!");
                        //controller.changeXml(row, column, newValue);
                    }

                }
                else
                    vBool = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Некорректное значение, введите число!");
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Некорректное значение, введите число!");
        }

        public void Get_Text_File(IModel m, ModelEventGetTextFile e)
        {
            fileCatalog = e.RPTPath;
            fileCatalog1 = e.XMLPath;
        }

        public void Save_Text_File(IModel m, ModelEventSaveTextFile e)
        {

        }

        private void buttonAproxPressure_Click(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                if (comboBoxPressure.SelectedIndex != -1 && paneList[comboBoxPressure.SelectedIndex].countOfAxis > 15)
                {
                    PressureCoeffCheck = true;
                    controller.aproximationPressure(comboBoxPressure.SelectedIndex);
                }
                else if(comboBoxPressure.SelectedIndex == -1 && paneList[comboBoxPressure.SelectedIndex].countOfAxis > 15)
                {
                    MessageBox.Show("Выберите график для расчета!");
                }
                else if(comboBoxPressure.SelectedIndex != -1 && paneList[comboBoxPressure.SelectedIndex].countOfAxis < 15)
                {
                    MessageBox.Show("В файле не хватает данных!");
                }
            }
        }

        public void aproximation_Pressure(IModel m, ModelEventPressureAprox e)
        {
            string str1 = "a1 = " + String.Format("{0:0.000000000000000}", e.pressureCoeff1[1]);
            string str2 = "a2 = " + String.Format("{0:0.000000000000000}", e.pressureCoeff1[2]);

            _DtPressTic = null;
            _DtPressADC = null;

            listBoxPressure.Items.Add(str1);
            listBoxPressure.Items.Add(str2);
            listBoxPressure.Items.Add("");

            _DtPressTic = e.DtPressTic1;
            _DtPressADC = e.DtPressADC1;
            listBoxPressure1.Items.Clear();
            listBoxPressure1.Items.Add("rotate.tar");
            listBoxPressure1.Items.Add("pressure.tar");
        }

        private void buttonAproxDensity_Click(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                if (TempAproxDensityCheck == true && comboBoxDensity.SelectedIndex != -1)
                {
                    DensityCoeffCheck = true;
                    controller.aproximationDensity(comboBoxDensity.SelectedIndex);
                }
                else if(TempAproxDensityCheck == false)
                {
                    MessageBox.Show("Термокоэффициенты не расчитаны!");
                }
                else if(comboBoxDensity.SelectedIndex == - 1)
                {
                    MessageBox.Show("Выберите график для расчета!");
                }
            }
        }

        public void aproximation_Density(IModel m, ModelEventDensityTar e)
        {

            _DtRoRo0 = null;
            _DtRoTic = null;
            //dataGridView4.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            _DtRoRo0 = e.DtRoRo0_1;
            _DtRoTic = e.DtRoTic_1;
            listBoxDensity.Items.Clear();
            listBoxDensity.Items.Add("density.tar");
            listBoxDensity.Items.Add("rgr.tar");
        }

        private void listBoxDensity_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewDensity.DataSource = null;
            if (listBoxDensity.SelectedIndex == 0)
                dataGridViewDensity.DataSource = _DtRoRo0;
            if (listBoxDensity.SelectedIndex == 1)
                dataGridViewDensity.DataSource = _DtRoTic;
        }

        private void listBoxPressure1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewPressure.DataSource = null;
            if (listBoxPressure1.SelectedIndex == 0)
                dataGridViewPressure.DataSource = _DtPressTic;
            if (listBoxPressure1.SelectedIndex == 1)
                dataGridViewPressure.DataSource = _DtPressADC;
        }

        private void buttonPressureClear_Click(object sender, EventArgs e)
        {
            listBoxPressure.Items.Clear();
        }

        private void buttonFlowRate_Click(object sender, EventArgs e)
        {
            if(fileOpen == true)
            {
                if (TempAproxTicCheck == true && comboBoxFlowRate.SelectedIndex != -1)
                {
                    controller.aproximationFlowRate(comboBoxFlowRate.SelectedIndex);
                }
                else if(TempAproxTicCheck == false)
                {
                    MessageBox.Show("Термокоэффициенты не расчитаны!");
                }
                else if(comboBoxFlowRate.SelectedIndex == - 1)
                {
                    MessageBox.Show("Выберите график для расчета!");
                }
            }
        }

        public void aproximation_FlowRate(IModel m, ModelEventFlowRate e)
        {
            string str1 = "a0 = " + String.Format("{0:0.000000000000000}", e.flowRateCoeff1[0]);
            string str2 = "a1 = " + String.Format("{0:0.000000000000000}", e.flowRateCoeff1[1]);
            string str3 = "a2 = " + String.Format("{0:0.000000000000000}", e.flowRateCoeff1[2]);

            listBoxFlowRate.Items.Add(str1);
            listBoxFlowRate.Items.Add(str2);
            listBoxFlowRate.Items.Add(str3);
            listBoxFlowRate.Items.Add("");

            FlowRateCoeffCheck = true;
        }

        private void listBoxRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewRate.DataSource = null;
            dataGridViewRate.DataSource = _DtqQ;
        }

        private void buttonFlowRateClear_Click(object sender, EventArgs e)
        {
            listBoxFlowRate.Items.Clear();
        }

        private void buttonRate_Click(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                if(comboBoxRate.SelectedIndex != -1 && TempAproxTicCheck == true && FlowRateCoeffCheck == true && PressureCoeffCheck == true && DensityCoeffCheck == true)
                {
                    AllCoeffCheck = true;
                    controller.aproximationRate(comboBoxRate.SelectedIndex);
                }
                else if(comboBoxRate.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите график для расчета!");
                }
                else if(TempAproxTicCheck == false)
                {
                    MessageBox.Show("Термокоэффициенты не расчитаны!");
                }
                else if(FlowRateCoeffCheck == false)
                {
                    MessageBox.Show("Термокоэффициенты постоянного расхода не расчитаны!");
                }
                else if(PressureCoeffCheck == false)
                {
                    MessageBox.Show("Коэффициенты давления не расчитаны!");
                }
                else if(DensityCoeffCheck == false)
                {
                    MessageBox.Show("Коэффициенты плотности не расчитаны!");
                }
            }
        }

        public void aproximation_Rate(IModel m, ModelEventRate e)
        {
            _DtqQ = e.DtqQ_1;
            listBoxRate.Items.Clear();
            listBoxRate.Items.Add("coriol.tar");
        }

        private void buttonReturnRemote_Click(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                controller.returnRemote(selectedGraph);
            }
        }

        public void return_Remote(IModel m, ModelEventReturnRemote e)
        {
            paneList[selectedGraph].filtredArrlist = e.filtredArrList1;
            paneList[selectedGraph].nowTimeStart = e.nowTimeStart1;
            paneList[selectedGraph].nowTimeFinish = e.nowTimeFinish1;
            paneList[selectedGraph].leftBorder = 0;
            paneList[selectedGraph].rightBorder = e.filtredArrList1[0].Count;
            paneList[selectedGraph].maxList = e.maxList1;
            paneList[selectedGraph].minList = e.minList1;
            paneList[selectedGraph].startMaxList = e.startMaxValue1;
            paneList[selectedGraph].startMinList = e.startMinValue1;
            paneList[selectedGraph].minScaleOfMassive = e.minScaleOfMassive1;
            paneList[selectedGraph].fileTimeStart = e.nowTimeStart1;
            paneList[selectedGraph].fileTimeFinish = e.nowTimeFinish1;

            majorStepX = 1200;
            minorStepX = 1200;
            xAxisFormat = "HH:mm";

            DrawGraph(paneList[selectedGraph]);

            textBoxTimeStart.Text = DateTime.FromOADate(paneList[selectedGraph].fileTimeStart).ToString();
            textBoxTimeFinish.Text = DateTime.FromOADate(paneList[selectedGraph].fileTimeFinish).ToString();
        }

        private void buttonNewGraphs_Click(object sender, EventArgs e)
        {
            if(fileOpen == true)
            {
                if (xmlFileAdded == true)
                {
                    controller.addDensityRate(reCalcCheck);
                    newGraphsPainted = true;
                }
                else
                {
                    MessageBox.Show("Заполните XML файл!");
                }
            }
        }

        private void buttonSaveZero_Click(object sender, EventArgs e)
        {
            if(fileOpen == true)
            {
                if (xmlFileAdded == true)
                {
                    double zoomFirst;
                    double zoomSecond;
                    zoomFirst = Math.Abs(paneList[selectedGraph].pane.XAxis.Scale.Min);
                    zoomSecond = Math.Abs(paneList[selectedGraph].pane.XAxis.Scale.Max);

                    controller.saveZeroRate(zoomFirst, zoomSecond, selectedGraph);
                }
                else
                {
                    MessageBox.Show("Заполните XML файл!");
                }
            }
        }

        public void save_ZeroRate(IModel m, ModelEventSaveZeroRate e)
        {
            labelSaveZero.Text = DateTime.FromOADate(e.leftTime).ToString("HH:mm:ss") + " - " + DateTime.FromOADate(e.rightTime).ToString("HH:mm:ss");
            zeroInterval[selectedGraph].Add(e.leftTime);
            zeroInterval[selectedGraph].Add(e.rightTime);
        }

        public void add_DensityAndRate(IModel m, ModelEventAddDensityAndRate e)
        {
            
        }

        private void numericUpDownGraphChange_ValueChanged(object sender, EventArgs e)
        {
            if(dataGridView1.ColumnCount != 0)
            {
                if (aproxChangeInt < numericUpDownGraphChange.Value)
                {
                    aproxChangeInt = numericUpDownGraphChange.Value;
                    dataGridView1.CurrentCell.Value = 1 * Convert.ToDouble(dataGridView1.CurrentCell.Value) / 100 + Convert.ToDouble(dataGridView1.CurrentCell.Value);
                    if (newGraphsPainted == true)
                    {
                        controller.addDensityRate(reCalcCheck);
                    }
                    else
                    {
                        MessageBox.Show("Постройте графики");
                    }
                }
                else
                {
                    aproxChangeInt = numericUpDownGraphChange.Value;
                    dataGridView1.CurrentCell.Value = Convert.ToDouble(dataGridView1.CurrentCell.Value) - 1 * Convert.ToDouble(dataGridView1.CurrentCell.Value) / 100;
                    if (newGraphsPainted == true)
                    {
                        controller.addDensityRate(reCalcCheck);
                    }
                    else
                    {
                        MessageBox.Show("Постройте графики");
                    }
                }
            }
        }

        private void buttonTarToXML_Click(object sender, EventArgs e)
        {
            if (fileOpen == true)
            {
                if (AllCoeffCheck == true)
                    controller.addXMLFile();
                else
                    MessageBox.Show("Расчитайте все коэффициенты!");
            }
        }

        public void add_XMLFile(IModel m, ModelEventReadXml e)
        {
            vBool = false;
            pCount = 0;
            countListDt = e.listDt1.Count;
            _listDt.Clear();
            _listDt = e.listDt1;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = e.dt1;

            for (int i = 0; i < e._p1.Count; i++)
            {
                pCount++;
                dataGridView1.Rows[i].HeaderCell.Value = e._p1[i];// работает
            }

            if (e._p1.Count == pCount)
                vBool = true;
            for (int i = 0; i < e._p1.Count + 1; i++)
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            labelTitleXML.Text = e._sumNameXml1;

            listBoxTar.Items.Clear();

            foreach (string i in e.attribyte1)
                listBoxTar.Items.Add(i);

            xmlFileAdded = true;
        }

        private void buttonDensityToRgr_Click(object sender, EventArgs e)
        {
            if(fileOpen == true)
            {
                if (xmlFileAdded == true)
                {
                    controller.copyDensity();
                }
                else
                {
                    MessageBox.Show("Не найдена таблица density.tar!");
                }
            }
        }

        public void copy_Density(IModel m, ModelEventCopyDensity e)
        {
            for (int i = 0; i < _listDt[e.rgrIndex].Rows.Count - e.zeroRgrIndex; i++)
            {
                _listDt[e.rgrIndex].Rows[i + e.zeroRgrIndex][0] = _listDt[e.densIndex].Rows[i + e.zeroDensIndex][1];
            }
            MessageBox.Show("Проверьте корректность таблицы rgr.tar!");
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (countOfGraphs < 10)
            {
                if (fileOpen == true)
                {
                    controller.getTextFile(true);
                }
                else
                {
                    controller.getTextFile(false);
                }

                LoadRPTFile(true);

                if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
                {
                    countOfGraphs++;
                }
                int rpt = 0;
                FileName.PathFile = openFileDialog1.FileName;

                //_pathFileName = null;
                _pathFileName = Path.GetFileName(openFileDialog1.FileName);
                pathFileName.Add(_pathFileName);
                //if (_pathFileName.IndexOf("Ex") > -1)
                //    pathFileName.Add(_pathFileName.Substring(2, _pathFileName.Length - 6));
                //else
                //    pathFileName.Add(_pathFileName.Substring(0, _pathFileName.Length - 4));

                string format = null;
                format = Path.GetExtension(openFileDialog1.FileName);
                if (format == ".rpt")
                    rpt = 1;
                else
                    rpt = 2;

                this.buttonCalc1.Enabled = true;
                this.buttonCalc2.Enabled = true;

                fileCatalog = Path.GetDirectoryName(openFileDialog1.FileName);

                controller.getFile(FileName.PathFile, openFileDialog1.SafeFileName, rpt);

                controller.saveTextFile(fileCatalog, fileCatalog1);
            }
            else
            {
                MessageBox.Show("Количество одновременно открытых графиков не больше 10!");
            }
        }
    }
}


