using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using BilderalbumAva.ViewModels;
using ReactiveUI;
using System.Linq;

namespace BilderalbumAva.Views;

public partial class FolderView : ReactiveUserControl<FolderViewModel>
{
    
    public FolderView()
    {
        this.WhenActivated(disposables => { /* Handle view activation etc. */ });
        InitializeComponent();

    }

}