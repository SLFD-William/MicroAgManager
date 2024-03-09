namespace Domain.Constants
{
    public class TreatmentConstants
    {
        public static string Grooming { get; private set; } = nameof(Grooming);
        public static string Intramuscular { get; private set; } = nameof(Intramuscular);
        public static string Intravenous { get; private set; } = nameof(Intravenous);
        public static string Oral { get; private set; } = nameof(Oral);
        public static string Subcutaneous { get; private set; } = nameof(Subcutaneous);
        public static string Surgical { get; private set; } = nameof(Surgical);
        public static string Topical { get; private set; } = nameof(Topical);
        public static readonly List<string> MethodsByInvasiveness = new List<string>()
        {
            Grooming,
            Topical,
            Oral,
            Subcutaneous,
            Intramuscular,
            Intravenous,
            Surgical
        };

        public static readonly List<string> NonWithdrawalTreatments =new List<string>()
        {
            Grooming,
            Surgical
        };
    }
}
