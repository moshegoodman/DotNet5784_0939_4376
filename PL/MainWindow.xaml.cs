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
    //private void btnInit_Click(object sender, RoutedEventArgs e)
    //{
    //    MessageBoxResult toInit = MessageBox.Show("Are you sure you want to initialize all data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
    //    if (toInit == MessageBoxResult.Yes)
    //    {
    //        s_bl.InitializeDB();
    //    }
    //}

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



    //-----------------ADMIN------------------------------------------------------------------------




    private void btn_tasks_Click(object sender, RoutedEventArgs e)
    {
        new ATaskListWindow().Show();
    }
    private void Btn_engineer_Click(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().Show();
    }
    private void btnInit_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult toInit = MessageBox.Show("Are you sure you want to initialize all data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (toInit == MessageBoxResult.Yes)
        {
            s_bl.InitializeDB();
        }
    }
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult toInit = MessageBox.Show("Are you sure you want to Reset all data?", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (toInit == MessageBoxResult.Yes)
        {
            s_bl.ResetDB();
        }
    }

    private void Gantt_Click(object sender, RoutedEventArgs e)
    {
        new GanttChartWindow().Show();
    }

    private void btnSchedule_Click(object sender, RoutedEventArgs e)
    {
        if (s_bl.Task.GetProjectStatus() == 3)
        {
            MessageBox.Show("Cannot Schedule tasks", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (s_bl.Task.GetProjectStatus() == 1)
        {
            MessageBoxResult setStage2 = MessageBox.Show("are you sure that you completes entering all the tasks and that your ready to move on to stage2?", "Schedule", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (setStage2 == MessageBoxResult.Yes)
            {
                new StartDateInputDialogWindow().Show();
            }
        }
        else
        {
            new TaskListForSchedule().Show();
        }
    }
}
