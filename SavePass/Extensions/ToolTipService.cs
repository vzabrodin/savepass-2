using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MaterialDesignThemes.Wpf;

namespace SavePass.Extensions
{
    public static class ToolTipService
    {
        public static async void Show(string text, int duration = 5000)
        {
            ToolTip tt = new ToolTip { Content = text, Placement = PlacementMode.Mouse };
            await tt.ShowDialog(text);
            //System.Windows.Controls.ToolTipService.SetShowDuration(tt, duration);
        }
    }
}
