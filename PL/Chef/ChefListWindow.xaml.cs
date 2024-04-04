
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using BO;
namespace PL.Chef;


/// <summary>
/// Interaction logic for ChefListWindow.xaml
/// </summary>
public partial class ChefListWindow : Window
{
    BO.Chef? chef_ = null;

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.ChefExperience level
    {
        get { return (BO.ChefExperience)GetValue(levelProperty); }
        set { SetValue(levelProperty, value); }
    }

    // Using a DependencyProperty as the backing store for level.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty levelProperty =
        DependencyProperty.Register("level", typeof(BO.ChefExperience), typeof(ChefListWindow), new PropertyMetadata(BO.ChefExperience.ללא_סינון));

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
    /// constructor
    /// </summary>
    public ChefListWindow()
    {
        ChefList = new ObservableCollection<BO.Chef>(s_bl?.Chef.ReadAll() ?? Enumerable.Empty<BO.Chef>());
        InitializeComponent();
    }

    /// <summary>
    /// Filter the list view by level.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeSelect(object sender, SelectionChangedEventArgs e)
    {
        ChefList = (level == BO.ChefExperience.ללא_סינון) ?
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
            ChefList = (level == BO.ChefExperience.ללא_סינון) ? new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAll()!) : new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllPerLevel(level)!);
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
                ChefList = (level == BO.ChefExperience.ללא_סינון) ? new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAll()!) : new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllPerLevel(level)!);
            };

            chefWindow.ShowDialog();
        }
    }
  
    private void ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (chef_ != null)
            {
                MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק שף?", "אישור מחיקת נתוני שף", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    s_bl.Chef.Delete(chef_.Id);
                    ChefList = (level == BO.ChefExperience.ללא_סינון) ? new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAll()!) : new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllPerLevel(level)!);
                    chef_ = null;
                }
            }
            else
            {
                MessageBox.Show($"יש לבחור שף למחיקה");
            }
        }
        catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}"); }
    }

    private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        chef_ = (sender as ListView)?.SelectedItem as BO.Chef;

    }

    private void ButtonOld_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            OldChefWindow oldchefWindow = new OldChefWindow();
            oldchefWindow.Closed += (s, args) =>
            {
                ChefList = (level == BO.ChefExperience.ללא_סינון) ? new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAll()!) : new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllPerLevel(level)!);
            };
            oldchefWindow.ShowDialog();
        }
        catch(Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
    }
}
