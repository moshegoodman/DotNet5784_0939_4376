using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.Admin;

/// <summary>
/// Interaction logic for DpendencyListWindow.xaml
/// </summary>
/// 

public partial class DependencyListWindow : Window
{

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.Task Task
    {
        get
        {
            return (BO.Task)GetValue(TaskProperty);
        }
        set { SetValue(TaskProperty, value); }
    }

    public static readonly DependencyProperty TaskProperty =
    DependencyProperty.Register("Task", typeof(BO.Task), typeof(DependencyListWindow), new PropertyMetadata(null));



    bool[] isEnabled { get; set; }
    public IEnumerable<BO.TaskInList> DependentList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }
    public static readonly DependencyProperty TaskListProperty =
   DependencyProperty.Register("DependentList", typeof(IEnumerable<BO.TaskInList>), typeof(DependencyListWindow), new PropertyMetadata(null));


    public BO.EngineerExperience ExperienceFilter { get; set; } = BO.EngineerExperience.None;

    //for the data binding a filtered list
    private void ExperienceFlt(object sender, SelectionChangedEventArgs e)
    {
        if (ExperienceFilter == BO.EngineerExperience.None)
        {
            DependentList = s_bl?.Task.ReadAll()!;
            return;
        }

        DependentList = (ExperienceFilter == BO.EngineerExperience.None) ?
         s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Complexity == ExperienceFilter)!;
    }
    public DependencyListWindow()
    {
        InitializeComponent();
        DependentList = s_bl?.Task.ReadAll()!;

    }
    public DependencyListWindow(BO.Task _task)
    {
        InitializeComponent();
        if (s_bl.Task.GetProjectStatus() != 1)
        {
            DependentList = _task.Dependencies;
        }
        else
        {
            DependentList = s_bl?.Task.ReadAll()!;

        }
        Task = _task;
        List<BO.TaskInList> taskinlist = s_bl!.Task.ReadAll()!.ToList<BO.TaskInList>();
        isEnabled = new bool[taskinlist.Count];


        for (int i = 0; i < taskinlist.Count; ++i)
        {
            isEnabled[i] = taskinlist.Any(x => x.Id == taskinlist[i].Id);


        }

    }

    private void Btn_Set_As_Dependency(object sender, RoutedEventArgs e)
    {

        try
        {
            BO.TaskInList? dependency_in_list = (sender as Button)?.DataContext as BO.TaskInList;


            s_bl.Task.AddDependency(Task.Id, dependency_in_list.Id);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }


    }




}

