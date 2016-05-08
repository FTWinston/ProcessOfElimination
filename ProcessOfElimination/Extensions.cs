using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProcessOfElimination
{
    public static class Extensions
    {
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
    }
}