using BlApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
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
class ConvertStatus1ToVisible : IValueConverter
{
    static readonly IBl s_bl = Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return s_bl.Task.GetProjectStatus() == 1 ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConvertStatus1AndNoDependencyToVisible : IValueConverter
{
    static readonly IBl s_bl = Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return s_bl.Task.GetProjectStatus() == 1 ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConvertStatus1ToEnabled : IValueConverter
{
    static readonly IBl s_bl = Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return s_bl.Task.GetProjectStatus() == 1 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStatus1ToHidden : IValueConverter
{
    static readonly IBl s_bl = Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return s_bl.Task.GetProjectStatus() == 1 ? Visibility.Hidden : Visibility.Visible;
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
        return (int)value == 0 ? Visibility.Visible : Visibility.Hidden;
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
        return (int)value == 0 ? Visibility.Hidden : Visibility.Visible;
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

class ConvertTimeSpanToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not null)
            return $"{((TimeSpan)value).Days} Days";

        else
            return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TimeSpan? timespan = new((int.Parse((string)value)), 0, 0, 0);
        return timespan;
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
            days = ((TimeSpan)(task.ScheduledDate - ProjectStartDate)).Days * 4;
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
        if (task.ScheduledDate != null && task.RequiredEffortTime != null)
            days = (task.RequiredEffortTime).Value.Days * 4;
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
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Task task = (BO.Task)value;

        if (task.ForecastDate != null && task.ForecastDate < s_bl.Clock && (task.Status != BO.Status.Done || task.CompleteDate > task.ForecastDate))
            return Brushes.Red;
        else if (task.Status == BO.Status.Done && task.CompleteDate <= task.ForecastDate)
            return Brushes.LightGreen;
        else
            return Brushes.LightBlue;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertNullToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertNotNullToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertTaskInEngineerToVisibility : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null || s_bl.Task.Read(((BO.TaskInEngineer)value).Id)!.CompleteDate != null ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class ConvertProjectStatusToVisibility : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return s_bl.Task.GetProjectStartDate == null ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConverteTaskToDates : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        BO.Task task = (BO.Task)value;
        string _scheduledDate, _startDate, _forecastDate, _completeDate, _requiredEffortTime = "";
        _scheduledDate = task.ScheduledDate != null ? task.ScheduledDate.Value.ToString("dd/MM/yyyy") + " -" : "No Scheduled";
        _startDate = task.StartDate != null ? task.StartDate.Value.ToString("dd/MM/yyyy") + " -" : "";
        _forecastDate = task.ForecastDate != null ? task.ForecastDate.Value.ToString("dd/MM/yyyy") : "";
        _completeDate = task.CompleteDate != null ? task.CompleteDate.Value.ToString("dd/MM/yyyy") : "";
        _requiredEffortTime = task.RequiredEffortTime != null ? task.RequiredEffortTime.Value.Days.ToString() : "";
        if (task.ScheduledDate != null)
            return $"Scheduled:   {_requiredEffortTime} days\n{_scheduledDate} {_forecastDate}\nExecuted:\n{_startDate} {_completeDate}";
        else
            return "";


    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


