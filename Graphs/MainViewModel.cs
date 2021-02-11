using ScottPlot;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Graphs
{
    public class MainViewModel : BaseViewModel
    {
        private BitmapImage imagePath;
        public BitmapImage ImagePath
        {
            get { return imagePath; }
            set { OnPropertyChanged(ref imagePath, value); }
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

        private ScottPlot.Plot plt;
        public ICommand ReloadGraphCommand { get; set; }


        public MainViewModel()
        {
            ReloadGraphCommand = new RelayCommand(param => Reload());

            plt = new ScottPlot.Plot(600, 400);
            Random rand = new Random(0);
            int pointCount = (int)1e6;
            int lineCount = 5;

            for (int i = 0; i < lineCount; i++)
                plt.PlotSignal(DataGen.RandomWalk(rand, pointCount));

            plt.Title("Signal Plot Quickstart (5 million points)");
            plt.YLabel("Vertical Units");
            plt.XLabel("Horizontal Units");
            CreateBitmap();
        }


        void CreateBitmap()
        {            
            // create an image and dispose memory stream
            var image = new BitmapImage();
            plt.SaveFig("1.png");

            using (var fs = new FileStream("1.png", FileMode.Open))
            {
                image.BeginInit();
                image.StreamSource = fs;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
            }
            image.Freeze();
            ImagePath = image;
        }

        void Reload()
        {
            plt.Title(TitleLabelText);
            plt.XLabel(XLabelText);
            plt.YLabel(YLabelText);
            CreateBitmap();
        }
    }
}