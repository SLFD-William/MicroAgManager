namespace Domain.Constants
{
    public static class MeasurementMethodConstants
    {
        public const string Direct = "Direct";
    }
    public static class UnitCategoryConstants
    {
        
        public static KeyValuePair<string,string> Mass { get; private set; } =new(nameof(Mass),"g");
        public static KeyValuePair<string,string> Volume { get; private set; } =new(nameof(Volume),"L");
        public static KeyValuePair<string,string> Length { get; private set; } =new(nameof(Length),"m");
        public static KeyValuePair<string,string> Area { get; private set; } =new(nameof(Area),"m2");
        public static KeyValuePair<string,string> Time { get; private set; } =new(nameof(Time),"s");
        public static KeyValuePair<string, string> Frequency { get; private set; } = new(nameof(Frequency), "1/s");
        public static KeyValuePair<string,string> Count { get; private set; } =new(nameof(Count),"ea");
        public static KeyValuePair<string,string> Temperature { get; private set; } =new(nameof(Temperature),"C");

        public static readonly Dictionary<string, string> DosageUnits = new Dictionary<string, string>()
        {
            {Mass.Key,Mass.Value },
            {Volume.Key,Volume.Value },
            {Count.Key,Count.Value }
        };
        public static readonly Dictionary<string,string> Units =new Dictionary<string,string>()
        {
            {Mass.Key,Mass.Value },
            {Volume.Key,Volume.Value },
            {Length.Key,Length.Value },
            {Area.Key,Area.Value },
            {Time.Key,Time.Value },
            {Frequency.Key,Frequency.Value },
            {Count.Key,Count.Value },
            {Temperature.Key,Temperature.Value }
        };
    }
}
