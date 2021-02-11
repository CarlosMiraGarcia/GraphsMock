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
        public List<string> ThemesItems { get; set; }


        private int selectedItem;
        public int SelectedItem
        {
            get { return selectedItem; }
            set { OnPropertyChanged(ref selectedItem, value); Reload(); }
        }

        private Plot plt;

        private readonly string ImagePath = "1.png";

        private bool isTick;
        public ICommand ReloadGraphCommand { get; set; }
        public ICommand ShowTicksCommand { get; set; }

        public MainViewModel()
        {
            ReloadGraphCommand = new RelayCommand(param => Reload());
            ShowTicksCommand = new RelayCommand(param => TickPressed());
            RandomSeedText = 12123;
            NumberVariablesText = 3;
            TitleLabelText = "Example Title";
            XLabelText = "Horizontal Units";
            YLabelText = "Vertical Units";
            CountText = 10;
            CreateList();
            CreatePlot();
            PlotProperties();
            Ticks();
            CreateBitmap();
        }

        void CreatePlot()
        {
            plt = new ScottPlot.Plot(600, 400);
        }

        void PlotProperties()
        {
            Random rand = new Random(RandomSeedText);
            int pointCount = CountText;
            int lineCount = NumberVariablesText;

            for (int i = 0; i < lineCount; i++)
                plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));
            plt.Title(TitleLabelText);
            plt.XLabel(XLabelText);
            plt.YLabel(YLabelText);
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
        void Reload()
        {
            CreatePlot();
            ChangeTheme();
            PlotProperties();
            Ticks();
            CreateBitmap();
        }

        void ChangeTheme()
        {
            switch (SelectedItem)
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

        void TickPressed()
        {
            isTick = !isTick;
            Reload();
        }

        void CreateList()
        {
            ThemesItems = new List<string>();
            ThemesItems.Add("Black");
        }
    }
}