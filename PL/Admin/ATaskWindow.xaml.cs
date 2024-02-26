using BlApi;
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
                Milestone = null,
                Complexity = BO.EngineerExperience.None,
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
                foreach (BO.TaskInList something in Task.Dependencies) { Console.WriteLine(something); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
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
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
    }
    private void Btn_Update_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Task.Update(Task);
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        List<BO.TaskInList> listDependents = Task.Dependencies;
        IEnumerable<BO.TaskInList> dependents = new List<BO.TaskInList>();
        dependents = listDependents;
        // foreach (TaskInList dependent in listDependents) { dependents.Append(dependent)}

        new ATaskListWindow(dependents).ShowDialog();
    }
}
