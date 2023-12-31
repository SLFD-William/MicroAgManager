﻿using Domain.Abstracts;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    //generate the model for the Chore Entity using the standard conventions of this code base.
    //please
    public class ChoreModel : BaseModel
    {
        [Required] public long RecipientTypeId { get; set; }
        [Required][MaxLength(40)] public string RecipientType { get; set; }
        [Required][MaxLength(40)] public string Name { get; set; }
        [Required][MaxLength(40)] public string Color { get; set; } = "transparent";
        //chore is due by noon once a day,
        //chode is due by 6:00am 2x a week every 2.5 days
        [Required] public TimeSpan DueByTime { get; set; } = new TimeSpan(12, 0, 0);
        [Required][Precision(18, 3)] public decimal Frequency { get; set; } = 1;
        [Required][ForeignKey(nameof(FrequencyUnit))] public long FrequencyUnitId { get; set; }
        [Required] public virtual Unit FrequencyUnit { get; set; }

        [Precision(18, 3)] public decimal? Period { get; set; } = 1;
        [ForeignKey(nameof(PeriodUnit))] public long? PeriodUnitId { get; set; }
        public virtual Unit? PeriodUnit { get; set; }
        public virtual ICollection<DutyModel?> Duties { get; set; } = new List<DutyModel>();

        public static ChoreModel? Create(Chore chore)
        {
            if (chore == null) return null;
            var model=PopulateBaseModel(chore, new ChoreModel {
                Name = chore.Name,
                Color = chore.Color,
                Frequency = chore.Frequency,
                FrequencyUnitId = chore.FrequencyUnitId,
                RecipientType = chore.RecipientType,
                RecipientTypeId = chore.RecipientTypeId,
                DueByTime = chore.DueByTime,
                PeriodUnitId = chore.PeriodUnitId,
                Period = chore.Period,
                Duties = chore.Duties?.Select(DutyModel.Create).ToList() ?? new List<DutyModel?>()
            }) as ChoreModel;
            return model;
        }

        public override BaseModel Map(BaseModel chore)
        {
            if (chore is not ChoreModel) return null;
            ((ChoreModel)chore).Color = Color;
            ((ChoreModel)chore).Frequency = Frequency;
            ((ChoreModel)chore).DueByTime = DueByTime;
            ((ChoreModel)chore).FrequencyUnitId = FrequencyUnitId;
            ((ChoreModel)chore).Name=Name;
            ((ChoreModel)chore).Period = Period;
            ((ChoreModel)chore).PeriodUnitId = PeriodUnitId;
            ((ChoreModel)chore).RecipientType = RecipientType;
            ((ChoreModel)chore).RecipientTypeId = RecipientTypeId;
            ((ChoreModel)chore).EntityModifiedOn = EntityModifiedOn;
            return chore;
        }

        public override BaseEntity Map(BaseEntity chore)
        {
            if (chore is not Chore) return null;
            ((Chore)chore).Color = Color;
            ((Chore)chore).Frequency = Frequency;
            ((Chore)chore).DueByTime = DueByTime;
            ((Chore)chore).FrequencyUnitId = FrequencyUnitId;
            ((Chore)chore).Name = Name;
            ((Chore)chore).Period = Period ?? 1;
            ((Chore)chore).PeriodUnitId = PeriodUnitId;
            ((Chore)chore).RecipientType = RecipientType;
            ((Chore)chore).RecipientTypeId = RecipientTypeId;
            chore.ModifiedOn = DateTime.UtcNow;
            return chore;

        }
    }
}
