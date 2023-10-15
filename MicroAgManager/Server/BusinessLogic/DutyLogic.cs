using Domain.Constants;

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
