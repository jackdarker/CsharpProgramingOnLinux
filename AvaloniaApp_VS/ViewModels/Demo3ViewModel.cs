using ReactiveUI;
using System;
using System.ComponentModel.DataAnnotations;

namespace AvaloniaApp_VS.ViewModels;

public class Demo3ViewModel : PageViewModelBase
{
    public Demo3ViewModel()
    {
    }

    public string Message => "very basic Example of checking input fields.";


    public override bool CanNavigateNext
    {
        get => true;
        protected set => throw new NotSupportedException();
    }


    public override bool CanNavigatePrevious
    {
        get => true;
        protected set => throw new NotSupportedException();
    }

}