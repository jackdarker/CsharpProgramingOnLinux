using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AvaloniaApp_VS.Views;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace AvaloniaApp_VS.ViewModels;

/// <summary>
///  This is our ViewModel for the first page
/// </summary>
public class Demo1ViewModel : PageViewModelBase
{

    public class Node
    {
        public ObservableCollection<Node>? SubNodes { get; }
        public string Title { get; }

        public Node(string title)
        {
            Title = title;
        }

        public Node(string title, ObservableCollection<Node> subNodes)
        {
            Title = title;
            SubNodes = subNodes;
        }
    }
    public Demo1ViewModel()
    {
    }
    public Demo1ViewModel(string Title,string View)
    {
        this.View = View;
        this.Title = Title;
        //OpenImageCmd = ReactiveCommand.Create<int>(_OpenImage);
        SelectedNodes = new ObservableCollection<Node>();
        Nodes = new ObservableCollection<Node>
            {
                new Node("Animals", new ObservableCollection<Node>
                {
                    new Node("Mammals", new ObservableCollection<Node>
                    {
                        new Node("Lion"), new Node("Cat"), new Node("Zebra")
                    })
                }),
                new Node("Birds", new ObservableCollection<Node>
                {
                    new Node("Robin"), new Node("Condor"),
                    new Node("Parrot"), new Node("Eagle")
                }),
                new Node("Insects", new ObservableCollection<Node>
                {
                    new Node("Locust"), new Node("House Fly"),
                    new Node("Butterfly"), new Node("Moth")
                }),
            };

        var moth = Nodes.Last().SubNodes?.Last();
        if (moth != null) SelectedNodes.Add(moth);
    }

    public ObservableCollection<Node> Nodes { get; }
    public ObservableCollection<Node> SelectedNodes { get; }
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