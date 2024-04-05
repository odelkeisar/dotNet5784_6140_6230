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
/// לוגיקת אינטקרציה עבור חלון לטיפול בארכיון שפים
/// </summary>
public partial class OldChefWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
/// <summary>
/// רשימת השפים שנמצאים בארכיון
/// </summary>
    public ObservableCollection<BO.Chef> ChefList
    {
        get { return (ObservableCollection<BO.Chef>)GetValue(ChefListProparty); }
        set { SetValue(ChefListProparty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ChefListProparty =
        DependencyProperty.Register("ChefList", typeof(ObservableCollection<BO.Chef>), typeof(OldChefWindow), new PropertyMetadata(null));

    /// <summary>
    /// השף שנבחר ברשימה בלחיצה על מקש ימני 
    /// </summary>
    public BO.Chef? selectedchef = null;
    /// <summary>
    /// בנאי לאיתחול הרשימה ואוביקט הסימון
    /// </summary>
    public OldChefWindow()
    {
        selectedchef = null;
        ChefList = new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllDeleted());
        InitializeComponent();
    }

    /// <summary>
    /// פעולה עבור לחיצה על מקש שיחזור
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonRecovery_Click(object sender, RoutedEventArgs e)
    {
        if (selectedchef != null)
        {
            try
            {
                s_bl.Chef.RecoveryChef(selectedchef);
                MessageBox.Show($"השיחזור בוצע בהצלחה", "הודעה", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                ChefList = new ObservableCollection<BO.Chef>(s_bl.Chef.ReadAllDeleted()); //רענון רשימה
                selectedchef = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign);
                this.Close();
            }
        }
        else { MessageBox.Show("יש לבחור שף לשיחזור", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RightAlign); }
    }

    /// <summary>
    /// השמת השף שנבחר ברשימה בתוך המשתנה selectedchef.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Marker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        selectedchef = (sender as ListView)?.SelectedItem as BO.Chef;
    }
}