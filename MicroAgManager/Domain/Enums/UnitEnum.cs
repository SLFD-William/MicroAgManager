using System.ComponentModel;

namespace Domain.Enums
{
    public class UnitEnum : BaseEnumeration
    {
        [Description("Undefined")]
        public static UnitEnum Undefined = new(0, nameof(Undefined));
        [Description("Pound")]
        public static UnitEnum Weight_Pounds = new(1, nameof(Weight_Pounds));
        [Description("Kilograms")]
        public static UnitEnum Weight_KiloGrams = new(2, nameof(Weight_KiloGrams));
        [Description("Each")]
        public static UnitEnum Weight_Each = new(3, nameof(Weight_Each));
        [Description("Inch")]
        public static UnitEnum Length_Inch = new(4, nameof(Length_Inch));
        [Description("Feet")]
        public static UnitEnum Length_Feet = new(5, nameof(Length_Feet));
        [Description("Farenheight")]
        public static UnitEnum Temperature_Farenheight = new(6, nameof(Temperature_Farenheight));
        [Description("Celsius")]
        public static UnitEnum Temperature_Celsius = new(7, nameof(Temperature_Celsius));
        
        
        [Description("Once")]
        public static UnitEnum Frequency_Once = new(8, nameof(Frequency_Once));
        [Description("Hourly")]
        public static UnitEnum Frequency_Hourly = new(9, nameof(Frequency_Hourly));
        [Description("Daily")]
        public static UnitEnum Frequency_Daily = new(10, nameof(Frequency_Daily));
        [Description("Monthly")]
        public static UnitEnum Frequency_Monthly = new(11, nameof(Frequency_Monthly));
        [Description("Yearly")]
        public static UnitEnum Frequency_Yearly = new(12, nameof(Frequency_Yearly));

        [Description("Cubic Centimeter")]
        public static UnitEnum Dosage_CubicCentimeter = new(13, nameof(Dosage_CubicCentimeter));
        [Description("Milliliter")]
        public static UnitEnum Dosage_Milliliter = new(14, nameof(Dosage_Milliliter));
        [Description("Grams")]
        public static UnitEnum Dosage_Grams = new(15, nameof(Dosage_Grams));
        [Description("per Gradient")]
        public static UnitEnum Dosage_perGradient = new(16, nameof(Dosage_perGradient));
        [Description("Each")]
        public static UnitEnum Dosage_Each = new(17, nameof(Dosage_Each));
        [Description("Percent")]
        public static UnitEnum Percent = new(18, nameof(Percent));

        [Description("Once")]
        public static UnitEnum Duration_Once = new(19, nameof(Duration_Once));
        [Description("Hours")]
        public static UnitEnum Duration_Hourly = new(20, nameof(Duration_Hourly));
        [Description("Days")]
        public static UnitEnum Duration_Days = new(21, nameof(Duration_Days));
        [Description("Months")]
        public static UnitEnum Duration_Months = new(22, nameof(Duration_Months));
        [Description("Years")]
        public static UnitEnum Duration_Years = new(23, nameof(Duration_Years));
        [Description("Weeks")]
        public static UnitEnum Duration_Weeks = new(24, nameof(Duration_Weeks));

        [Description("Acres")]
        public static UnitEnum Area_Acres = new(25, nameof(Area_Acres));

        public UnitEnum(long id, string name) : base(id, name)
        {
        }
    }
}
