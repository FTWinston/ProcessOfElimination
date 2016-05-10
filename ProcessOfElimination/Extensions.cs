using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProcessOfElimination
{
    public static class Extensions
    {
        private static Random random = new Random();

        public static string ToSimpleString(this TimeSpan ts)
        {
            int parts = 0, partsToShow = 2;

            var sb = new StringBuilder();
            if (ts.Days > 0)
            {
                sb.Append(ts.Days);
                sb.Append(ts.Days == 1 ? " day" : " days");
                parts++;
            }

            if (parts < partsToShow && ts.Hours > 0)
            {
                if (parts > 0)
                    sb.Append(" ");
                sb.Append(ts.Hours);
                sb.Append(ts.Hours == 1 ? " hour" : " hours");
                parts++;
            }

            if (parts < partsToShow && ts.Minutes > 0)
            {
                if (parts > 0)
                    sb.Append(" ");
                sb.Append(ts.Minutes);
                sb.Append(ts.Minutes == 1 ? " minute" : " minutes");
                parts++;
            }

            if (parts == 0) // don't show seconds unless they're the only part
            {
                if (parts > 0)
                    sb.Append(" ");
                sb.Append(ts.Seconds);
                sb.Append(ts.Seconds == 1 ? " second" : " seconds");
                parts++;
            }

            return sb.ToString();
        }

        public static string Describe(this int value, string singular, string plural)
        {
            return string.Format("{0} {1}", value, value == 1 ? singular : plural);
        }

        public static string Ordinal(this int value)
        {
            if (value <= 0)
                return value.ToString();

            switch (value % 100)
            {
                case 11:
                case 12:
                case 13:
                    return value + "th";
            }

            switch (value % 10)
            {
                case 1:
                    return value + "st";
                case 2:
                    return value + "nd";
                case 3:
                    return value + "rd";
                default:
                    return value + "th";
            }
        }

        public static T PickRandom<T>(this ICollection<T> items)
        {
            var toSkip = random.Next(items.Count);
            return items.Skip(toSkip).Take(1).First();
        }

        public static void Shuffle<T>(this IList<T> items)
        {
            for (int i = 0; i < items.Count - 1; i++)
            {
                int j = random.Next(i, items.Count);

                T temp = items[j];
                items[j] = items[i];
                items[i] = temp;
            }
        }
    }
}