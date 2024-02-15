using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Chef;


/// <summary>
/// Interaction logic for ChefListWindow.xaml
/// </summary>
public partial class ChefListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.ChefExperience level { get; set; } /*BO.ChefExperience.None;*/
    public ChefListWindow()
    {
        InitializeComponent();
        //ChefList = new ObservableCollection<BO.Chef>(s_bl?.Chef.ReadAll() ?? Enumerable.Empty<BO.Chef>());

    }
    public ObservableCollection<BO.Chef> ChefList
    {
        get { return (ObservableCollection< BO.Chef >) GetValue(ChefListProparty); }
        set { SetValue(ChefListProparty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ChefListProparty =
        DependencyProperty.Register("ChefList", typeof(ObservableCollection<BO.Chef>), typeof(ChefListWindow), new PropertyMetadata(null));

    private void ChangeSelect(object sender, SelectionChangedEventArgs e)
    {
        ChefList = (level == BO.ChefExperience.None) ?
            new ObservableCollection<BO.Chef>(s_bl?.Chef.ReadAll()!) : new ObservableCollection<BO.Chef>(s_bl?.Chef.ReadAllPerLevel(level)!);

    }
}
}
