
using System;

namespace AvaloniaApp_VS.ViewModels
{
    public class AboutViewModel : PageViewModelBase
    {

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
