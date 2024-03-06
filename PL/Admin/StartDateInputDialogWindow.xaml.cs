using BlApi;
using System;
using System.Windows;

namespace PL.Admin;

/// <summary>
/// Interaction logic for StartDateInputDialogWindow.xaml
/// </summary>
public partial class StartDateInputDialogWindow : Window
{
    static readonly IBl s_bl = Factory.Get();
    public DateTime StartDate { get; set; }
    public StartDateInputDialogWindow()
    {
        InitializeComponent();
    }
    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Task.SetStage2(StartDate);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
        Close();
        new ATaskListWindow().Show();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
