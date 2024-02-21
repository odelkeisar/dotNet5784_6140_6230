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

namespace PL.Chef
{
    /// <summary>
    /// Interaction logic for SelectTaskOfChefWindow.xaml
    /// </summary>
    public partial class SelectTaskOfChefWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        BO.Chef chef1 = new BO.Chef();
        public SelectTaskOfChefWindow(BO.Chef chef)
        {
            try
            {
                TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAllPossibleTasks(chef));
                chef1 = chef;
            } 
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                this.Close();
            }

            InitializeComponent();
        }

        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProparty); }
            set { SetValue(TaskListProparty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProparty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(SelectTaskOfChefWindow), new PropertyMetadata(null));

        private void PosibbleTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BO.TaskInList? selectedTask = ((ListView)sender).SelectedItem as BO.TaskInList;
                
                if (selectedTask != null)
                {
                    chef1.task = new BO.TaskInChef { Id = selectedTask.Id, Alias = selectedTask.Alias };
                    s_bl.Chef.Update(chef1);
                    MessageBox.Show($"משימה {selectedTask.Id} הוקצתה בהצלחה");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                this.Close();
            }
        }

    }
}
