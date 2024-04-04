using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
/// מימוש לוגי של ActChefWindow.xaml
/// </summary>
public partial class ActChefWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Task1 task
    {
        get { return (BO.Task1)GetValue(taskProparty); }
        set { SetValue(taskProparty, value); }
    }

    //ActChefWindow הקוד הזה מגדיר מאפיין תלותי בתכנית, הקשור לתכנית מסוג BO.Task1, בתוך חלון מסוג 
   /// <summary>
   /// ישות מסוג תלות מאפשרת לקשר משתנים בקוד האחרורי למאפיינים בקופץ הזמל
   /// </summary>
    public static readonly DependencyProperty taskProparty =
        DependencyProperty.Register("task", typeof(BO.Task1), typeof(ActChefWindow), new PropertyMetadata(null));

    BO.Chef chef;
    /// <summary>
    /// בנאי של החלון לפי המספר זהות של השף המתקבל
    /// </summary>
    /// <param name="idNumber"></param>
    public ActChefWindow(int idNumber)
    {
        chef = s_bl.Chef.Read(idNumber)!;
        task = s_bl.Task1.Read((int)chef.task!.Id!)!;
     
        InitializeComponent();  //פונקציה שמקשרת בין הקוד של הזמל עם אותו שם
    }

    /// <summary>
    /// כפתור מילוי תאריך התחלה
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StartDate_Checked(object sender, RoutedEventArgs e)
    {
        task.StartDate = s_bl.Task1.ReadClockProject();

        try
        {
            s_bl.Task1.UpdateStartDate(task);
            MessageBox.Show("הפעולה בוצעה בהצלחה!");
            this.Close();
        }

        catch (Exception ex) 
        {
            MessageBox.Show($"Error: {ex.Message}");
            this.Close();
        }
    }
    /// <summary>
    /// כפתור עדכון תאריך סיום
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CompleteDate_Checked(object sender, RoutedEventArgs e)
    {
        task.CompleteDate = s_bl.Task1.ReadClockProject();

        try
        {
            s_bl.Task1.UpdateFinalDate(task);
            MessageBox.Show("הפעולה בוצעה בהצלחה!");
            this.Close();
        }

        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
            this.Close();
        }
    }
}