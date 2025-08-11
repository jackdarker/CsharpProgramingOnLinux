using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace AvaloniaApp_VS.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        //Debug.WriteLine($"Click! Celsius={Celsius.Text}");
        Convert();
    }

    private void Convert()
    {
        if (double.TryParse(Celsius.Text, out double C))
        {
            var F = C * (9d / 5d) + 32;
            Fahrenheit.Text = F.ToString("0.0");
        }
        else
        {
            Celsius.Text = "0";
            Fahrenheit.Text = "0";
        }
    }

    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        Convert();
    }
}
