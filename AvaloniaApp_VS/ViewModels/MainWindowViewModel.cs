using AvaloniaApp_VS.Views;
using DynamicData;
using ReactiveUI;
using System;
using System.Reactive;
using System.Windows.Input;

namespace AvaloniaApp_VS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<int, Unit> NavigateCommand { get; }
        public MainWindowViewModel()
        {
            // Set current page to first on start up
            _CurrentPage = Pages[0];

            // Create Observables which will activate to deactivate our commands based on CurrentPage state
            var canNavNext = this.WhenAnyValue(x => x.CurrentPage.CanNavigateNext);
            var canNavPrev = this.WhenAnyValue(x => x.CurrentPage.CanNavigatePrevious);

            NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
            NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, canNavPrev);
            NavigateCommand = ReactiveCommand.Create<int>(_Navigate);
        }
        private void _Navigate(int page)
        {
            if(page==99)
            {
                (new AboutView()).Show();
            }
            else CurrentPage = Pages[page];
        }
        // A read.only array of possible pages
        private readonly PageViewModelBase[] Pages =
        {
            new Demo1ViewModel(),
            new Demo3ViewModel(),
            new Demo2ViewModel(),
            new AboutViewModel()
        };

        // The default is the first page
        private PageViewModelBase _CurrentPage;

        /// <summary>
        /// Gets the current page. The property is read-only
        /// </summary>
        public PageViewModelBase CurrentPage
        {
            get { return _CurrentPage; }
            private set { this.RaiseAndSetIfChanged(ref _CurrentPage, value); }
        }
        public ICommand Navigate1Command { get; }
        public ICommand Navigate2Command { get; }
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
