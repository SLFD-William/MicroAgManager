namespace Domain.Enums
{
    public class UnitTypeEnum : BaseEnumeration
    {
        public static UnitTypeEnum Weight = new UnitTypeEnum(1, nameof(Weight));
        public static UnitTypeEnum Length = new UnitTypeEnum(2, nameof(Length));
        public static UnitTypeEnum Temperature = new UnitTypeEnum(3, nameof(Temperature));
        public static UnitTypeEnum Frequency = new UnitTypeEnum(4, nameof(Frequency));
        public static UnitTypeEnum Dosage = new UnitTypeEnum(5, nameof(Dosage));
        public static UnitTypeEnum Area = new UnitTypeEnum(5, nameof(Area));
        public UnitTypeEnum(long id, string name) : base(id, name)
        {
        }
    }
}
