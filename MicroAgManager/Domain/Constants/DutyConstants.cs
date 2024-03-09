namespace Domain.Constants
{
    public static class DutyCommandConstants
    {
        public static string Complete { get; private set; } = "Complete";
        public static string Measurement { get; private set; } = "Measurement";
        public static string Treatment { get; private set; } = "Treatment";
        public static string Photograph { get; private set; } = "Photograph";
        public static string Registration { get; private set; } = "Registration";
        public static string Breed { get; private set; } = "Breed";
        public static string Service { get; private set; } = "Service";
        public static string Birth { get; private set; } = "Birth";
        public static string Feed { get; private set; } = "Feed";
        public static string Reap { get; private set; } = "Reap";
        public static readonly List<string> NonRecordCommands = new List<string>()
        {
            Birth,Breed,Complete,Reap,Service
        };
    }
    public static class DutyRelationshipConstants
    {
        public static string Self { get; private set; } = "Self";
        public static string Mother { get; private set; } = "Mother";
        public static string Father { get; private set; } = "Father";
    }
    public static class ScheduledDutySourceConstants
    {
        public static string Milestone { get; private set; } = "Milestone";
        public static string Event { get; private set; } = "Event";
        public static string Chore { get; private set; } = "Chore";
    }
    public static class ScheduledDutyRecordConstants
    {
        public static string BreedingRecord { get; private set; } = "BreedingRecord";
        public static string Measurement { get; private set; } = "Measurement";
        public static string TreatmentRecord { get; private set; } = "TreatmentRecord";
        public static string Registration { get; private set; } = "Registration";
    }
}
