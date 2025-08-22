using BilderalbumAva.ViewModels;
///see https://github.com/AvaloniaUI/Avalonia.Samples/tree/main/src/Avalonia.Samples/Routing/BasicViewLocatorSample
/// <summary>
/// An abstract class for enabling page navigation.
/// </summary>
public abstract class PageViewModelBase : ViewModelBase
{
    public string View {  get;  set; }
    /// <summary>
    /// The Title of this page
    /// </summary>
    public string Title { get; set; }
}