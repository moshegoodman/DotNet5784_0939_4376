using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PL.Admin;

public partial class EngineerWindow : Window
{
    static readonly IBl s_bl = Factory.Get();

    //engineer object for working with the data

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(EngineerWindow), new PropertyMetadata(null));


    public BO.Engineer Engineer
    {
        get
        {
            return (BO.Engineer)GetValue(EngineerProperty);
        }
        set { SetValue(EngineerProperty, value); }
    }
    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
    //window ctor
    public EngineerWindow(int id = 0)
    {
        InitializeComponent();
        if (id == 0)
        {
            Engineer = new BO.Engineer()
            {
                Id = 0,
                Name = "",
                Email = "",
                Cost = 0,
                Task = null,
                Level = BO.EngineerExperience.None
            };
        }
        else
        {
            try
            {

                Engineer = s_bl.Engineer.Read(id)!;
                //initialize the relevent tasks for the engineer
                TaskList = s_bl.Task.ReadAll(task => task.Complexity <= Engineer.Level && task.Engineer == null && task.Dependencies.All(d => s_bl.Task.Read(d.Id)!.CompleteDate != null));
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
            s_bl.Engineer.Create(Engineer);
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR",MessageBoxButton.OK,MessageBoxImage.Error); }
    }
    private void Btn_Update_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Engineer.Update(Engineer);
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR",MessageBoxButton.OK,MessageBoxImage.Error); }
    }
    private void BtnTask_choose(object sender, RoutedEventArgs e)
    {
        new ATaskListWindow().Show();
    }

}
