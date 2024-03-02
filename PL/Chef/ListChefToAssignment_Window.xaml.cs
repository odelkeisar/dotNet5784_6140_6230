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

namespace PL.Chef
{
    /// <summary>
    /// Interaction logic for ListChefToAssignment_Window.xaml
    /// </summary>
    public partial class ListChefToAssignment_Window : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ObservableCollection<BO.Chef> ChefList
        {
            get { return (ObservableCollection<BO.Chef>)GetValue(ChefListProparty); }
            set { SetValue(ChefListProparty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChefListProparty =
            DependencyProperty.Register("ChefList", typeof(ObservableCollection<BO.Chef>), typeof(ChefListWindow), new PropertyMetadata(null));

        int id = 0;
        public BO.Chef? selectedchef = null;
        public ListChefToAssignment_Window(int id_)
        {
            id = id_;
            ChefList = new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllNotAssigned()!);
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedchef = (sender as ListView)?.SelectedItem as BO.Chef;
            this.Close();
        }
    }
}
