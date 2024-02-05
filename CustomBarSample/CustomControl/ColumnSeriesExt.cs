using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Graphics.Internals;
using System.Collections.ObjectModel;

namespace CustomBarSample
{
    public class ColumnSeriesExt : ColumnSeries
    {
        internal ChartLabelStyle LabelStyle { get; set; }

        int index = 0;

        public ColumnSeriesExt()
        {
            LabelStyle = new ChartLabelStyle() { FontSize = DeviceInfo.Platform == DevicePlatform.Android ? 10 : 12, TextColor = Colors.Black };
        }

        protected override ChartSegment CreateSegment()
        {
            var segment = new ColumnSegmentExt();
            segment.Item = GetImageSource(index);
            index++;
            return segment;
        }

        private TradeModel? GetImageSource(int index)
        {
            if (ItemsSource is ObservableCollection<TradeModel> source)
            {
                return source[index];
            }

            return null;
        }

        protected override void DrawSeries(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF clipRect)
        {
            //reset index
            index = 0;
            base.DrawSeries(canvas, segments, clipRect);
        }
    }

    public class ColumnSegmentExt : ColumnSegment
    {
        internal TradeModel? Item { get; set; }

        readonly float imageWidth = DeviceInfo.Platform == DevicePlatform.Android ? 20 : 35;
        readonly float imageOffset = 10;

        readonly PointF textOffset = new PointF(DeviceInfo.Platform == DevicePlatform.Android ? 5 : 10, DeviceInfo.Platform == DevicePlatform.Android ? 5 : 20);
        readonly PointF linePosition = new PointF(DeviceInfo.Platform == DevicePlatform.Android ? 5 : 0, 3);

        const float labelOffset = 30;

        float TextX, TextY;
        float LineX1, LineX2, LineY;
        float ImageX;
        float ImageY;

        protected override void OnLayout()
        {
            base.OnLayout();
            if (Series is ColumnSeriesExt series && Item != null)
            {
                var style = series.LabelStyle;
                style.Parent = series.ActualYAxis!.Parent;
                var size = Item.Description.Measure(style);
                CalculatePositions(size);
            }
        }

        protected override void Draw(ICanvas canvas)
        {
            if (Series is ColumnSeriesExt series)
            {
                //var index = series.GetDataPointIndex(Left + ((Right - Left) / 2),series.SeriesClipRect.Center.Y - ((Bottom + Top) / 2));
                var style = series.LabelStyle;

                if (Item != null)
                {
                    // For calculate the positions
                    canvas.DrawText(Item.Description, TextX, TextY, style);
                    canvas.StrokeColor = Colors.Black;
                    canvas.DrawLine(LineX1, LineY, LineX2, LineY);
                    canvas.DrawImage(Item.Image, ImageX, ImageY, imageWidth, imageWidth);
                }
            }

            base.Draw(canvas);
        }

        internal void CalculatePositions(Size size)
        {
            if (Item != null && Item.Category == "export") // Export
            {
                CalculateExportPositions(size);
            }
            else // Import 
            {
                CalculateImportPositions(size);
            }
        }

        private void CalculateExportPositions(Size size)
        {
            var segmentLength = Right - Left;
            TextX = Left;
            TextY = Top - textOffset.Y;

            LineX1 = Left;
            LineX2 = size.Width > segmentLength ? Left + (float)size.Width : Right + labelOffset;
            LineY = Top - linePosition.Y;

            ImageX = LineX2 + imageOffset;
            ImageY = Top;
        }

        private void CalculateImportPositions(Size size)
        {
            var segmentLength = Right - Left;
            TextX = Right - (float)size.Width - textOffset.X;
            TextY = Top - textOffset.Y;

            LineX1 = size.Width > segmentLength ? (Right - (float)size.Width) - 5 : Left - labelOffset;
            LineX2 = Right;
            LineY = Top - linePosition.Y;

            ImageX = LineX1 - (imageWidth + imageOffset);
            ImageY = Top;
        }
    }
}
