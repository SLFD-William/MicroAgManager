namespace Domain.Interfaces
{
    public interface IHasReschedule
    {
        public bool? Reschedule { get; set; }
        public DateTime? RescheduleDueOn { get; set; }
    }
}
