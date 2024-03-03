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
    /// Interaction logic for GantWindow.xaml
    /// </summary>
    public partial class GantWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProparty); }
            set { SetValue(TaskListProparty, value); }
        }
        public static readonly DependencyProperty TaskListProparty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        public GantWindow()
        {
            TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAll());

            InitializeComponent();
        }
    }
}
