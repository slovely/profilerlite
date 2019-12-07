using System;

namespace ProfilerLite.Core.Models
{
    public static class Extensions
    {
        public static string ToHumanReadableTime(this int timeInMs)
        {
            if (timeInMs < 1000) return $"{timeInMs}ms";
            if (timeInMs < 60000) return $"{Math.Round(timeInMs / 1000d, 2)}s";
            return $"{Math.Round(timeInMs / 60000d, 2)}mins";
        }
    }
}