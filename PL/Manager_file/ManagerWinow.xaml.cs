using PL.Chef;
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
        public ManagerWinow()
        {
            InitializeComponent();
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
                DalTest.Initialization.Reset();
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
                DalTest.Initialization.Do();

            //Factory.Get().InitializeDB();

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
    }
}
