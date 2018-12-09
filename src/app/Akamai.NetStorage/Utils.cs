namespace Akamai.NetStorage
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    static class Utils
    {
        public static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetEpochSeconds(this DateTime current)
        {
            return (long)current.Subtract(EpochStart).TotalSeconds;
        }

        public static DateTime FromEpochSeconds(this long seconds) =>
            EpochStart.AddSeconds(seconds);

        public static string ToHex(this byte[] data) => 
            string.Concat(Array.ConvertAll(data, x => x.ToString("X2"))).ToLower();

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source) =>
            new ReadOnlyCollection<T>(source.ToList());


        public static T EnsureNotNull<T>(this T? value) where T : struct
        {
            if (value == null)
                throw new InvalidOperationException();

            return value.Value;
        }

        public static R? Map<T, R>(this T? value, Func<T, R> map) 
            where T : struct 
            where R : struct
        {
            if (value == null)
                return null;

            return map(value.Value);
        }
    }
}