using PL.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace PL;
class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class ConvertIdToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToAddBtnVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Visible" : "Hidden";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}




class ConvertIdToUpdateBtnVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Hidden" : "Visible";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertDateTimeToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not null)
            return ((DateTime)value).ToString("dd/MM/yyyy");
        else
            return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertListToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<BO.TaskInList> TaskList)
        {
            string listString = string.Join(", ", TaskList.Select(task => task.Id));
            if (listString.Length <= 1)
                return "0 dependencies";
            else
                return listString;
        }
        else
        {
            // Handle the case where value is not a List<int>
            return string.Empty; // or throw an exception, log an error, etc.
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConverteTaskToMargin : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Task task = (BO.Task)value;
        DateTime? ProjectStartDate = s_bl.Task.GetProjectStartDate();
        int days = 0;
        if (ProjectStartDate != null && task.ScheduledDate != null)
            days = ((TimeSpan)(task.ScheduledDate - ProjectStartDate)).Days * 10;
        if (days <= 0)
            days = 0;
        return new Thickness(days, 0, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConverteTaskToWidth : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        BO.Task task = (BO.Task)value;
        int days = 0;
        if (task.ScheduledDate != null)
            days = ((TimeSpan)(s_bl.Clock - task.ScheduledDate)).Days * 10;
        if (days <= 0)
            days = 0;
        return $"{days}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConverteStatusToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Task task = (BO.Task)value;

        if (task.Status == BO.Status.InJeopardy)
            return Brushes.Red;
        else if(task.Status == BO.Status.Done)
            return Brushes.LightGreen;
        else
            return Brushes.LightBlue;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}