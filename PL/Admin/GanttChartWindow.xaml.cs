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

namespace PL.Admin;

/// <summary>
/// Interaction logic for GanttChartWindow.xaml
/// </summary>
public partial class GanttChartWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public IEnumerable<BO.Task> TaskList
    {
        get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }
    public static readonly DependencyProperty TaskListProperty =
   DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(GanttChartWindow), new PropertyMetadata(null));

    public DateTime? ProjectStartDate
    {
        get { return (DateTime?)GetValue(DateTimeProperty); }
        set { SetValue(DateTimeProperty, value); }
    }
    public static readonly DependencyProperty DateTimeProperty =
   DependencyProperty.Register("ProjectStartDate", typeof(DateTime?), typeof(GanttChartWindow), new PropertyMetadata(null));
    public GanttChartWindow()
    {
        InitializeComponent();

        ProjectStartDate = s_bl.Task.GetProjectStartDate();
        TaskList = from task in s_bl.Task.ReadAll()
                   select s_bl.Task.Read(task.Id);
    }
}
