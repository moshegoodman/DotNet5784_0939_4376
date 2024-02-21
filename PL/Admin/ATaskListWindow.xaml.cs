using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for ATakListWindow.xaml
    /// </summary>
    /// 

    public partial class ATaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
       DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        private void ListView_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            return;
        }
        public BO.EngineerExperience ExperirnceFilter { get; set; } = BO.EngineerExperience.None;

        //for the data binding a filtered list
        private void ExperienceFlt(object sender, SelectionChangedEventArgs e)
        {
            //    TaskList = (ExperirnceFilter == BO.EngineerExperience.None) ?
            //        s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == ExperirnceFilter)!;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            new ATaskWindow().ShowDialog();
            TaskList = s_bl?.Task.ReadAll()!;
        }

        public ATaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll()!;

        }
    }
}
