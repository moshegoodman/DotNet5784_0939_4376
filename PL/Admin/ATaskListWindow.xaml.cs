﻿using System.Collections.Generic;
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


    public BO.EngineerExperience ExperirnceFilter { get; set; } = BO.EngineerExperience.None;

    //for the data binding a filtered list
    private void ExperienceFlt(object sender, SelectionChangedEventArgs e)
    {
        //    TaskList = (ExperirnceFilter == BO.EngineerExperience.None) ?
        //        s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == ExperirnceFilter)!;
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
    private void ListView_MouseDoubleClick(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? task_in_list = (sender as ListView)?.SelectedItem as BO.TaskInList;
        if (task_in_list != null)
        {
            new ATaskWindow(task_in_list.Id).ShowDialog();
            TaskList = s_bl.Task.ReadAll()!;
        }
    }
}