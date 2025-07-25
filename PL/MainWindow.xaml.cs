﻿using PL.Chef;
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
using System.Threading.Tasks;


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

    public DateTime? startDate
    {
        get { return (DateTime)GetValue(startDateProparty); }
        set { SetValue(startDateProparty, value); }
    }
    public static readonly DependencyProperty startDateProparty =
        DependencyProperty.Register("startDate", typeof(DateTime?), typeof(MainWindow), new PropertyMetadata(null));
    public DateTime? endDate
    {
        get { return (DateTime)GetValue(endDateProparty); }
        set { SetValue(endDateProparty, value); }
    }
    public static readonly DependencyProperty endDateProparty =
        DependencyProperty.Register("endDate", typeof(DateTime?), typeof(MainWindow), new PropertyMetadata(null));

    /// <summary>
    /// בנאי
    /// </summary>
    public MainWindow()
    {
        // יש להגדיר את המשתנה clock לפני הקריאה ל-InitializeComponent
        clock = s_bl.Task1.ReadClockProject();
        startDate = s_bl.Task1.ReadStartProject();
        endDate = s_bl.Task1.ReadEndProject();

        // כאן המשתנה clock כבר הוא חלק מהמערכת הקשר
        InitializeComponent();
    }
   /// <summary>
   /// כניסה למסך המנהל
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    private void Manager_Click(object sender, RoutedEventArgs e)
    {
        ManagerWinow managerWinow = new ManagerWinow();
        managerWinow.Closed += (s, args) => //הוא מקור הארוע כלומר החלון שנסגר, ו-ארג'ס זה מידע נוסף על האירוע S 
        {
            startDate = s_bl.Task1.ReadStartProject();
            endDate = s_bl.Task1.ReadEndProject();
            clock = s_bl.Task1.ReadClockProject();
        };
        managerWinow.ShowDialog();
    }

    /// <summary>
    /// כניסה למסך השף
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Chef_Click(object sender, RoutedEventArgs e)
    {
        BO.Chef chef = new BO.Chef();
        string idNumber = Microsoft.VisualBasic.Interaction.InputBox("אנא הזן מספר זהות:", "הזנת מספר זהות", "" );

        // בדיקה אם המספר זהות שהוזן אינו ריק
        if (!string.IsNullOrWhiteSpace(idNumber))
        {
            try
            {
                chef = s_bl.Chef.Read(int.Parse(idNumber))!;

                if (chef.task == null) //אם אין לשף משימה אז יפתח חלון לבחירת משימה חדשה
                {
                    SelectTaskOfChefWindow selectTaskOfChefWindownew = new SelectTaskOfChefWindow(chef);

                    selectTaskOfChefWindownew.Closed += (s, args) =>
                    {
                        BO.TaskInList? selectedTask = selectTaskOfChefWindownew.taskSelected;
                        if (selectedTask != null)
                        {
                            try
                            {
                                chef.task = new BO.TaskInChef { Id = selectedTask.Id, Alias = selectedTask.Alias };
                                s_bl.Chef.Update(chef);
                                MessageBox.Show($"משימה {selectedTask.Id} הוקצתה בהצלחה", "הודעה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"{ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                            }
                        }
                    };
                    selectTaskOfChefWindownew.ShowDialog();
                }

                else
                    new ActChefWindow(int.Parse(idNumber)).ShowDialog();
            }

            catch (Exception ex)
            {
                MessageBox.Show($" {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }

        }
        else
        {
            MessageBox.Show("לא הוזן מספר זהות.", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
        }
    }
   /// <summary>
   /// קידום הזמן בשעה
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    private void plusHour_Click(object sender, RoutedEventArgs e)
    {
        TimeSpan time = new(0, 1, 0, 0);
        s_bl.Task1.UpdateClockProject(time);
        clock = s_bl.Task1.ReadClockProject();
    }

    /// <summary>
    /// קידום השעה בדקה
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void plusMinute_Click(object sender, RoutedEventArgs e)
    {
        TimeSpan time = new(0, 0, 1, 0);
        s_bl.Task1.UpdateClockProject(time);
        clock = s_bl.Task1.ReadClockProject();
    }
}

