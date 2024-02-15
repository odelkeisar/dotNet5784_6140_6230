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

namespace PL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChefList_Click(object sender, RoutedEventArgs e)
        {
            new ChefListWindow().Show();
        }

        private void DataInitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח שברצונך לאתחל את הנתונים?", "אישור איתחול", MessageBoxButton.YesNo);

        // בדיקת התשובה של המשתמש
            if (result == MessageBoxResult.Yes)
        {
            // אם המשתמש אישר, קריאה למתודת האתחול
                DalTest.Initialization.Do();

            //Factory.Get().InitializeDB();


        }
    }
}