using DaNangZ.CoreLib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DaNangZ.Web.Common
{
    public static class Constant
    {
        public static int PageSize = 20;
        public static string Active = "Active";
        public static string Inactive = "Inactive";

        public static class StatusIndicator
        {
            public static string YettoStart = "Yet to Start";
            public static string InProgress = "In Progress";
            public static string Completed = "Completed";
            public static string UnabletoComplete = "Unable to Complete";
            public static string Overdue = "Overdue";
        }
    }
}