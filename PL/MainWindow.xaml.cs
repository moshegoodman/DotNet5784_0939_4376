using PL.Admin;
using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    public MainWindow()
    {
        int id = 0;
        InitializeComponent();
    }

    private void btnAdmins_Click(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().Show();
    }

    private void btnInit_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult toInit = MessageBox.Show("Are you sure you want to initialize all data?", "Initialization", MessageBoxButton.YesNo);
        if (toInit == MessageBoxResult.Yes)
        {
            DalTest.Initialization.Do();
        }
    }
}
