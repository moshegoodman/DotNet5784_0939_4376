using BlApi;
using System;
using System.Windows;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for UpdateScheduleWindow.xaml
    /// </summary>
    public partial class UpdateScheduleWindow : Window
    {
        static readonly IBl s_bl = Factory.Get();


        public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(ATaskWindow), new PropertyMetadata(null));


        //
        public BO.Task task
        {
            get
            {
                return (BO.Task)GetValue(TaskProperty);
            }
            set { SetValue(TaskProperty, value); }
        }

        public DateTime ScheduledDate { get; set; }



        public UpdateScheduleWindow(int id)
        {

            InitializeComponent();
            task = s_bl.Task.Read(id)!;


        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Update(task.Id, ScheduledDate);
                Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
