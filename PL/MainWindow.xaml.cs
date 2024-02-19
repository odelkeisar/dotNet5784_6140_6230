using PL.Chef;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using BO;
using PL.Manager_file;
using System.Collections.ObjectModel;


namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public DateTime clock
    {
        get { return (DateTime)GetValue(clockProparty); }
        set { SetValue(clockProparty, value); }
    }

    public static readonly DependencyProperty clockProparty =
        DependencyProperty.Register("clock", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

    

    public MainWindow()
    {
        // יש להגדיר את המשתנה clock לפני הקריאה ל-InitializeComponent
        clock = s_bl.Task1.ReadClockProject();

        // כאן המשתנה clock כבר הוא חלק מהמערכת הקשר
        InitializeComponent();
    }

    /// <summary>
    /// Button implementation: treatment of chefs, displaying the list of chefs.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChefList_Click(object sender, RoutedEventArgs e)
    {
        new ChefListWindow().ShowDialog();
    }
    /// <summary>
    /// Data initialization button implementation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataInitialization_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך לאתחל את הנתונים?", "אישור איתחול", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
            DalTest.Initialization.Do();

        //Factory.Get().InitializeDB();

    }
    /// <summary>
    /// Implementing a data clear button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את הנתונים?", "אישור ניקוי נתונים", MessageBoxButton.YesNo);
    

        if (result == MessageBoxResult.Yes)
            DalTest.Initialization.Reset();
    }

    private void Manager_Click(object sender, RoutedEventArgs e)
    {
        new ManagerWinow().ShowDialog();
    }

    private void Chef_Click(object sender, RoutedEventArgs e)
    {
        BO.Chef ?chef = new BO.Chef();
        string idNumber = Microsoft.VisualBasic.Interaction.InputBox("אנא הזן מספר זהות:", "הזנת מספר זהות", "");

        // בדיקה אם המספר זהות שהוזן אינו ריק
        if (!string.IsNullOrWhiteSpace(idNumber))
        {
            try
            {
                chef = s_bl.Chef.Read(int.Parse(idNumber));
                new ActChefWindow(int.Parse(idNumber)).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }
        else
        {
            MessageBox.Show("לא הוזן מספר זהות.");
        }
    }

    private void plusTime_Click(object sender, RoutedEventArgs e)
    {
        TimeSpan time = new (0, 1, 0, 0);
        s_bl.Task1.UpdateClockProject(time);
        clock = s_bl.Task1.ReadClockProject();
    }
}