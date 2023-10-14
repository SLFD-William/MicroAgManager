using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;
using Domain.Entity;

namespace BackEnd.BusinessLogic
{
    public static class DutyLogic
    {
        public static string GetRecordTypeFromCommand(Domain.Entity.Duty duty)
        { 
            switch (duty.Command)
            {
                case nameof(DutyCommandConstants.Measurement):
                    return nameof(Measurement);
                case nameof(DutyCommandConstants.Treatment):
                    return nameof(TreatmentRecord);
                default:
                    return string.Empty;
            }
        }
    }
}
