using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;

namespace AvaloniaApp_VS.Views;

public partial class Demo7View : UserControl
{
    
    public Demo7View()
    {
        InitializeComponent();

    }

    private void TabItem_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        var tab = ((TabItem)sender);

        //tab.IsSelected=true;
        message.Text = tab.Header.ToString();
    }

    private void TabStrip_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        //if(message!=null) message.Text = e.AddedItems[0].ToString();
        //((Avalonia.Controls.Primitives.TabStrip)sender).SelectedItem = e.AddedItems[0];
        //((TabItem)e.AddedItems[0]).IsSelected=true;
    }
}