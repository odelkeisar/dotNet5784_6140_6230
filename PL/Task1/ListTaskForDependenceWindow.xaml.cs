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

namespace PL.Task1
{
    /// <summary>
    /// Interaction logic for ListTaskForDependenceWindow.xaml
    /// </summary>
    public partial class ListTaskForDependenceWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.TaskInList? selectesTask;

        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProparty); }
            set { SetValue(TaskListProparty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProparty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(ListTaskForDependenceWindow), new PropertyMetadata(null));

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ListTaskForDependenceWindow(BO.Task1 task_)
        {
            selectesTask = null;
            TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAllNondependenceTask(task_));
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectesTask = (sender as ListView)?.SelectedItem as BO.TaskInList;
            this.Close();
        }
    }
}
