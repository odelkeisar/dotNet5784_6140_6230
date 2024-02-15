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
            Chef = new BO.Chef() { Id = 0, deleted = false };
            Chef.task = new BO.TaskInChef();
            InitializeComponent();
        }
        public ChefWindow(int Id)
        {
            try
            {
                Chef = s_bl.Chef.Read(Id);
                if(Chef!.task == null) 
                {
                    Chef.task=new BO.TaskInChef();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            InitializeComponent();
        }

        public BO.Chef? Chef
        {
            get { return (BO.Chef)GetValue(ChefProparty); }
            set { SetValue(ChefProparty, value); }
        }

        public static readonly DependencyProperty ChefProparty =
       DependencyProperty.Register("Chef", typeof(BO.Chef), typeof(ChefWindow), new PropertyMetadata(null));


        private void ButtonAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            // מחזיר את הכפתור שנלחץ
            Button? clickedButton = sender as Button;

            // בדיקה אם הכפתור הוא הכפתור שהתבצעה עליו הלחיצה
            if (clickedButton != null)
            {
                if (Chef.task.Id == null)
                    Chef.task = null;
                try
                {
                    if (clickedButton.Content == "Update")
                    {
                        s_bl.Chef.Update(Chef!);
                    }
                    if (clickedButton.Content == "Add")
                    {
                        s_bl.Chef.Create(Chef!);
                    }
                    MessageBox.Show("הפעולה בוצעה בהצלחה!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                    if (Chef!.task == null)
                    {
                        Chef.task = new BO.TaskInChef();
                    }
                }

            }
        }
    }



}
