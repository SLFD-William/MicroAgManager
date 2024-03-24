namespace Domain.Constants
{
    public static class DutyCommandConstants
    {
        public const string Complete  = "Complete";
        public const string Measurement = "Measurement";
        public const string Treatment = "Treatment";
        public const string Photograph = "Photograph";
        public const string Registration = "Registration";
        public const string Breed = "Breed";
        public const string Service = "Service";
        public const string Birth = "Birth";
        public const string Feed = "Feed";
        public const string Reap = "Reap";
        public static readonly List<string> NonRecordCommands = new List<string>()
        {
            Birth,Breed,Complete,Reap,Service
        };
        public static readonly List<string> AllCommands = new List<string>()
        {
            Birth,Breed,Complete,Reap,Service,Measurement,Treatment,Photograph,Registration
        };
        public static readonly List<string> AvailableCommands = new List<string>()
        {
            Breed,Service,Measurement,Treatment,Registration
        };
    }
    public static class DutyRelationshipConstants
    {
        public const string Self = "Self";
        public const string Mother  = "Mother";
        public const string Father  = "Father";
    }
    public static class ScheduledDutySourceConstants
    {
        public const string Milestone  = "Milestone";
        public const string Event  = "Event";
        public const string Chore  = "Chore";
    }
    public static class ScheduledDutyRecordConstants
    {
        public const string BreedingRecord  = "BreedingRecord";
        public const string Measurement  = "Measurement";
        public const string TreatmentRecord  = "TreatmentRecord";
        public const string Registration  = "Registration";
    }
}
