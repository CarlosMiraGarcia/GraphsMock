using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Graphs
{
    public class MainViewModel : BaseViewModel
    {
        private BitmapImage imageGraph;
        public BitmapImage ImageGraph
        {
            get { return imageGraph; }
            set { OnPropertyChanged(ref imageGraph, value); }
        }
            
        private string xLabelText;
        public string XLabelText
        {
            get { return xLabelText; }
            set { OnPropertyChanged(ref xLabelText, value); }
        }

        private string yLabelText;
        public string YLabelText
        {
            get { return yLabelText; }
            set { OnPropertyChanged(ref yLabelText, value); }
        }

        private string titleLabelText;
        public string TitleLabelText
        {
            get { return titleLabelText; }
            set { OnPropertyChanged(ref titleLabelText, value); }
        }

        private int numberVariablesText;
        public int NumberVariablesText
        {
            get { return numberVariablesText; }
            set { OnPropertyChanged(ref numberVariablesText, value); }
        }        
        
        private int randomSeedText;
        public int RandomSeedText
        {
            get { return randomSeedText; }
            set { OnPropertyChanged(ref randomSeedText, value); }
        }

        private int countText;
        public int CountText
        {
            get { return countText; }
            set { OnPropertyChanged(ref countText, value); }
        }

        private int selectedTheme;
        public int SelectedTheme
        {
            get { return selectedTheme; }
            set { OnPropertyChanged(ref selectedTheme, value); LoadPlot(); }
        }        
        
        private int selectedLineStyle;
        public int SelectedLineStyle
        {
            get { return selectedLineStyle; }
            set { OnPropertyChanged(ref selectedLineStyle, value); LoadPlot(); }
        }
        private float xSpacingText;
        public float XSpacingText
        {
            get { return xSpacingText; }
            set { OnPropertyChanged(ref xSpacingText, value); }
        }

        private float ySpacingText;
        public float YSpacingText
        {
            get { return ySpacingText; }
            set { OnPropertyChanged(ref ySpacingText, value); }
        }

        private Plot plt;

        private readonly string ImagePath = "1.png";

        private bool isTick;
        private bool isGrid;
        public ICommand ReloadGraphCommand { get; set; }
        public ICommand ShowTicksCommand { get; set; }
        public ICommand ShowGridCommand { get; set; }

        public MainViewModel()
        {
            ReloadGraphCommand = new RelayCommand(param => LoadPlot());
            ShowTicksCommand = new RelayCommand(param => TickPressed());
            ShowGridCommand = new RelayCommand(param => GridPressed());
            RandomSeedText = 12123;
            NumberVariablesText = 3;
            CountText = 10;
            TitleLabelText = "Example Title";
            XLabelText = "Horizontal Units";
            YLabelText = "Vertical Units";
            XSpacingText = 1;
            YSpacingText = .4f;
            LoadPlot();
        }
        void CreatePlot()
        {
            plt = new ScottPlot.Plot(600, 400);
            plt.Title(TitleLabelText);
            plt.XLabel(XLabelText);
            plt.YLabel(YLabelText);
            plt.Grid(lineStyle: LineStyle.Dash);
            plt.Grid(xSpacing: XSpacingText, ySpacing: YSpacingText);
            plt.Grid(lineWidth: 2);
        }
        void PlotProperties()
        {
            //Random rand = new Random(RandomSeedText);

            //PlotHistogram();
            PlotGraph();

            //// Graph visualization
            //int pointCount = CountText;
            //int lineCount = NumberVariablesText;
            //for (int i = 0; i < lineCount; i++)
            //    plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));

            
        }
        void CreateBitmap()
        {            
            // create an image and dispose memory stream
            var image = new BitmapImage();
            plt.SaveFig(ImagePath);

            using (var fs = new FileStream(ImagePath, FileMode.Open))
            {
                image.BeginInit();
                image.StreamSource = fs;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
            }
            image.Freeze();
            ImageGraph = image;
        }
        void LoadPlot()
        {
            CreatePlot();
            PlotProperties();
            ThemeSelection();
            LineStyleSelection();
            Ticks();
            Grid();
            CreateBitmap();
        }
        void ThemeSelection()
        {
            switch (SelectedTheme)
            {
                case 0:
                    plt.Style(ScottPlot.Style.Default);
                    break;
                case 1:
                    plt.Style(ScottPlot.Style.Seaborn);
                    break;              
                case 2:
                    plt.Style(ScottPlot.Style.Control);
                    break;              
                case 3:
                    plt.Style(ScottPlot.Style.Blue1);
                    break;              
                case 4:
                    plt.Style(ScottPlot.Style.Blue2);
                    break;              
                case 5:
                    plt.Style(ScottPlot.Style.Blue3);
                    break;                
                case 6:
                    plt.Style(ScottPlot.Style.Light1);
                    break;                
                case 7:
                    plt.Style(ScottPlot.Style.Light2);
                    break;                
                case 8:
                    plt.Style(ScottPlot.Style.Gray1);
                    break;                
                case 9:
                    plt.Style(ScottPlot.Style.Gray2);
                    break;                
                case 10:
                    plt.Style(ScottPlot.Style.Black);
                    break;
                default:
                    plt.Style(ScottPlot.Style.Default);
                    break;
            }
        }                
        void LineStyleSelection()
        {
            switch (SelectedLineStyle)
            {
                case 0:
                    plt.Grid(lineStyle: LineStyle.Dash);
                    break;
                case 1:
                    plt.Grid(lineStyle: LineStyle.DashDot);
                    break;              
                case 2:
                    plt.Grid(lineStyle: LineStyle.DashDotDot);
                    break;              
                case 3:
                    plt.Grid(lineStyle: LineStyle.Dot);
                    break;              
                case 4:
                    plt.Grid(lineStyle: LineStyle.None);
                    break;              
                case 5:
                    plt.Grid(lineStyle: LineStyle.Solid);
                    break;                
                default:
                    plt.Grid(lineStyle: LineStyle.Dash);
                    break;
            }
        }
        void Ticks()
        {
            if (isTick)
            {
                plt.Ticks(displayTicksX: false);
                plt.Ticks(displayTicksY: false);
            }

            else
            {
                plt.Ticks(displayTicksX: true);
                plt.Ticks(displayTicksY: true);
            }
        }   
        void Grid()
        {
            if (isGrid)
            {
                plt.Grid(enable: false);
            }

            else
            {
                plt.Grid(enable: true);
            }
        }
        void TickPressed()
        {
            isTick = !isTick;
            LoadPlot();
        }    
        void GridPressed()
        {
            isGrid = !isGrid;
            LoadPlot();
        }
        void PlotHistogram()
        {
            double[] values = { 0, 20, 25, 28, 39, 59 };
            var his = new ScottPlot.Statistics.Histogram(values, min: 0, max: 70);
            double barWidth = his.binSize * 1.2;
            plt.PlotBar(his.bins, his.countsFracCurve, barWidth: barWidth, outlineWidth: 0);
            plt.PlotScatter(his.bins, his.countsFracCurve, markerSize: 0, lineWidth: 2, color: Color.Black);
            plt.Axis(null, null, 0, null);
        }
        void PlotGraph()
        {
            Random rand = new Random(RandomSeedText);
            int pointCount = CountText;
            int lineCount = NumberVariablesText;
            for (int i = 0; i < lineCount; i++)
                plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));
        }
    }
}