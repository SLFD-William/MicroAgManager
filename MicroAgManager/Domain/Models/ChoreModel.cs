using Domain.Abstracts;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain.Models
{
    //generate the model for the Chore Entity using the standard conventions of this code base.
    //please
    public class ChoreModel : BaseModel,IChore,IHasFrequencyAndDuration
    {
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string Color { get; set; } = "transparent";
        //chore is due by noon once a day,
        //chode is due by 6:00am 2x a week every 2.5 days
        [Required] public TimeSpan DueByTime { get; set; } = new TimeSpan(12, 0, 0);

        public decimal DurationScalar { get; set; } = 0;
        [Precision(18, 3)] public long? DurationUnitId { get; set; }
        public virtual UnitModel? DurationUnit { get; set; }
        [Required][Precision(18, 3)] public decimal PerScalar { get; set; } = 1;
        [ForeignKey(nameof(PerUnit))][Required] public long? PerUnitId { get; set; }
        public virtual UnitModel? PerUnit { get; set; }
        [Required][Precision(18, 3)] public decimal EveryScalar { get; set; } = 1;
        [ForeignKey(nameof(EveryUnit))][Required] public long? EveryUnitId { get; set; }
        public virtual UnitModel? EveryUnit { get; set; }

        public virtual ICollection<DutyModel?> Duties { get; set; } = new List<DutyModel?>();
        public bool Enabled { get; set; } = true;

        DateTime IChore.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }
        ICollection<IDuty>? IChore.Duties { get => Duties as ICollection<IDuty>; set => Duties=value as ICollection<DutyModel?> ?? new List<DutyModel?>(); }
        
        public static ChoreModel? Create(Chore chore)
        {
            if (chore == null) return null;
            var model=PopulateBaseModel(chore, new ChoreModel {
                Name = chore.Name,
                Color = chore.Color,
                RecipientType = chore.RecipientType,
                RecipientTypeId = chore.RecipientTypeId,
                DueByTime = chore.DueByTime,
                DurationScalar = chore.DurationScalar,
                DurationUnitId = chore.DurationUnitId,
                PerScalar = chore.PerScalar,
                PerUnitId = chore.PerUnitId,
                EveryScalar = chore.EveryScalar,
                EveryUnitId = chore.EveryUnitId,
                Enabled=chore.Enabled,
                Duties = chore.Duties?.Select(DutyModel.Create).ToList() ?? new List<DutyModel?>()
            }) as ChoreModel;
            return model;
        }

        public override BaseModel Map(BaseModel chore)
        {
            if (chore is not ChoreModel) return null;
            ((ChoreModel)chore).Color = Color;
            ((ChoreModel)chore).DueByTime = DueByTime;
            ((ChoreModel)chore).Name=Name;
            ((ChoreModel)chore).RecipientType = RecipientType;
            ((ChoreModel)chore).RecipientTypeId = RecipientTypeId;
            ((ChoreModel)chore).EntityModifiedOn = EntityModifiedOn;
            ((ChoreModel)chore).DurationScalar = DurationScalar;
            ((ChoreModel)chore).DurationUnitId = DurationUnitId;
            ((ChoreModel)chore).PerUnitId = PerUnitId;
            ((ChoreModel)chore).PerScalar = PerScalar;
            ((ChoreModel)chore).EveryUnitId = EveryUnitId;
            ((ChoreModel)chore).EveryScalar = EveryScalar;
            ((ChoreModel)chore).Enabled = Enabled;
            return chore;
        }

        public override BaseEntity Map(BaseEntity chore)
        {
            if (chore is not Chore) return null;
            ((Chore)chore).Color = Color;
            ((Chore)chore).DueByTime = DueByTime;
            ((Chore)chore).Name = Name;
            ((Chore)chore).RecipientType = RecipientType;
            ((Chore)chore).RecipientTypeId = RecipientTypeId;
            ((Chore)chore).DurationScalar = DurationScalar;
            ((Chore)chore).DurationUnitId = DurationUnitId;
            ((Chore)chore).PerUnitId = PerUnitId;
            ((Chore)chore).PerScalar = PerScalar;
            ((Chore)chore).EveryUnitId = EveryUnitId;
            ((Chore)chore).EveryScalar = EveryScalar;
            ((Chore)chore).Enabled = Enabled;
            chore.ModifiedOn = DateTime.UtcNow;
            return chore;

        }
    }
}
