using BlApi;
using PL.Admin;
using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly IBl s_bl = Factory.Get();

    //opens window
    public MainWindow()
    {
        InitializeComponent();
    }

    //opens window with a list of engineers for the administraator to manage
    private void btnAdmins_Click(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().ShowDialog();
    }

    //initializes data 
    private void btnInit_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult toInit = MessageBox.Show("Are you sure you want to initialize all data?", "Initialization", MessageBoxButton.YesNo);
        if (toInit == MessageBoxResult.Yes)
        {
            s_bl.InitializeDB();
        }
    }
}
