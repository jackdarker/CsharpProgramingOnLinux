using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace BilderalbumAva.ViewModels
{
    public class Node : ReactiveObject
    {
        public ObservableCollection<Node>? SubFolders { get; }
        public string Title { get; }
        public DirectoryInfo Tag { get; set; }

        public Node(string title)
        {
            Title = title;
        }

        public Node(string title, ObservableCollection<Node> subFolders)
        {
            Title = title;
            SubFolders = subFolders;
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
            this.SubFolders.Clear();
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
                this.SubFolders.Add(childNode);
            }

        }
    }
    public class Item
    {
        public Item(string Name)
        {
            this.Name = Name;
        }
        public string Name { get; set; }
        public FileInfo Tag { get; set; }
        public string Image { get; set; }
    }

    public class FolderViewModel : ViewModelBase, IActivatableViewModel
    {
        static int ITEMSPERPAGE = 5;
        private enum SortOrder
        {
            NameAsc = 1,
            NameDesc = 2,
            DateAsc = 3,
            DateDesc = 4
        }
        public ViewModelActivator Activator { get; }
        public FolderViewModel()
        {
            SelectedFolders = new ObservableCollection<Node>();
            Folders = new ObservableCollection<Node>{
                new Node("Root",new ObservableCollection<Node>{ new Node("???")
                })
            };
            Items = new ObservableCollection<Item>();
            Items.AddRange([new Item("cat"), new Item("camel"), new Item("cow"), new Item("chameleon")]);
            Pages = new ObservableCollection<string>();
            Pages.AddRange(["Page 1"]);
            PageRefreshCommand = ReactiveCommand.Create<int>(_RefreshPage);
            FolderChangeCommand = ReactiveCommand.Create(_RefreshItems);
            Activator = new ViewModelActivator();
            this.WhenActivated((CompositeDisposable disposables) =>
            {
                this.WhenAnyValue(x => x.CurrentPage).Where(x => (x >= 0)).InvokeCommand(PageRefreshCommand);
                this.WhenAnyValue(x => x.CurrentPath).Where(x => (x != null && x.Exists)).Select(items => -1).InvokeCommand(PageRefreshCommand);
                /*this.WhenAnyValue(x => x.SelectedFolders)//[0].Tag)
                    .Where(x => (x != null))
                    .
                    .InvokeCommand(FolderChangeCommand);*/
                //this.SelectedFolders.CollectionChanged += SelectedFolders_CollectionChanged;
                this.SelectedFolders
                    // Convert the collection to a stream of chunks,
                    // so we have IObservable<IChangeSet<TKey, TValue>>
                    // type also known as the DynamicData monad.
                    .ToObservableChangeSet(x => x)
                    // Each time the collection changes, we get
                    // all updated items at once.
                    .ToCollection()
                    .Where(wheremethod)
                    .Select(selectMethod)
                    .BindTo(this, x => x.CurrentPath);
                    //.ToProperty(this, x => x.CurrentPath);
                //.Select(items => Unit.Default)
                //.InvokeCommand(FolderChangeCommand);

                /* handle activation TODO */
                Disposable
                    .Create(() => { /* handle deactivation */ })
                    .DisposeWith(disposables);
            });
        
        }

        private static DirectoryInfo selectMethod( IReadOnlyCollection<Node> items)
        {
            return (items.Last().Tag);
        }

        private static bool wheremethod(IReadOnlyCollection<Node> items)
        {
            return (items.Any());
        }
        private void SelectedFolders_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //FolderChangeCommand.Execute();
            _RefreshItems();
        }

        public ReactiveCommand<int, Unit> PageRefreshCommand { get; }
        public ReactiveCommand<Unit, Unit> FolderChangeCommand { get; }

        public ObservableCollection<Node> Folders { get; }
        public ObservableCollection<Node> SelectedFolders { get; }

        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<String> Pages { get; }

        private DirectoryInfo? _currentPath;
        public DirectoryInfo? CurrentPath
        {
            get { return _currentPath; }
            set { this.RaiseAndSetIfChanged(ref _currentPath, value); }
        }
        private int _currentPage = 0;
        public int CurrentPage
        {
            get { return _currentPage; }
            set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        private SortOrder m_SortOrder = SortOrder.NameAsc;
        private DirectoryInfo m_PreviousDirectory = null;
        private int m_PreviousPage = -1;
        private int m_FilesInDirectory = 0;

        private void _RefreshPage(int ActiveIndex)    //#hack: this is bound to CurrentPage but also button up/down and so we need some INT for direction
        {
            ;
            m_FilesInDirectory = GetImagesInDirectory(CurrentPath).Length; //
            int _SelectedPage = CurrentPage;
            int _CurrPageCount = Pages.Count;
            int _NewPageCount = (m_FilesInDirectory / ITEMSPERPAGE) +
                (((m_FilesInDirectory % ITEMSPERPAGE) == 0) ? 0 : 1);
            _NewPageCount = Math.Max(1, _NewPageCount);

            int _Pages = Math.Max(_CurrPageCount, _NewPageCount);
            bool _ManualUpdate = true;
            for (int i = 0; i < _Pages; i++)
            {
                string _PageText = string.Format("{0:D} of {1:D} ({2:D}Files)", (i + 1), _NewPageCount, m_FilesInDirectory);
                if (i >= _NewPageCount) //remove unused pages
                {
                    Pages.RemoveAt(Pages.Count - 1);
                    _ManualUpdate = false;
                }
                else if (i >= _CurrPageCount)  //add Page
                {
                    Pages.Add(_PageText);
                    _ManualUpdate = false;
                }
                else if (Pages[i].ToString() != _PageText) //rename page
                {
                    Pages[i] = _PageText;    //will trigger SelectedIndexChanged-event??
                    _ManualUpdate = false;
                }
            }
            //if Directory changes, select page1, else select active or previous page
            //will trigger preview creation
            if (ActiveIndex < 0)
            {
                CurrentPage= Math.Min(_SelectedPage, Pages.Count - 1);
            }
            else
            {
                CurrentPage = Math.Min(ActiveIndex, Pages.Count - 1);
            }
            //if (_ManualUpdate) 
                _RefreshItems();    //if neither index nor text changed, trigger manually
        }
        private void _RefreshItems()
        {
            DirectoryInfo Dir = CurrentPath;
            int Page = CurrentPage;
            if (m_PreviousDirectory != Dir || m_PreviousPage != Page)
            {//dont delete & recreate images if directory not changed
                Items.Clear();
            }
            m_PreviousDirectory = Dir;
            m_PreviousPage = Page;
            
            if (Dir != null)
            {   //filter images from folder
                FileInfo[] Files = GetImagesInDirectory(Dir);
                for (int i = Page * ITEMSPERPAGE; (i < Files.Length && i < Page * ITEMSPERPAGE + ITEMSPERPAGE); i++)
                {
                    Item FileItem = new Item(Files[i].Name);
                    FileItem.Tag = Files[i];
                    Items.Add(FileItem);
                }
                //update page-list
            }
        }
        private int fileInfoComparer(FileInfo fi1, FileInfo fi2)
        {
            int Result = -1;
            switch (m_SortOrder)
            {
                case SortOrder.NameDesc:
                    Result = fi2.Name.CompareTo(fi1.Name);
                    break;
                case SortOrder.DateAsc:
                    if (fi1.LastWriteTime > fi2.LastWriteTime) Result = 1;
                    else if (fi1.LastWriteTime == fi2.LastWriteTime) Result = 0;
                    break;
                case SortOrder.DateDesc:
                    if (fi1.LastWriteTime < fi2.LastWriteTime) Result = 1;
                    else if (fi1.LastWriteTime == fi2.LastWriteTime) Result = 0;
                    break;
                default:
                    Result = fi1.Name.CompareTo(fi2.Name);
                    break;
            }
            return Result;
        }
        private FileInfo[] GetImagesInDirectory(DirectoryInfo Dir)
        {
            FileInfo[] AllFiles = new FileInfo[0];
            FileInfo[] Files = new FileInfo[0];
            if (Dir != null)
            {
                try
                {
                    AllFiles = Dir.GetFiles("*", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    ;   //IO or AccessException
                }
                Comparison<FileInfo> comp = new Comparison<FileInfo>(fileInfoComparer);
                Array.Sort(AllFiles, comp);
                int[] FileOk = new int[AllFiles.Length];
                int NumberFiles = 0;
                string Extension;
                for (int i = 0; i < AllFiles.Length; i++)
                {
                    FileOk[NumberFiles] = -1;
                    Extension = AllFiles[i].Extension.ToLower();
                    if (Extension == ".jpg" ||
                        Extension == ".jpeg" ||
                        Extension == ".bmp" ||
                        Extension == ".gif" ||
                        Extension == ".tga" ||
                        Extension == ".png")
                    {
                        FileOk[NumberFiles] = i;
                        NumberFiles++;
                    }
                }
                Files = new FileInfo[NumberFiles];
                for (int i = 0; i < NumberFiles; i++)
                {
                    Files[i] = AllFiles[FileOk[i]];
                }
            }
            return Files;
        }
        private async void CreateThumb(FileInfo file)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var _file = await topLevel.StorageProvider.TryGetFileFromPathAsync(file.FullName);

                    // Open reading stream from the first file.
                    await using var stream = await _file.OpenReadAsync();
                using var streamReader = new StreamReader(stream);
                // Reads all the content of file as a text.
                //var fileContent = await streamReader.ReadToEndAsync();
                MyImage.Source = new Bitmap(streamReader.BaseStream);
            }
        }
    }
    }
