using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;

namespace AvaloniaApp_VS.Views;
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
public partial class Demo5View : Window
{
    
    public Demo5View()
    {
        InitializeComponent();
        /*animals.ItemsSource = new string[]
                {"cat", "camel", "cow", "chameleon", "mouse", "lion", "zebra" }
            .OrderBy(x => x);*/
        animals.ItemsSource = new Item[]
                {new Item("cat",""), new Item("camel",""), new Item("cow",""), new Item("chameleon","") };
    }
}