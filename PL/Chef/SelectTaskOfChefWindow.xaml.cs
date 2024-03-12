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
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); //הגדרת משתנה סטטי שיכול להיכנס לשכבה הלוגית
        public BO.TaskInList? taskSelected = null;
       /// <summary>
       /// בנאי שמאתחל את המשימות האפשריות לשף שהוא קיבל
       /// </summary>
       /// <param name="chef"></param>
        public SelectTaskOfChefWindow(BO.Chef chef)
        {
            taskSelected = null;
            TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAllPossibleTasks(chef));

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

        /// <summary>
        ///listview פונקציה המתבצעת כאשר המשתמש עושה קליק כפול על אחד מהפריטים ב
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            taskSelected = ((ListView)sender).SelectedItem as BO.TaskInList; // השמת הפריט הנבחר במשתנה שמיועד לשמירת המשימה שנבחרה
            this.Close();
        }

    }
}
