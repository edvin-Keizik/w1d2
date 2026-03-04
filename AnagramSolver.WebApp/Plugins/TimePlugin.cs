using System.ComponentModel;
using Microsoft.SemanticKernel;

public class TimePlugin
{
    [KernelFunction, Description("Gets current date and time")]
    public string GetCurrentTime()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    [KernelFunction, Description("Gets current day of the week")]
    public string GetDayOfWeek()
    {
        return DateTime.Now.ToString("dddd");
    }

    [KernelFunction, Description("Calculates time difference in hours between current time and provided date")]
    public string GetHoursSince(string dateTime)
    {
        if (DateTime.TryParse(dateTime, out var parsedDate))
        {
            var difference = DateTime.Now - parsedDate;
            return $"{difference.TotalHours:F2} hours";
        }
        return "Invalid date format";
    }
}
