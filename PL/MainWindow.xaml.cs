using PL.Chef;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// conctractor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button implementation: treatment of chefs, displaying the list of chefs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChefList_Click(object sender, RoutedEventArgs e)
        {
            new ChefListWindow().ShowDialog();
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
        /// Implementing a data clear button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך למחוק את הנתונים?", "אישור ניקוי נתונים", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                DalTest.Initialization.Reset();
        }
    }
}