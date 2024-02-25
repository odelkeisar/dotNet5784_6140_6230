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
        string[] arr = { "None", "Completed tasks", "Tasks in care", "Unassigned tasks", "Tasks without a scheduled date" };

        public string FilterList
        {
            get { return (string)GetValue(FilterListProparty); }
            set { SetValue(FilterListProparty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterListProparty =
            DependencyProperty.Register("FilterList", typeof(string), typeof(TaskListWindow), new PropertyMetadata(null));


        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProparty); }
            set { SetValue(TaskListProparty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProparty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
        public TaskListWindow()
        {
            TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAll() ?? Enumerable.Empty<BO.TaskInList>());
            InitializeComponent();
        }
        private void ChangeSelect(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FilterList == "None")
                    TaskList = new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAll()!);
                if (FilterList == "Completed tasks")
                    TaskList = new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAllCompleted()!);
                if (FilterList == "Tasks in care")
                    TaskList = new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAllTasksInCare()!);
                if (FilterList == "Unassigned tasks")
                    TaskList = new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAllNoChefWasAssigned()!);
                if (FilterList == "Tasks without a scheduled date")
                    TaskList = new ObservableCollection<BO.TaskInList>(s_bl?.Task1.ReadAllNoScheduledDate()!);
            }
            catch (Exception ex)
            {
                TaskList= new ObservableCollection<BO.TaskInList>();
            }
        }

        public ObservableCollection<string> FilterArray
        {
            get { return new ObservableCollection<string>(arr); }
        }

    }
}
