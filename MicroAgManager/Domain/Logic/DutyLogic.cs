using Domain.Constants;
using Domain.Entity;

namespace Domain.Logic
{
    public static class DutyLogic
    {
        public static string GetRecordTypeFromCommand(Duty duty)
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
