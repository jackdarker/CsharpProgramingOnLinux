using Avalonia.Controls;
using System.Reactive;
using DynamicData;
using ReactiveUI;
using BilderalbumAva.ViewModels;
using Avalonia.ReactiveUI;
namespace BilderalbumAva.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Button_AddPane(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var tb1 = new FolderView();
        var _i = MyGrid.Children.Count;
        
        //Grid.SetRow(tb1, 0);
        //Grid.SetColumn(tb1, _i);
        //Grid.SetColumnSpan(tb1, 1);
        MyGrid.Children.Insert(_i-1,tb1);
    }
}