using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using BO;

namespace PL.Task1;

public partial class GantWindow : Window
{
   
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public ObservableCollection<BO.TaskInList> TaskList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProparty); }
        set { SetValue(TaskListProparty, value); }
    }
    public static readonly DependencyProperty TaskListProparty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(GantWindow), new PropertyMetadata(null));

    public List<DateTime>? DateTimes { get; set; }
    DataTable dataTable = new DataTable();

    public GantWindow()
    {
        DateTimes = null;
        InitializeComponent();
        InitializeDataGrid(); // הוסף פעולת טעינה כאשר החלון נטען
    }
    /// <summary>
    /// 
    /// </summary>
    private void InitializeDataGrid()
    {
        // קריאת המשימות מהמקור הנתון (במקרה שלך, זה נראה כי המימוש שלך עושה זאת כבר)
        TaskList = new ObservableCollection<BO.TaskInList>(s_bl.Task1.ReadAll());

        // קביעת טווח התאריכים לתצוגת התרשים גאנט
        StartDate = s_bl.Task1.ReadStartProject();
        EndDate = s_bl.Task1.ReadEndProject();

        // אתחול DateTimes
        DateTimes = new List<DateTime>();

        // הוספת כל התאריכים ל-DateTimes
        for (DateTime day = (DateTime)StartDate!; day <= EndDate; day = day.AddMinutes(30))
        {
            DateTimes.Add(day);
        }

        // יצירת טבלת נתונים עם עמודת שם המשימה
        DataTable dataTable = new DataTable();

        dataGridSched.Columns.Add(new DataGridTextColumn() { Header = "Task Id", Binding = new Binding("[0]") });
        dataTable.Columns.Add("Task Id", typeof(int));

        dataGridSched.Columns.Add(new DataGridTextColumn() { Header = "Task Name", Binding = new Binding("[1]") });
        dataTable.Columns.Add("Task Name", typeof(string));


        // ציור כל התאריכים בכותרות התאים של התרשים גאנט
        int col = 2;
        for (DateTime day = (DateTime)StartDate!; day < EndDate; day = day.AddMinutes(30))
        {
            string strDay = day.ToString(); //"21/2/2024"
            DataGridTextColumn dateColumn = new DataGridTextColumn
            {
                Header = strDay, // כאן אנו מבצעים בדיקה אם אנו בעמודה הראשונה
                Binding = new Binding($"[{col}]")
            };
            dataGridSched.Columns.Add(dateColumn);
            dataTable.Columns.Add(strDay, typeof(string));// typeof(System.Windows.Media.Color));
            col++;
        }


        // יצירת שורות עבור כל משימה בתרשים גאנט
        foreach (var task in TaskList)
        {
            DataRow newRow = dataTable.NewRow();
            newRow[0] = task.Id;
            newRow[1] = task.Alias;


            var task_ = s_bl.Task1.Read(task.Id);

            for (DateTime day = (DateTime)StartDate!; day < EndDate; day = day.AddMinutes(30))
            {
                string strDay = day.ToString(); //"21/2/2024"

                if (day < task_!.ScheduledDate && day.AddMinutes(30) <= task_.ScheduledDate || day >= task_.ForecastDate)
                {
                    newRow[strDay] ="";
                   
                }//"EMPTY";
                else
                    newRow[strDay] = task.status.ToString(); //BO.TaskStatus.TaskIsSchedualed; //"FULL";

            }

            dataTable.Rows.Add(newRow);
        }


        // הצגת הנתונים בתרשים גאנט
        dataGridSched.ItemsSource = dataTable.DefaultView;
    } 

}

