﻿using System;
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

namespace PL.Chef;

/// <summary>
/// Interaction logic for ChefWindow.xaml
/// </summary>
public partial class ChefWindow : Window
{
    /// <summary>
    /// A dependent type chef.
    /// </summary>
    public BO.Chef? Chef
    {
        get { return (BO.Chef)GetValue(ChefProparty); }
        set { SetValue(ChefProparty, value); }
    }

    public static readonly DependencyProperty ChefProparty =
   DependencyProperty.Register("Chef", typeof(BO.Chef), typeof(ChefWindow), new PropertyMetadata(null));

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    /// <summary>
    /// An empty constructor to create a new chef.
    /// </summary>
    public ChefWindow()
    {
        Chef = new BO.Chef() { Id = 0, deleted = false };
        Chef.task = new BO.TaskInChef();
        InitializeComponent();
    }
    int id_ = 0;
    /// <summary>
    /// A parameter builder that receives an ID and displays details of an existing chef for updating details.
    /// </summary>
    /// <param name="Id"></param>
    public ChefWindow(int Id)
    {
        id_= Id;
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
            MessageBox.Show($"{ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
        }
        InitializeComponent();
    }
   

    /// <summary>
    /// Button implementation: add/update.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        // מחזיר את הכפתור שנלחץ
        Button? clickedButton = sender as Button;

        // בדיקה אם הכפתור הוא הכפתור שהתבצעה עליו הלחיצה
        if (clickedButton != null)
        {
            if (Chef!.task!.Id == null)
                Chef.task = null;
            try
            {
                if ((string)clickedButton.Content == "עדכן")
                {
                    s_bl.Chef.Update(Chef!);
                }
                if ((string)clickedButton.Content == "הוסף")
                {
                    s_bl.Chef.Create(Chef!);
                }
                MessageBox.Show("הפעולה בוצעה בהצלחה!", "הודעה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);

                if ((string)clickedButton.Content == "עדכן")
                {
                    Chef = s_bl.Chef.Read(Chef.Id);//רענון החלון לנתונים הקודמים
                    if (Chef!.task == null)
                    {
                        Chef.task = new BO.TaskInChef();
                    }
                }
            }

        }
    }

    /// <summary>
    /// כפתור הוספת משימה לשף
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonSelectTask_Click(object sender, RoutedEventArgs e)
    {
        if (id_ == 0)  //אם המספר זהות אפס זה אומר שאנחנו בהוספת שף ובשלב זה עדיין לא ניתן להוסיף משימה
            MessageBox.Show($" לא ניתן להקצות משימה בשלב הוספת שף חדש", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
        else
        {
            try
            {
                SelectTaskOfChefWindow selectTaskOfChefWindow = new SelectTaskOfChefWindow(Chef); //יצירת חלון בחירת משימה
                selectTaskOfChefWindow.Closed += (s, args) => //אירוע לאחר סגירת החלון
                {
                    if(selectTaskOfChefWindow.taskSelected!=null)
                    {
                        BO.TaskInList task = selectTaskOfChefWindow.taskSelected;
                        if (task != null)
                        {
                            Chef.task = new BO.TaskInChef() { Id = task.Id, Alias = task.Alias };
                            BO.Chef temp = Chef;
                            Chef = new BO.Chef(); ;
                            Chef = temp;
                            selectTaskOfChefWindow.taskSelected = null;
                        }
                    }
                };
                selectTaskOfChefWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
        }

    }
}