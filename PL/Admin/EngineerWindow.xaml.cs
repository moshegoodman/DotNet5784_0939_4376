using BlApi;
using System;
using System.Windows;

namespace PL.Admin;

public partial class EngineerWindow : Window
{
    static readonly IBl s_bl = Factory.Get();

    int is_Add = 0;

    //engineer object for working with the data 
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
    //window ctor
    public EngineerWindow(int id = 0)
    {
        is_Add = id;
        InitializeComponent();
        if (id == 0)
        {
            Engineer = new BO.Engineer()
            {
                Id = 0,
                Name = "",
                Email = "",
                Cost = 0,
                Task = null,
                Level = BO.EngineerExperience.None
            };
        }
        else
        {
            try
            {
                Engineer = s_bl.Engineer.Read(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
    }

    //opens the add/update window
    private void Btn_Add_Update_Click(object sender, RoutedEventArgs e)
    {

        if (is_Add == 0)
            try
            {
                s_bl.Engineer.Create(Engineer);
                Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }
        else
            try
            {
                s_bl.Engineer.Update(Engineer);
                Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); }

    }
}
