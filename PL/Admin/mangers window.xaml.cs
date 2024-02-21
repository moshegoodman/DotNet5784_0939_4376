using BlApi;
using System.Windows;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class managerWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();

        public managerWindow()
        {
            InitializeComponent();
        }

        private void btn_tasks_Click(object sender, RoutedEventArgs e)
        {
            new ATaskListWindow().Show();
        }
        private void Btn_engineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        private void btnInit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult toInit = MessageBox.Show("Are you sure you want to initialize all data?", "Initialization", MessageBoxButton.YesNo);
            if (toInit == MessageBoxResult.Yes)
            {
                s_bl.InitializeDB();
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult toInit = MessageBox.Show("Are you sure you want to Reset all data?", "Reset", MessageBoxButton.YesNo);
            if (toInit == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }
    }
}
