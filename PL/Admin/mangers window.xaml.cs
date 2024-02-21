using System.Windows;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class managerWindow : Window
    {
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
    }
}
