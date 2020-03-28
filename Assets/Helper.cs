using System;

public static class Helper
{
    public static int ToInteger(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            str = "0";
        }

        return Convert.ToInt32(str.Replace(",", ""));
    }
}