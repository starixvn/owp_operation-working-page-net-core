using OWP.Models;

namespace OWP.Helpers;
public static class HelperService
{
    private const int TimeZoneOffsetGMT8 = 0;

    public static ShiftTimeSpan ParseShiftTimeSpan(string shiftCode, int? timeZoneOffset)
    {
        var shiftCodes = shiftCode.Split('>');
        if (shiftCodes.Length == 2)
        {
            TimeSpan startTime;
            TimeSpan endTime;
            var isValidStartTime = TryParseTimeInShiftCode(shiftCodes[0], out startTime);
            var isValidEndTime = TryParseTimeInShiftCode(shiftCodes[1], out endTime);
            if (isValidStartTime && isValidEndTime)
            {
                var getStartTime = startTime.Add(new TimeSpan(0, -timeZoneOffset ?? 0, 0));
                var getEndTime = endTime.Add(new TimeSpan(0, -timeZoneOffset ?? 0, 0));

                return new ShiftTimeSpan
                {
                    StartTime = new TimeSpan(getStartTime.Hours, getStartTime.Minutes, 0),
                    EndTime = new TimeSpan(getEndTime.Hours, getEndTime.Minutes, 0)
                };
            }
        }

        return null;
    }

    public static ShiftTimeSpan ParseShiftTimeSpan(string shiftCode)
   => ParseShiftTimeSpan(shiftCode, TimeZoneOffsetGMT8);
    

    private static bool TryParseTimeInShiftCode(string timeText, out TimeSpan timeSpan)
    {
        string textHour;
        string textMinute;

        var hourAndMinute = timeText.Split(':');
        if (hourAndMinute.Length == 2)
        {
            textHour = hourAndMinute[0];
            textMinute = hourAndMinute[1];
        }
        else
        {
            textHour = timeText;
            textMinute = "0";
        }

        var isValidHour = int.TryParse(textHour, out var hour);
        var isValidMinute = int.TryParse(textMinute, out var minute);

        if (!isValidHour || !isValidMinute)
        {
            timeSpan = TimeSpan.Zero;
            return false;
        }

        if (hour is >= 0 and < 24 && minute is >= 0 and < 60)
        {
            timeSpan = new TimeSpan(hour, minute, 0);
            return true;
        }

        timeSpan = TimeSpan.Zero;
        return false;
    }

}
