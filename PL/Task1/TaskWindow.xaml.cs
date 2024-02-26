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
            task = new BO.Task1();
            
            try
            {
                if(id!=0)
                {
                    task = s_bl.Task1.Read(id)!;
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            InitializeComponent();
        }
    }
}
