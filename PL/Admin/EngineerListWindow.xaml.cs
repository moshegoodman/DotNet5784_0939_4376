using System.Windows;
using System.Windows.Controls;

namespace PL.Admin;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }

    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));


    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}
