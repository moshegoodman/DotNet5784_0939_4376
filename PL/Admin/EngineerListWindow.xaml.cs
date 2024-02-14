using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Admin;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>


public partial class EngineerListWindow : Window
{

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));


    public BO.EngineerExperience ExperirnceFilter { get; set; } = BO.EngineerExperience.None;
    //window ctor
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow().ShowDialog();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }




    //for the data binding a filtered list
    private void ExperienceFlt(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (ExperirnceFilter == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == ExperirnceFilter)!;
    }

    //opens update window with the engineers details
    private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
        if (engineer != null)
        {
            new EngineerWindow(engineer.Id).ShowDialog();
            EngineerList = s_bl.Engineer.ReadAll()!;
        }
    }
}
