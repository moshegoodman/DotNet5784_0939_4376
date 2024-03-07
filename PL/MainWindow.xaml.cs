using BlApi;
using PL.Admin;
using PL.Engineer;
using System;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Controls;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly IBl s_bl = Factory.Get();

    public DateTime Clock
    {
        get
        {
            return (DateTime)GetValue(ClockProperty);
        }
        set { SetValue(ClockProperty, value); }
    }
    public static readonly DependencyProperty ClockProperty =
        DependencyProperty.Register("Clock", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

    //opens window
    public MainWindow()
    {
        InitializeComponent();
        Clock = s_bl.Clock;
    }

    //opens window with a list of engineers for the administraator to manage
    private void btnAdmins_Click(object sender, RoutedEventArgs e)
    {
        new managerWindow().Show();
    }

    //initializes data 
    private void btnInit_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult toInit = MessageBox.Show("Are you sure you want to initialize all data?", "Initialization", MessageBoxButton.YesNo,MessageBoxImage.Question);
        if (toInit == MessageBoxResult.Yes)
        {
            s_bl.InitializeDB();
        }
    }

    private void btnEngineer_Click(object sender, RoutedEventArgs e)
    {
        new NameInputDialogWindow().Show();
    }

    private void AddDayClick(object sender, RoutedEventArgs e)
    {
        s_bl.AddOneDay();
        Clock = s_bl.Clock;
    }

    private void AddHourClick(object sender, RoutedEventArgs e)
    {
        s_bl.AddOneHour();
        Clock = s_bl.Clock;
    }
}
