using BO;
using DO;
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
    ////public BO.ChefExperience level { get; set; } = BO.ChefExperience.None; /*BO.ChefExperience.None;*/


    public BO.ChefExperience level
    {
        get { return (BO.ChefExperience)GetValue(levelProperty); }
        set { SetValue(levelProperty, value); }
    }

    // Using a DependencyProperty as the backing store for level.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty levelProperty =
        DependencyProperty.Register("level", typeof(BO.ChefExperience), typeof(ChefListWindow), new PropertyMetadata(BO.ChefExperience.None));

    /// <summary>
    /// constructor
    /// </summary>
    public ChefListWindow()
    {
        ChefList = new ObservableCollection<BO.Chef>(s_bl?.Chef.ReadAll() ?? Enumerable.Empty<BO.Chef>());
        InitializeComponent();
    }

    /// <summary>
    /// The list of chefs, of the dependent type.
    /// </summary>
    public ObservableCollection<BO.Chef> ChefList
    {
        get { return (ObservableCollection<BO.Chef>)GetValue(ChefListProparty); }
        set { SetValue(ChefListProparty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ChefListProparty =
        DependencyProperty.Register("ChefList", typeof(ObservableCollection<BO.Chef>), typeof(ChefListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Filter the list view by level.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeSelect(object sender, SelectionChangedEventArgs e)
    {
        ChefList = (level == BO.ChefExperience.None) ?
            new ObservableCollection<BO.Chef>(s_bl?.Chef.ReadAll()!) : new ObservableCollection<BO.Chef>(s_bl?.Chef.ReadAllPerLevel(level)!);
    }

    /// <summary>
    /// Opening a window to create a new chef and refresh the list afterwards.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonAdd_Click(object sender, RoutedEventArgs e)
    {
        ChefWindow chefWindow = new ChefWindow();
        chefWindow.Closed += (s, args) =>
        {
            ChefList = (level == BO.ChefExperience.None) ? new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAll()!) : new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllPerLevel(level)!);
        };
        chefWindow.ShowDialog();
    }
    /// <summary>
    /// Opening a window to display a single chef and update his details, and refresh the list afterwards
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListView_UpdateChef_Click(object sender, MouseButtonEventArgs e)
    {
        BO.Chef? chef = (sender as ListView)?.SelectedItem as BO.Chef;
        if (chef != null)
        {
            ChefWindow chefWindow = new ChefWindow(chef.Id);
            chefWindow.Closed += (s, args) =>
            {
                ChefList =(level== BO.ChefExperience.None)? new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAll()!):new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllPerLevel(level)!);
            };
            chefWindow.ShowDialog();
        }
    }


}
