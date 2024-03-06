using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for TaskListForSchedule.xaml
    /// </summary>
    public partial class TaskListForSchedule : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
       DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListForSchedule), new PropertyMetadata(null));



        public TaskListForSchedule()
        {
            TaskList = s_bl?.Task.ReadAll(task => task.ScheduledDate == null)!;

            InitializeComponent();
        }


        private void ListView_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            BO.TaskInList? task_in_list = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task_in_list != null)
            {
                new UpdateScheduleWindow(task_in_list.Id).ShowDialog();
                TaskList = s_bl?.Task.ReadAll(task => task.ScheduledDate == null)!;

            }
        }
    }
}
