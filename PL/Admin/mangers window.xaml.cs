using BlApi;
using System.Windows;

namespace PL.Admin;

/// <summary>
/// Interaction logic for Window1.xaml
/// </summary>
public partial class managerWindow : Window
{
    static readonly IBl s_bl = Factory.Get();

    public managerWindow()
    {
        InitializeComponent();
    }

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
        MessageBoxResult toInit = MessageBox.Show("Are you sure you want to initialize all data?", "Initialization", MessageBoxButton.YesNo,MessageBoxImage.Question);
        if (toInit == MessageBoxResult.Yes)
        {
            s_bl.InitializeDB();
        }
    }
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult toInit = MessageBox.Show("Are you sure you want to Reset all data?", "Reset", MessageBoxButton.YesNo,MessageBoxImage.Question);
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
    private void btnStage_Click(object sender, RoutedEventArgs e)
    {
        // new startDateDialogInputWindow().Show();
    }


}
