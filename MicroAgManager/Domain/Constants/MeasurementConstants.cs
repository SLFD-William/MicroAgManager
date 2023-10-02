namespace Domain.Constants
{
    public static class MeasurementMethodConstants
    {
        public static string Direct { get; private set; } = "Direct";
    }
    public static class MeasurementTypeConstants
    {
        public static string Weight { get; private set; } = "Weight";
        public static string Length { get; private set; } = "Length";
        public static string Temperature { get; private set; } = "Temperature";
        public static string Frequency { get; private set; } = "Frequency";
        public static string Volume { get; private set; } = "Volume";
        public static string Area { get; private set; } = "Area";
    }
    public static class MeasurementUnitConstants
    {
        public static string Undefined { get; private set; } = "Undefined";
        public static string Weight_Pounds { get; private set; } = "Pounds";
        public static string Weight_KiloGrams { get; private set; } = "kilograms";
        public static string Weight_Grams { get; private set; } = "grams";
        public static string Length_Inch { get; private set; } = "Inch";
        public static string Length_Feet { get; private set; } = "Feet";
        public static string Temperature_Farenheight { get; private set; } = "Farenheight";
        public static string Temperature_Celsius { get; private set; } = "Celsius";

        public static string Frequency_Once { get; private set; } = "Once";
        public static string Frequency_Hourly { get; private set; } = "Hourly";
        public static string Frequency_Daily { get; private set; } = "Daily";
        public static string Frequency_Monthly { get; private set; } = "Monthly";
        public static string Frequency_Yearly { get; private set; } = "Yearly";

        public static string Volume_CubicCentimeter { get; private set; } = "Cubic Centimeter";
        public static string Volume_Milliliter = "Milliliter";

        public static string Count_Each { get; private set; } = "Each";
        public static string Count_Percent { get; private set; } = "Percent";
        public static string Dosage_perGradient { get; private set; } = "per Gradient";
        
        public static string Duration_Hours { get; private set; } = "Hours";
        public static string Duration_Days { get; private set; } = "Days";
        public static string Duration_Months { get; private set; } = "Months";
        public static string Duration_Years { get; private set; } = "Years";
        public static string Duration_Weeks { get; private set; } = "Weeks";
        public static string Area_Acres { get; private set; } = "Acres";
    }
}
