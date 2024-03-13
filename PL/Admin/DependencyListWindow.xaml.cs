using System.Collections.Generic;
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
    public DependencyListWindow(IEnumerable<BO.TaskInList> dependents)
    {
        InitializeComponent();
        DependentList = dependents;



    }
}
