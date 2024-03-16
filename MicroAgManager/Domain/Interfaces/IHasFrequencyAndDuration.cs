namespace Domain.Interfaces
{
    public interface IHasFrequencyAndDuration
    {
        decimal DurationScalar { get; set; }
        long? DurationUnitId { get; set; }
        decimal PerScalar { get; set; }
        long? PerUnitId { get; set; }
        decimal EveryScalar { get; set; }
        long? EveryUnitId { get; set; }
    }
}
