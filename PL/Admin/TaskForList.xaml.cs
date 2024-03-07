using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace PL.Admin;


public partial class ATaskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }
    public static readonly DependencyProperty TaskListProperty =
   DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(ATaskListWindow), new PropertyMetadata(null));


    public BO.EngineerExperience ExperienceFilter { get; set; } = BO.EngineerExperience.None;

    //for the data binding a filtered list
    private void ExperienceFlt(object sender, SelectionChangedEventArgs e)
    {
        if (ExperienceFilter == BO.EngineerExperience.None)
        {
            TaskList = s_bl?.Task.ReadAll()!;
            return;
        }

        TaskList = (ExperienceFilter == BO.EngineerExperience.None) ?
         s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Complexity == ExperienceFilter)!;
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        new ATaskWindow().ShowDialog();
        TaskList = s_bl?.Task.ReadAll()!;
    }

    public ATaskListWindow()
    {
        InitializeComponent();
        TaskList = s_bl?.Task.ReadAll()!;

    }
    public ATaskListWindow(IEnumerable<BO.TaskInList> dependents)
    {
        InitializeComponent();
        TaskList = dependents;



    }
    private void Update_Click(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? task_in_list = (sender as Button)?.DataContext as BO.TaskInList;
        if (task_in_list != null)
        {
            new ATaskWindow(task_in_list.Id).ShowDialog();
            TaskList = s_bl.Task.ReadAll()!;
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? task_in_list = (sender as Button)?.DataContext as BO.TaskInList;
        if (task_in_list != null)
        {
            MessageBoxResult toDelete = MessageBox.Show($"Are you sure you want to delete the current task with id: {task_in_list.Id} for all?", "Delete", MessageBoxButton.YesNoCancel,MessageBoxImage.Question);
            if (toDelete == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Task.Delete(task_in_list.Id);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            TaskList = s_bl.Task.ReadAll()!;

        }

    }
}
