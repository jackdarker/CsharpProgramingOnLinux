using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System.IO;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;

namespace AvaloniaApp_VS.Views;

public partial class Demo4View : UserControl
{
    public Demo4View()
    {
        InitializeComponent();        
    }
   
    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //Notice this event-handler is ASYNC
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            SuggestedStartLocation = await topLevel.StorageProvider.TryGetWellKnownFolderAsync(WellKnownFolder.Downloads),
            Title = "Open Image File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // Open reading stream from the first file.
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            // Reads all the content of file as a text.
            //var fileContent = await streamReader.ReadToEndAsync();
            MyImage.Source= new Bitmap(streamReader.BaseStream);
        }
    }
}