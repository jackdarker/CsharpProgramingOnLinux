
using System;

namespace AvaloniaApp_VS.ViewModels
{
    public class AboutViewModel : PageViewModelBase
    {
        public AboutViewModel(string Title, string View)
        {
            this.View = View;
            this.Title = Title;
        }
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
    }
}
