using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApp_VS.Views;
using ReactiveUI;
using System;
using System.Reactive;

namespace AvaloniaApp_VS.ViewModels;

/// <summary>
///  This is our ViewModel for the first page
/// </summary>
public class Demo1ViewModel : PageViewModelBase
{
    public Demo1ViewModel()
    {
    }
    public Demo1ViewModel(string Title,string View)
    {
        this.View = View;
        this.Title = Title;
        //OpenImageCmd = ReactiveCommand.Create<int>(_OpenImage);
    }


    /// <summary>
    /// The content of this page
    /// </summary>
    public string Message => "Press \"Next\" to register yourself.";

    public Bitmap Image { get; set; }
    // This is our first page, so we can navigate to the next page in any case
    public override bool CanNavigateNext
    {
        get => true;
        protected set => throw new NotSupportedException();
    }
    // You cannot go back from this page
    public override bool CanNavigatePrevious
    {
        get => false;
        protected set => throw new NotSupportedException();
    }
    /*private string[]? _SelectedFiles;
    public string[]? SelectedFiles
    {
        get { return _SelectedFiles; }
        set { this.RaiseAndSetIfChanged(ref _SelectedFiles, value); }
    }
    
    public ReactiveCommand<int, Unit> OpenImageCmd { get; }
    private void _OpenImage(int page)
    {

    }*/
}