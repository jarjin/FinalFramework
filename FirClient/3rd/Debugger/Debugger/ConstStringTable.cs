using System;

public static class ConstStringTable
{    
    static string[] secDict = new string[100];
    static string[] tenDict = new string[10];

    static ConstStringTable()
    {
        for (int i = 0; i < 100; i++)
        {
            secDict[i] = string.Intern(i.ToString("00"));
        }

        for (int i = 0; i < 10; i++)
        {
            tenDict[i] = string.Intern(i.ToString());
        }
    }

    static public string GetTimeIntern(int time)
    {
        if (time < 0 || time > 99)
        {
            return time.ToString();
        }

        return secDict[time];        
    }

    static public string GetNumIntern(int num)
    {
        if (num < 0 || num > 99)
        {
            return num.ToString();
        }

        return num < 10 ? tenDict[num] : secDict[num];
    }
}
