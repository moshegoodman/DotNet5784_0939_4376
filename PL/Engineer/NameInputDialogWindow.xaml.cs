using BlApi;
using System;
using System.Windows;
using System.Windows.Input;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for Window1.xaml
/// </summary>
public partial class NameInputDialogWindow : Window
{
    static readonly IBl s_bl = Factory.Get();
    BO.Engineer CurrentEngineer;
    public int? UserId { get; set; }

    public NameInputDialogWindow()
    {
        InitializeComponent();
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CurrentEngineer = s_bl.Engineer.Read((int)UserId!)!;
            Close();
            new EEngineerWindow((int)UserId).ShowDialog();


        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            try
            {
                CurrentEngineer = s_bl.Engineer.Read((int)UserId!)!;
                Close();
                new EEngineerWindow((int)UserId).ShowDialog();


            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
        }
    }
}

