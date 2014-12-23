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
        public const string TADisciplineProcess = "Process";
        public const string TADisciplineEngineering = "Engineering";

        public enum Status
        {
            Submitted,
            Draft,
            CompletedDraft
        }
    }
}
