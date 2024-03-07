using BlApi;
using PL.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EEngineerWindow.xaml
    /// </summary>
    public partial class EEngineerWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();


        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(EEngineerWindow), new PropertyMetadata(null));
        public BO.Engineer Engineer
        {
            get
            {
                return (BO.Engineer)GetValue(EngineerProperty);
            }
            set { SetValue(EngineerProperty, value); }
        }
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EEngineerWindow), new PropertyMetadata(null));
        public EEngineerWindow(int id)
        {
            InitializeComponent();
            try
            {

                Engineer = s_bl.Engineer.Read(id)!;
                //initialize the relevent tasks for the engineer
                TaskList = s_bl.Task.ReadAll(task => task.Complexity <= Engineer.Level && task.Engineer == null && task.Dependencies.All(d => s_bl.Task.Read(d.Id)!.CompleteDate != null));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Completed_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Engineer.Task != null)
            {
                try
                {
                    s_bl.Task.SetCompleteDate(Engineer.Task.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                try
                {

                    Engineer = s_bl.Engineer.Read(Engineer.Id)!;
                    //initialize the relevent tasks for the engineer
                    TaskList = s_bl.Task.ReadAll(task => task.Complexity <= Engineer.Level && task.Engineer == null && task.Dependencies.All(d => s_bl.Task.Read(d.Id)!.CompleteDate != null));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void Select_Task(object sender, SelectionChangedEventArgs e)
        {
            BO.TaskInList? taskInList = (sender as ComboBox)?.SelectedItem as BO.TaskInList;
            if (taskInList != null)
            {
                try
                {
                    s_bl?.Task.DesignateEngineer(taskInList.Id, Engineer.Id);
                    s_bl?.Task.SetStartDate(taskInList.Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                try
                {

                    Engineer = s_bl!.Engineer.Read(Engineer.Id)!;
                    //initialize the relevent tasks for the engineer
                    TaskList = s_bl.Task.ReadAll(task => task.Complexity <= Engineer.Level && task.Engineer == null && task.Dependencies.All(d => s_bl.Task.Read(d.Id)!.CompleteDate != null));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
