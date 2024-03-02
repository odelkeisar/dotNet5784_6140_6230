using BO;
using PL.Chef;
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

namespace PL.Task1;


/// <summary>
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Task1 task
    {
        get { return (BO.Task1)GetValue(TaskProparty); }
        set { SetValue(TaskProparty, value); }
    }
    public static readonly DependencyProperty TaskProparty =
        DependencyProperty.Register("task", typeof(BO.Task1), typeof(TaskWindow), new PropertyMetadata(null));
    public TaskWindow(int id)
    {
        try
        {
            if (id != 0)
            {
                task = s_bl.Task1.Read(id)!;
                if (task.chef == null) { task.chef = new BO.ChefInTask(); }
                if (task.dependeencies == null) { task.dependeencies = new List<BO.TaskInList>(); }

            }
            else
            {
                task = new BO.Task1();
                task.chef = new BO.ChefInTask();
                task.CreatedAtDate = s_bl.Task1.ReadClockProject();
                task.Copmlexity = BO.ChefExperience.Beginner;
                task.dependeencies = new List<BO.TaskInList>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }

        InitializeComponent();
    }

    private void ButtonAssignmentChef_Click(object sender, RoutedEventArgs e)
    {
        if (task.Id == 0)
            MessageBox.Show($"לא ניתן להקצות שף בשלב יצירת המשימה");
        else
        {
            try
            {
                ListChefToAssignment_Window listChefToAssignment_Window = new ListChefToAssignment_Window(task.Id);
                listChefToAssignment_Window.Closed += (s, args) =>
                {
                    if (listChefToAssignment_Window.selectedchef != null)
                    {
                        BO.Chef? _chef = listChefToAssignment_Window.selectedchef;
                        task.chef!.Id = _chef.Id;
                        task.chef.Name = _chef.Name;
                        BO.Task1 task2 = task;
                        task = new BO.Task1();
                        task = task2;
                        listChefToAssignment_Window.selectedchef = null;
                    }
                };
                listChefToAssignment_Window.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}"); }
        }
    }

    BO.TaskInList? taskMarker = new BO.TaskInList();
    private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        taskMarker = (sender as ListView)!.SelectedItem as BO.TaskInList;
    }

    private void ButtonDeleteDependence_Click(object sender, RoutedEventArgs e)
    {
        if (s_bl.Task1.ReadEndProject() != null)
        {
            MessageBox.Show("לא ניתן לעדכן תלויות לאחר שנקבע לוח זמנים לפרויקט");
            return;
        }
        if (taskMarker == null)
            MessageBox.Show($"יש לבחור משימה תלות למחיקה");
        else
        {
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק משימת תלות?", "אישור מחיקת נתוני משימה", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                task.dependeencies = task.dependeencies!.Where(x => x.Id != taskMarker.Id).ToList();
                BO.Task1 task2 = task;
                task = new BO.Task1();
                task = task2;
            }
        }
    }

    private void ButtonAddDependence_Click(object sender, RoutedEventArgs e)
    {
        if (s_bl.Task1.ReadEndProject() != null)
        {
            MessageBox.Show("לא ניתן לעדכן תלויות לאחר שנקבע לוח זמנים לפרויקט");
            return;
        }
        ListTaskForDependenceWindow listTaskForDependenceWindow = new ListTaskForDependenceWindow(task);
        listTaskForDependenceWindow.Closed += (s, args) =>
        {
            BO.TaskInList? task_ = listTaskForDependenceWindow.selectesTask;
            if (task_ != null)
            {
                task.dependeencies!.Add(task_);
                BO.Task1 task2 = task;
                task = new BO.Task1();
                task = task2;
            }
        };
        listTaskForDependenceWindow.ShowDialog();
    }

    private void Button_AddOrUpdate_Click(object sender, RoutedEventArgs e)
    {
        // מחזיר את הכפתור שנלחץ
        Button? clickedButton = sender as Button;

        if (task.chef!.Id == 0)
            task.chef = null;
        try
        {
            if ((string)clickedButton!.Content == "עדכן")
            {
                s_bl.Task1.Update(task);
            }
            if ((string)clickedButton.Content == "הוסף")
            {
                s_bl.Task1.Create(task);
            }
            MessageBox.Show("הפעולה בוצעה בהצלחה!");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
            task = s_bl.Task1.Read(task.Id)!;
            if (task.chef == null) { task.chef = new BO.ChefInTask(); }
            if (task.dependeencies == null) { task.dependeencies = new List<BO.TaskInList>(); }

        }
    }
}

