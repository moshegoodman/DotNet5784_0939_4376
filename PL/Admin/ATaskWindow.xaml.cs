using BlApi;
using DO;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PL.Admin;

/// <summary>
/// Interaction logic for ATaskWindow.xaml
/// </summary>
public partial class ATaskWindow : Window
{



    static readonly IBl s_bl = Factory.Get();

    public static readonly DependencyProperty TaskProperty =
    DependencyProperty.Register("Task", typeof(BO.Task), typeof(ATaskWindow), new PropertyMetadata(null));

    public BO.Task Task
    {
        get
        {
            return (BO.Task)GetValue(TaskProperty);
        }
        set { SetValue(TaskProperty, value); }
    }

    public ATaskWindow(int id = 0)
    {
        InitializeComponent();

        if (id == 0)
        {
            Task = new BO.Task()
            {
                Id = 0,
                Alias = "",
                Description = "",
                CreatedAtDate = DateTime.Now,
                Status = BO.Status.Unscheduled,
                Dependencies = new List<BO.TaskInList>(),
                Complexity = BO.EngineerExperience.Beginner,
                Deliverables = "",
                Remarks = "",
                RequiredEffortTime = null,
                StartDate = null,
                ScheduledDate = null,
                ForecastDate = null,
                DeadlineDate = null,
                CompleteDate = null,
                Engineer = null
            };
        }
        else
        {
            try
            {
                Task = s_bl.Task.Read(id)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }




    //opens the add/update window
    private void Btn_Add_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Task.Create(Task);
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private void Btn_Update_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int? engineerId = Task.Engineer != null ? Task.Engineer.Id : null;
            s_bl.Task.Update(Task);
            if (engineerId != null)
            {
                s_bl.Task.DesignateEngineer(Task.Id, (int)engineerId);
            }
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Button_See_Dependency_Click(object sender, RoutedEventArgs e)
    {


        new DependencyListWindow(Task).ShowDialog();
    }
    private void Button_Add_Dependency_Click(object sender, RoutedEventArgs e)
    {

        new DependencyListWindow(Task).ShowDialog();
    }

    private void Btn_Engineer_designation(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().ShowDialog();
    }
}
