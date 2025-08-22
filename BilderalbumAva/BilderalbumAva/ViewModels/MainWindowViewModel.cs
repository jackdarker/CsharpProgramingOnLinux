using BilderalbumAva.Views;
using BilderalbumAva.ViewModels;
using DynamicData;
using ReactiveUI;
using System;
using System.Reactive;
using System.Windows.Input;

namespace BilderalbumAva.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<int, Unit> NavigateCommand { get; }
        public MainWindowViewModel()
        {
            string DefaultView = "";//for opening Model with different view; see ViewLocator
            Pages = [ new FolderViewModel()];

            // Set current page to first on start up
            _CurrentPage = Pages[0];

            // Create Observables which will activate to deactivate our commands based on CurrentPage state
            /*var canNavNext = this.WhenAnyValue(x => x.CurrentPage.CanNavigateNext);
            var canNavPrev = this.WhenAnyValue(x => x.CurrentPage.CanNavigatePrevious);

            NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
            NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, canNavPrev);
            NavigateCommand = ReactiveCommand.Create<int>(_Navigate);*/
        }

        object content = new FolderViewModel();
        public object ContentView
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
        private void _Navigate(int page)
        {

            CurrentPage = Pages[page];
        }
        private ViewModelBase[] Pages;

        // The default is the first page
        private ViewModelBase _CurrentPage;

        /// <summary>
        /// Gets the current page. The property is read-only
        /// </summary>
        public ViewModelBase CurrentPage
        {
            get { return _CurrentPage; }
            private set { this.RaiseAndSetIfChanged(ref _CurrentPage, value); }
        }

        /// <summary>
        /// Gets a command that navigates to the next page
        /// </summary>
        public ICommand NavigateNextCommand { get; }

        private void NavigateNext()
        {
            // get the current index and add 1
            var index = Pages.IndexOf(CurrentPage) + 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = Pages[index];
        }

        /// <summary>
        /// Gets a command that navigates to the previous page
        /// </summary>
        public ICommand NavigatePreviousCommand { get; }

        private void NavigatePrevious()
        {
            // get the current index and subtract 1
            var index = Pages.IndexOf(CurrentPage) - 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = Pages[index];
        }
    }
}
