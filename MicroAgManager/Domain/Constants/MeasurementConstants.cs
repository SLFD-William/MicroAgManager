﻿namespace Domain.Constants
{
    public static class MeasurementMethodConstants
    {
        public static string Direct { get; private set; } = "Direct";
    }
    public static class UnitCategoryConstants
    {
        
        public static KeyValuePair<string,string> Mass { get; private set; } =new(nameof(Mass),"g");
        public static KeyValuePair<string,string> Volume { get; private set; } =new(nameof(Volume),"L");
        public static KeyValuePair<string,string> Length { get; private set; } =new(nameof(Length),"m");
        public static KeyValuePair<string,string> Area { get; private set; } =new(nameof(Area),"m2");
        public static KeyValuePair<string,string> Time { get; private set; } =new(nameof(Time),"s");
        public static KeyValuePair<string,string> Count { get; private set; } =new(nameof(Count),"ea");
        public static KeyValuePair<string,string> Temperature { get; private set; } =new(nameof(Temperature),"C");

        public static Dictionary<string, string> DosageUnits { get; private set; } = new Dictionary<string, string>()
        {
            {Mass.Key,Mass.Value },
            {Volume.Key,Volume.Value },
            {Count.Key,Count.Value }
        };
        public static Dictionary<string,string> Units { get; private set; }=new Dictionary<string,string>()
        {
            {Mass.Key,Mass.Value },
            {Volume.Key,Volume.Value },
            {Length.Key,Length.Value },
            {Area.Key,Area.Value },
            {Time.Key,Time.Value },
            {Count.Key,Count.Value },
            {Temperature.Key,Temperature.Value }
        };
    }
}
