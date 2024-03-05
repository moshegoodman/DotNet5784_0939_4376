using BlApi;
using PL.Admin;
using System;
using System.Windows;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for Window1.xaml
/// </summary>
public partial class NameInputDialogWindow : Window
{
    static readonly IBl s_bl = Factory.Get();
    BO.Engineer CurrentEngineer;
    public int UserId { get; set; }

    public NameInputDialogWindow()
    {
        InitializeComponent();
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CurrentEngineer = s_bl.Engineer.Read(UserId);
            Close();
            new EngineerWindow(UserId).ShowDialog();


        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

