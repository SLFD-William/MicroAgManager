using System.ComponentModel;

namespace Domain.Enums
{
    public  class PlotUseEnum : BaseEnumeration
    {
        public PlotUseEnum(long id, string name) : base(id, name){}
        [Description("General Use")]
        public static PlotUseEnum GeneralUse = new(0, nameof(GeneralUse));
        [Description("Pasture")]
        public static PlotUseEnum Pasture = new(1, nameof(Pasture));
        [Description("Garden")]
        public static PlotUseEnum Garden = new(2, nameof(Garden));
        [Description("Outbuilding")]
        public static PlotUseEnum Outbuilding = new(3, nameof(Outbuilding));
    }
}
