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

namespace PL.Chef;

/// <summary>
/// Interaction logic for ActChefWindow.xaml
/// </summary>
public partial class ActChefWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Task1 task
    {
        get { return (BO.Task1)GetValue(taskProparty); }
        set { SetValue(taskProparty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty taskProparty =
        DependencyProperty.Register("task", typeof(BO.Task1), typeof(ActChefWindow), new PropertyMetadata(null));

    BO.Chef chef;
    public ActChefWindow(int idNumber)
    {
        chef = s_bl.Chef.Read(idNumber)!;

        if (chef.task != null)
        {
            task = s_bl.Task1.Read((int)chef.task.Id);
        }

        else
        {
            task = new BO.Task1();
        }

        InitializeComponent();
    }



}
