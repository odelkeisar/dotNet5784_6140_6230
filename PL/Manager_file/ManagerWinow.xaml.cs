using PL.Chef;
using PL.Task1;
using System;
using System.Collections.Generic;
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

namespace PL.Manager_file
{
    /// <summary>
    /// Interaction logic for ManagerWinow.xaml
    /// </summary>
    public partial class ManagerWinow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DateTime? startDateProject
        {
            get { return (DateTime?)GetValue(startDateProjectProparty); }
            set { SetValue(startDateProjectProparty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty startDateProjectProparty =
            DependencyProperty.Register("startDateProject", typeof(DateTime?), typeof(ManagerWinow), new PropertyMetadata(null));
        public DateTime? endDateProject
        {
            get { return (DateTime?)GetValue(endDateProjectProparty); }
            set { SetValue(endDateProjectProparty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty endDateProjectProparty =
            DependencyProperty.Register("endDateProject", typeof(DateTime?), typeof(ManagerWinow), new PropertyMetadata(null));
        public ManagerWinow()
        {
            InitializeComponent();
            startDateProject = s_bl.Task1.ReadStartProject();
            endDateProject=s_bl.Task1.ReadEndProject();
        }

        /// <summary>
        /// Implementing a data clear button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את הנתונים?", "אישור ניקוי נתונים", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {

                s_bl.InitializeResetB();
            }
            startDateProject=s_bl.Task1.ReadStartProject(); 
            endDateProject= s_bl.Task1.ReadEndProject();
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
                s_bl.InitializeDB();
            startDateProject = s_bl.Task1.ReadStartProject();
            endDateProject = s_bl.Task1.ReadEndProject();

        }

        /// <summary>
        /// Button implementation: treatment of chefs, displaying the list of chefs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    
        private void BottonChef_Click(object sender, RoutedEventArgs e)
        {
            new ChefListWindow().ShowDialog();
        }

        private void ButtonUpdateStartProject_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
               s_bl.Task1.CreateStartProject((DateTime)startDateProject!);
                startDateProject=s_bl.Task1.ReadStartProject();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                startDateProject = s_bl.Task1.ReadStartProject();
            }
        }

        private void ButtonUpdateEndProject_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                s_bl.Task1.CreateEndProject((DateTime)endDateProject!);
                endDateProject = s_bl.Task1.ReadEndProject();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                endDateProject = s_bl.Task1.ReadEndProject();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                s_bl.Task1.CreateEndProject((DateTime)endDateProject!);
                endDateProject = s_bl.Task1.ReadEndProject();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                endDateProject = s_bl.Task1.ReadEndProject();
            }
        }

        //private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{

        //}
    }
}
