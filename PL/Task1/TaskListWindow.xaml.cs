using PL.Chef;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
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

namespace PL.Task1
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        /// <summary>
        /// A property of a task status.
        /// </summary>
        public BO.Status StatusTask_
        {
            get { return (BO.Status)GetValue(StatusTask_Property); }
            set { SetValue(StatusTask_Property, value); }
        }
        public static readonly DependencyProperty StatusTask_Property =
            DependencyProperty.Register("StatusTask_", typeof(BO.Status), typeof(TaskListWindow), new PropertyMetadata(BO.Status.None));
        /// <summary>
        ///  Gets or sets the collection of tasks displayed in the window.
        /// 
        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProparty); }
            set { SetValue(TaskListProparty, value); }
        }
        public static readonly DependencyProperty TaskListProparty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// constractor
        /// </summary>
        public TaskListWindow()
        {
            TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAll());
            InitializeComponent();
        }
        /// <summary>
        /// Handles the event when the selection changes in a control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeSelect(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TaskList = StatusTask_ == BO.Status.None ? new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAll()!) : new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAllPerStatus(StatusTask_)!);
            }
            catch (Exception ex)
            {
                TaskList = new ObservableCollection<BO.TaskInList>();
            }
        }

        /// <summary>
        /// Filter by unassigned tasks 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Unassigned_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAllNoChefWasAssigned().Where(task => task.status == StatusTask_));
            }
            catch (Exception ex)
            {
                TaskList = new ObservableCollection<BO.TaskInList>();
            }
        }
        /// <summary>
        /// Canceling the filtering of the unassigned tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Unassigned_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskList = StatusTask_ == BO.Status.None ? new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAll()!) : new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAllPerStatus(StatusTask_)!);
            }
            catch (Exception ex)
            {
                TaskList = new ObservableCollection<BO.TaskInList>();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (task_ != null)
                {
                    MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק משימה?", "אישור מחיקת נתוני משימה", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        s_bl.Task1.Delete(task_.Id);
                        TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAll()!);
                       task_ = null;
                    }
                }
                else
                {
                    MessageBox.Show($"יש לבחור משימה למחיקה");
                }
            }
            catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}"); }
        }

        BO.TaskInList? task_ = null;

        private void DeleteMarker(object sender, MouseButtonEventArgs e)
        {
            task_ = (sender as ListView)?.SelectedItem as BO.TaskInList;
        }

        private void ButtonAddTaskWindow_Click(object sender, RoutedEventArgs e)
        {
           int id = 0;
           new TaskWindow(id).ShowDialog();
        }
    }
}
