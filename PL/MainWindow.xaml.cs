using BlApi;
using PL.Admin;
using PL.Engineer;
using System;
using System.ComponentModel;
using System.DirectoryServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
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


    //------------------CLOCK--------------------------//


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
    private void StartSimulationClick(object sender, RoutedEventArgs e)
    {
        Thread thread = new Thread(UpdateClock);
        thread.Start();
    }

    private void UpdateClock()
    {
        while (true)
        {
            s_bl.AddOneHour();

            // Update Clock property on UI thread using Dispatcher
            Dispatcher.Invoke(() =>
            {
                Clock = s_bl.Clock;
            });

            Thread.Sleep(1000); // Sleep for 1 second
        }
    }



    //-----------------ADMIN-------------------------------------//




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
                new StartDateInputDialogWindow().ShowDialog();
                new TaskListForSchedule().Show();

            }
        }
        else
        {
            new TaskListForSchedule().Show();
        }
    }

    private void btnAutoSchedule_Click(object sender, RoutedEventArgs e)
    {
        try
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
                    new StartDateInputDialogWindow().ShowDialog();
                }
            }
            s_bl.Task.AutoSchedule(s_bl.Clock);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); }
    }


    //--------------------------------ENGINEER---------------------------------------------------//



    BO.Engineer CurrentEngineer;

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        if (UserId != null)
        {
            try
            {
                CurrentEngineer = s_bl.Engineer.Read(int.Parse(UserId))!;
                new EEngineerWindow(int.Parse(UserId)).ShowDialog();


            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        else { MessageBox.Show("You didn't enter an Id", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        UserId = null;
    }

    private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            BtnOk_Click(sender, e);
        }
    }


    private string? _userId;

    public event PropertyChangedEventHandler PropertyChanged;

    public string? UserId
    {
        get { return _userId; }
        set
        {
            if (_userId != value)
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


