using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Constant
{
    public static class Constant
    {
        public const string InsBy = "InsBy";
        public const string InsAt = "InsAt";
        public const string UpdBy = "UpdBy";
        public const string UpdAt = "UpdAt";
        public const string StatusId = "StatusId";
        public const string Active = "StatusActive";
        public const string InActive = "StatusInactive";
        public const string Deleted = "StatusInactive";

        public static class StatusIndicator
        {
            public static string YettoStart = "Yet to Start";
            public static string InProgress = "In Progress";
            public static string Completed = "Completed";
            public static string UnabletoComplete = "Unable to Complete";
            public static string Overdue = "Overdue";
        }

        public enum Status
        {
            Submitted,
            Draft,
            CompletedDraft
        }
    }
}
