using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Admin;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    public BO.EngineerExperience ExperirnceFilter { get; set; } = BO.EngineerExperience.None;

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow().Show();
    }
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));






    private void ExperienceFlt(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (ExperirnceFilter == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == ExperirnceFilter)!;
    }

}
