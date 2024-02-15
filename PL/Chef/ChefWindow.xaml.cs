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
using System.Xml;

namespace PL.Chef
{
    /// <summary>
    /// Interaction logic for ChefWindow.xaml
    /// </summary>
    public partial class ChefWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ChefWindow()
        {
            int num = 0;
            InitializeComponent();

            if (num == 0)
            {
                Chef = new BO.Chef();
            }

            else
            {

                try
                {
                    Chef = s_bl.Chef.Read(num);

                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        public BO.Chef Chef
        {
            get { return (BO.Chef)GetValue(ChefProparty); }
            set { SetValue(ChefProparty, value); }
        }

        public static readonly DependencyProperty ChefProparty =
       DependencyProperty.Register("Chef", typeof(BO.Chef), typeof(ChefWindow), new PropertyMetadata(null));
    }

   

}
