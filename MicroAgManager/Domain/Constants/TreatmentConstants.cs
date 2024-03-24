namespace Domain.Constants
{
    public class TreatmentConstants
    {
        public const string Grooming  = nameof(Grooming);
        public const string Intramuscular  = nameof(Intramuscular);
        public const string Intravenous  = nameof(Intravenous);
        public const string Oral  = nameof(Oral);
        public const string Subcutaneous  = nameof(Subcutaneous);
        public const string Surgical  = nameof(Surgical);
        public const string Topical  = nameof(Topical);
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
