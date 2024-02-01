using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;
using System;
using System.Reflection;
using Microsoft.Maui.Graphics.Platform;
using Syncfusion.Maui.Graphics.Internals;
using Microsoft.Maui.Graphics.Text;

namespace CustomBarSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

#if !ANDROID && !IOS
            Chart.Legend = new ChartLegend();
#endif
        }
    }

    public class ColumnSeriesExt : ColumnSeries
    {
        protected override ChartSegment CreateSegment()
        {
            return new ColumnSegmentExt(); 
        }


        protected override void DrawSeries(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF clipRect)
        {          
            if (segments != null)
            {
                for(int i = 0; i < segments.Count; i++)
                {
                    var segment = segments[i];

                    if(segment is ColumnSegmentExt columnsegment)
                    {
                        columnsegment.Index = i;
                    }
                }
                base.DrawSeries(canvas, segments, clipRect);
            }
        }

    }

    public class ColumnSegmentExt : ColumnSegment
    {
        public int Index { get;set; }
        protected override void Draw(ICanvas canvas)
        {
            if (Series is not ColumnSeriesExt series)
            {
                return;
            }
            var items = series.ItemsSource as ObservableCollection<TradeModel>;

            if (items is not null)
            {
                var item = items[Index];

                var style = new ChartLabelStyle() { FontSize = Device.RuntimePlatform == Device.Android ? 10 : 12, TextColor = Colors.Black, Parent = series.ActualYAxis!.Parent };

                if (series.BindingContext is TradeGoodsData viewmodel)
                {
                    // For calculate the positions
                    viewmodel.CalculatePositions(this, item, style);

                    canvas.DrawText(item.Description, viewmodel.TextX, viewmodel.TextY, style);
                    canvas.DrawLine(viewmodel.LineX1, viewmodel.LineY, viewmodel.LineX2, viewmodel.LineY);
                    canvas.DrawImage(item.Image, viewmodel.ImageX, viewmodel.ImageY, viewmodel.ImageWidth, viewmodel.ImageHeight);
                }  
            }          
            base.Draw(canvas);           
        }
    }
}
