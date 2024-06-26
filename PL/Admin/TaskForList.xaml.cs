﻿using System.Collections.Generic;
using System.Linq;
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


    public bool[] ExperienceFilter { get; set; } = new bool[] { true, true, true, true, true };
    public BO.General IsComplete { get; set; } = BO.General.None;
    public BO.General IsDesignated { get; set; } = BO.General.None;
    public string SearchFilter
    {
        get { return (string)GetValue(SearchFilterProperty); }
        set { SetValue(SearchFilterProperty, value); }
    }

    public static readonly DependencyProperty SearchFilterProperty =
        DependencyProperty.Register("SearchFilter", typeof(string), typeof(ATaskListWindow), new PropertyMetadata(""));

    //for the data binding a filtered list


    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        new ATaskWindow().ShowDialog();
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
        }
    }

    private void RadioButton_Complete_Checked(object sender, RoutedEventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;
        switch (radioButton.Content.ToString())
        {
            case "Yes":
                IsComplete = BO.General.Yes;


                break;
            case "No":
                IsComplete = BO.General.No;
                break;
            case "None":
                IsComplete = BO.General.None;
                break;
            default:
                break;


        }
        UpdateTaskList();
    }
    private void RadioButton_Experience_Checked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        switch (checkBox.Content)
        {
            case BO.EngineerExperience.Beginner:
                ExperienceFilter[0] = true;

                break;
            case BO.EngineerExperience.AdvancedBeginner:
                ExperienceFilter[1] = true;

                break;
            case BO.EngineerExperience.Intermediate:
                ExperienceFilter[2] = true;

                break;
            case BO.EngineerExperience.Advanced:
                ExperienceFilter[3] = true;
                break;

            case BO.EngineerExperience.Expert:
                ExperienceFilter[4] = true;

                break;
            default:

                break;


        }
        UpdateTaskList();
    }
    //
    private void RadioButton_Designated_Checked(object sender, RoutedEventArgs e)
    {
        RadioButton radioButton = sender as RadioButton;
        switch (radioButton.Content.ToString())
        {
            case "Yes":
                IsDesignated = BO.General.Yes;
                break;
            case "No":
                IsDesignated = BO.General.No;
                break;
            case "None":
                IsDesignated = BO.General.None;


                break;
            default:
                break;


        }
        UpdateTaskList();
    }

    //if the user unchecks a box
    private void CheckBox_Experience_Unchecked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        switch (checkBox.Content)
        {
            case BO.EngineerExperience.Beginner:
                ExperienceFilter[0] = false;

                break;
            case BO.EngineerExperience.AdvancedBeginner:
                ExperienceFilter[1] = false;

                break;
            case BO.EngineerExperience.Intermediate:
                ExperienceFilter[2] = false;

                break;
            case BO.EngineerExperience.Advanced:
                ExperienceFilter[3] = false;
                break;

            case BO.EngineerExperience.Expert:
                ExperienceFilter[4] = false;

                break;
            default:
                break;


        }
        UpdateTaskList();
    }

    //fiters the tasks
    private void UpdateTaskList()
    {
        TaskList = s_bl.Task.ReadAll();
        if (IsDesignated != BO.General.None)
            TaskList = TaskList.Where(task => IsDesignated == BO.General.Yes ? s_bl!.Task.Read(task.Id).Engineer != null : s_bl!.Task.Read(task.Id).Engineer == null).ToList();

        if (IsComplete != BO.General.None)
            TaskList = TaskList.Where(task => IsComplete == BO.General.Yes ? s_bl!.Task.Read(task.Id).CompleteDate != null : s_bl!.Task.Read(task.Id).CompleteDate == null).ToList();
        for (int i = 0; i < 5; ++i)
        {
            if (ExperienceFilter[i] == false)
                TaskList = TaskList.Where(task => (int)(s_bl!.Task.Read(task.Id).Complexity) != i).ToList();
        }


        TaskList = TaskList.Where(task => s_bl!.Task.Read(task.Id)!.Alias.ToLower().Contains(SearchFilter.ToLower())).ToList();

    }

    private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        SearchFilter = ((TextBox)sender).Text;
        UpdateTaskList();

    }


}
