using BlApi;
using System.Windows;

namespace PL.Admin;

/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    static readonly IBl s_bl = Factory.Get();



    public int EngineerId { get; set; } = 0;
    public string EngineerName { get; set; } = "";
    public string EngineerEmail { get; set; } = "";
    public double EngineerCost { get; set; } = 0;
    public int EngineerTask { get; set; } = 0;
    public BO.EngineerExperience EngineerLevel { get; set; } = BO.EngineerExperience.None;

    public BO.Engineer Engineer
    {
        get
        {
            return (BO.Engineer)GetValue(EngineerProperty);
        }
        set { SetValue(EngineerProperty, value); }
    }
    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
    public EngineerWindow(int Id = 0, int flag = 0)
    {

        InitializeComponent();
        try
        {
            BO.Engineer engineer = s_bl.Engineer.Read(Id)!;
            flag = 1;
        }
        catch
        {
            flag = 0;
        }

    }
    //public EngineerExists()
    //{ 
    ////}
}
