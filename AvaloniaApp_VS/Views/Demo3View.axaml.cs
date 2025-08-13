using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace AvaloniaApp_VS.Views;

public partial class Demo3View : UserControl
{
    private int _count = 0;
    public Demo3View()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _count++;
        message.Text = "Clicked " +_count.ToString()+" "+ ((Button)sender).Content + "! ";
        if (sender.GetType() ==typeof(Avalonia.Controls.Primitives.ToggleButton))
        {
            message.Text += ((((Avalonia.Controls.Primitives.ToggleButton)sender)?.IsChecked == true) ? "checked" : "not checked");
        }
    }

    private void SplitButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _count++;
        if (sender.GetType() == typeof(SplitButton))
        {
            message.Text = "Clicked " + _count.ToString() + " " + ((SplitButton)sender).Content + "! ";
        }
        else if (sender.GetType() == typeof(MenuItem))
        {
            message.Text = "Clicked " + _count.ToString() + " " + ((MenuItem)sender).Header + "! ";
        }
    }
}