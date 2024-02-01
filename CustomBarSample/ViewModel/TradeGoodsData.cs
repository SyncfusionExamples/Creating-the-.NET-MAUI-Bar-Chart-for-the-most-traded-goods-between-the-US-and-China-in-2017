using Microsoft.Maui.Graphics.Platform;
using Microsoft.VisualBasic;
using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomBarSample
{
    public class TradeGoodsData
    {
        public float ImageWidth { get; set; }
        public float ImageHeight { get; set; }

        public float ImageXPosition { get; set; }

        public float TextXPosition {  get; set; }
        public float TextYPosition {  get; set; }

        public float LineXPosition { get; set; }
        public float LineYPosition { get; set; }

        public float TextX;
        public float TextY;

        public float LineX1;
        public float LineX2;
        public float LineY;

        public float ImageX;
        public float ImageY;

        public ObservableCollection<TradeModel> ImportDataCollection
        {
            get; set;
        }
        public ObservableCollection<TradeModel> ExportDataCollection
        {
            get; set;
        }

        public Dictionary<string, Stream>? Stream { get; set; } = new Dictionary<string, Stream>();

        public List<Brush> CustomBrushes { get; set; }

        public TradeGoodsData()
        {
            ImportDataCollection = new ObservableCollection<TradeModel>();
            ExportDataCollection = new ObservableCollection<TradeModel>();

            Assembly executingAssembly = typeof(App).GetTypeInfo().Assembly;
            Stream? ExportStream = executingAssembly.GetManifestResourceStream("CustomBarSample.Resources.Raw.Export_Data.csv");
            Stream? ImportStream = executingAssembly.GetManifestResourceStream("CustomBarSample.Resources.Raw.Import_Data.csv");

            ReadCSV(ImportDataCollection, ImportStream);
            ReadCSV(ExportDataCollection, ExportStream);
           

            CustomBrushes = new List<Brush>();
            LinearGradientBrush gradientColor1 = new LinearGradientBrush();
            gradientColor1.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 2, Color = Color.FromRgb(249, 135, 197) },
                new GradientStop() { Offset = 1, Color = Color.FromRgb(254, 91, 172) },
                new GradientStop() { Offset = 0, Color = Color.FromRgb(248, 24, 148) }
            }; 

            LinearGradientBrush gradientColor2 = new LinearGradientBrush();
            gradientColor2.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 1, Color = Color.FromRgb(51,153,102) },
                new GradientStop() { Offset = 0, Color = Color.FromRgb(0,204,153) }
            };

            CustomBrushes.Add(gradientColor1);
            CustomBrushes.Add(gradientColor2);

            SetPlatformSize();
        }

        private void ReadCSV(ObservableCollection<TradeModel> dataCollection, Stream? datestream)
        {
            string line;
            List<string> lines = new();

            if(datestream != null)
            {
                using StreamReader reader = new(datestream);
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                lines.RemoveAt(0);
                foreach (var datapoint in lines)
                {
                    string[] data = datapoint.Split(',');

                    var model = new TradeModel(data[9], Convert.ToDouble(data[8]), data[10], data[5]);

                    // add the relevant image to the itemsource
                    SetStream(model);

                    dataCollection.Add(model); 
                }
            }
        }

        private void SetPlatformSize()
        {
            if (Microsoft.Maui.Devices.DeviceInfo.Platform == Microsoft.Maui.Devices.DevicePlatform.Android)
            {
                ImageWidth = 20;
                ImageHeight = 20;

                ImageXPosition = 10;

                TextXPosition = 5;
                TextYPosition = 5;

                LineXPosition = 5;
                LineYPosition = 3;
            }
            else
            {
                ImageWidth = 45;
                ImageHeight = 45;

                ImageXPosition = 10;

                TextXPosition= 10;
                TextYPosition = 20;


                LineXPosition = 0;
                LineYPosition = 3;
            }
        }

        private void SetStream(TradeModel data)
        {
            Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;

            string imageName = $"{data.GoodsName}.png";
            Stream? stream = assembly.GetManifestResourceStream($"CustomBarSample.Resources.Images.{imageName}");

            if (stream != null && data.GoodsName is not null)
            {
                var image = PlatformImage.FromStream(stream);

                data.Image = image;
            }
        }

        internal void CalculatePositions(ColumnSegmentExt columnSegmentExt, TradeModel item, ChartLabelStyle style)
        {
            var size = item.Description.Measure(style);
            var segmentLength = columnSegmentExt.Right - columnSegmentExt.Left;

            if (item.Category == "export") // Export
            {
                // text positions
                TextX = columnSegmentExt.Left;
                TextY = columnSegmentExt.Top - TextYPosition;

                // Line positions
                LineX1 = columnSegmentExt.Left;
                if (size.Width > segmentLength)
                {
                    LineX2 = columnSegmentExt.Left + (float)size.Width;
                }
                else
                {
                    LineX2 = columnSegmentExt.Right + 30;
                }
                LineY = columnSegmentExt.Top - LineYPosition;

                //Image Positions

                ImageX = LineX2 + ImageXPosition;
                ImageY = columnSegmentExt.Top;

            }
            else // Import 
            {
                //text positions
                TextX = columnSegmentExt.Right - (float)size.Width - TextXPosition;
                TextY = columnSegmentExt.Top - TextYPosition;

                // Line positions
                if (size.Width > segmentLength)
                {
                    LineX1 = (columnSegmentExt.Right - (float)size.Width) - 5;
                }
                else
                {
                    LineX1 = columnSegmentExt.Left - 30;
                }
                LineX2 = columnSegmentExt.Right;
                LineY = columnSegmentExt.Top - LineYPosition;

                //Image Positions

                ImageX = LineX1 - ImageWidth - LineXPosition;
                ImageY = columnSegmentExt.Top;
            }
        }
    }
}
