using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Graphics.Internals;
using UiImage = Microsoft.Maui.Graphics.IImage;
using Microsoft.Maui.Controls.Shapes;
using static Microsoft.Maui.Controls.Button.ButtonContentLayout;

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
}
