using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
/// Interaction logic for Gantt.xaml
/// </summary>
public partial class Gantt : Window, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    private ObservableCollection<string> _monthLabels;
    public ObservableCollection<string> MonthLabels
    {
        get { return _monthLabels; }
        set { _monthLabels = value; OnPropertyChanged(nameof(MonthLabels)); }
    }

    private ObservableCollection<YearBorderData> _yearBorders;
    public ObservableCollection<YearBorderData> YearBorders
    {
        get { return _yearBorders; }
        set { _yearBorders = value; OnPropertyChanged(nameof(YearBorders)); }
    }

    public Gantt()
    {
        InitializeComponent();
        DataContext = this;

        // Initialize MonthLabels
        MonthLabels = new ObservableCollection<string>();
        DateTime startDate = new DateTime(2024, 1, 1); // Your chosen start date
        for (int i = 0; i < 12; i++)
        {
            MonthLabels.Add(startDate.AddMonths(i).ToString("MMM yyyy"));
        }

        // Initialize YearBorders
        YearBorders = new ObservableCollection<YearBorderData>();
        double width = 800 / 12; // Approximate size of a month
        for (int i = 0; i < 12; i++)
        {
            YearBorders.Add(new YearBorderData { Left = i * width, Width = width });
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class YearBorderData
    {
        public double Left { get; set; }
        public double Width { get; set; }
    }

}