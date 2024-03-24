namespace Domain.Constants
{
    public static class LivestockFeedTypeConstants
    {
        public const string Hay = "Hay";
        public const string Haylage = "Haylage";
        public const string Silage = "Silage";
        public const string Concentrate = "Concentrate";
        public const string Supplement = "Supplement";
    }
    public static class LivestockCareConstants
    {
        public const string Individual = "Individual";
        public const string Collective = "Collective";
    }
    public static class LivestockStatusModeConstants
    {
        public const string Unchanged = "Unchanged";
        public const string True = "True";
        public const string False = "False";
    }
    public static class LivestockFeedQuantityUnitsConstants
    {
        public const string Pounds = "Pounds";
        public const string Bales = "Bales";
    }
    public static class LivestockFeedDistributionConstants
    {
        public const string FreeRange = "Free Range";
        public const string Serving = "Serving";
    }
    public static class LivestockFeedServingFrequenceConstants
    {
        public const string NoServing = "No Serving";
        public const string OnceADay = "One Serving Per Day";
        public const string TwiceADay = "Two Servings Per Day";
    }
    public static class LivestockFeedAnalysisParameterConstants
    {
        public const string DryMatter = "Dry Matter";
        public const string Protein = "Protein";
        public const string Fibres = "Fibres";
        public const string Energy = "Energy";
        public const string Minerals = "Minerals";
        public const string Calculation = "Calculation";
        public const string Other = "Other";
    }
    public static class MilestoneSystemRequiredConstants
    { 
        public const string Parturition = "Parturition";
        public const string Birth = DutyCommandConstants.Birth;
        public const string Death = "Death";
        public const string Breed = DutyCommandConstants.Breed;
    }
    public static class BreedingResolutionConstants
    {
        public const string Success = "Success";
        public const string NotImpregnated = "Not Impregnated";
    }
}
