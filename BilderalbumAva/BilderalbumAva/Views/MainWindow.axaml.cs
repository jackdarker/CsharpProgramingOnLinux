using Avalonia;
using Avalonia.Controls;
using BilderalbumAva.ViewModels;
using System.Reactive.Subjects;

namespace BilderalbumAva.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //textBlock2[!TextBlock.TextProperty] = textBlock1[!TextBlock.TextProperty]; 
        //InitialPane[!ContentControl.ContentProperty] = new Subject<string>().ToBinding()
        InitialPane.Content = new FolderViewModel();
    }
    private void Button_AddPane(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var _i = MyGrid.Children.Count;
        var tb1 = new FolderViewModel();
        tb1.CurrentPath = "D:";
        var _con = new ContentControl();
        _con.Content = tb1;
        MyGrid.Children.Insert(_i - 1, _con);
    }
}