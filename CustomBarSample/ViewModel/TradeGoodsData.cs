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
        }

        private void ReadCSV(ObservableCollection<TradeModel> dataCollection, Stream? datestream)
        {
            string? line;
            List<string> lines = new();

            if (datestream != null)
            {
                using StreamReader reader = new(datestream);
                while ((line = reader?.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                
                lines.RemoveAt(0);
                
                foreach (var datapoint in lines)
                {
                    string[] data = datapoint.Split(',');

                    var model = new TradeModel(data[9], Convert.ToDouble(data[8]), data[10], data[5]);

                    // add the relevant image to the itemsource
                    UpdateImage(model);

                    dataCollection.Add(model);
                }
            }
        }

        private void UpdateImage(TradeModel data)
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
    }
}
