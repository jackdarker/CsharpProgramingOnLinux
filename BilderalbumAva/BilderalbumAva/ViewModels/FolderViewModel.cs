using Avalonia.Media.Imaging;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilderalbumAva.ViewModels
{
    public class Node : ReactiveObject
    {
        public ObservableCollection<Node>? SubNodes { get; }
        public string Title { get; }
        public DirectoryInfo Tag { get; set; }

        public Node(string title)
        {
            Title = title;
        }

        public Node(string title, ObservableCollection<Node> subNodes)
        {
            Title = title;
            SubNodes = subNodes;
        }
        private Boolean _isExpanded = false;
        public Boolean IsExpanded
        {
            get { return _isExpanded; }
            set {
                if (value == true) { AppendChildDirectories(); }
                this.RaiseAndSetIfChanged(ref _isExpanded, value); 
            }
        }
        public void AppendChildDirectories()
        {
            this.SubNodes.Clear();
            DirectoryInfo[] childDirectories;
            if (this.Tag != null)
            {
                try
                {
                    DirectoryInfo RootDirectory = (DirectoryInfo)this.Tag;   //?? fails if dir is CD-Rom and not ready
                    childDirectories = RootDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    childDirectories = new DirectoryInfo[0];
                }
            }
            else
            {   //if Node is ROOT, adding Drives
                string[] Drives = Environment.GetLogicalDrives();
                childDirectories = new DirectoryInfo[Drives.Length];
                int i = 0;
                for (i = 0; i < Drives.Length; i++)
                {
                    childDirectories[i] = new DirectoryInfo(Drives[i]);
                }
            }
            foreach (DirectoryInfo childDirectory in childDirectories)
            {
                Node childNode = new Node(childDirectory.Name,
                    new ObservableCollection<Node>{ new Node("???")  }); //create dummy node to enable expand
                childNode.Tag = childDirectory;
                    //childNode.ContextMenuStrip = ctxMenuTreeView;
                this.SubNodes.Add(childNode);
            }

        }
    }
    public class Item
    {
        public Item(string Name, string Image)
        {
            this.Name = Name;
            this.Image = Image;
        }
        public string Name { get; set; }
        public string Image { get; set; }
    }
    public class FolderViewModel : ViewModelBase
    {
        public FolderViewModel()
        {
            //OpenImageCmd = ReactiveCommand.Create<int>(_OpenImage);
            SelectedNodes = new ObservableCollection<Node>();
            Nodes = new ObservableCollection<Node>{
                new Node("Root",new ObservableCollection<Node>{ new Node("???")
                }) 
            };
            Items = new ObservableCollection<Item>();
            Items.AddRange( [new Item("cat",""), new Item("camel",""), new Item("cow",""), new Item("chameleon","") ]);
            Pages = new ObservableCollection<string>();
            Pages.AddRange(["Page 1"]);
            PageRefreshCommand = ReactiveCommand.Create(_RefreshPage);
            this.WhenAnyValue(x => x.CurrentPage).Where(x => (x >= 0)).InvokeCommand(PageRefreshCommand);
            this.WhenAnyValue(x => x.SelectedNodes)
                .Where(x=>(x.Count>0)).InvokeCommand(PageRefreshCommand);
            //.Where(x => (x>=0))
            //.Throttle(TimeSpan.FromSeconds(.25))

        }
        public ReactiveCommand<Unit, Unit> PageRefreshCommand { get; }
        private void _RefreshPage()
        {
            ;
        }
        public ObservableCollection<Node> Nodes { get; }
        public ObservableCollection<Node> SelectedNodes { get; }

        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<String> Pages { get; }

        private string? _currentPath;
        public string? CurrentPath
        {
            get { return _currentPath; }
            set { this.RaiseAndSetIfChanged(ref _currentPath, value); }
        }
        private int _currentPage = -1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }
    }
    }
