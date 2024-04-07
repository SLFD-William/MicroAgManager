using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Logic;

namespace Domain.Models
{
    public class BreedingRecordModel :BaseModel, IHasRecipient, IBreedingRecord
    {
        [NotMapped] DateTime IBreedingRecord.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value == EntityModifiedOn ? EntityModifiedOn : EntityModifiedOn; }
        [Required][ForeignKey("Female")] public long FemaleId { get; set; }
        [ForeignKey("Male")] public long? MaleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public int? StillbornMales { get; set; }
        public int? StillbornFemales { get; set; }
        public int? BornMales { get; set; }
        public int? BornFemales { get; set; }
        [MaxLength(40)]public string? Resolution { get; set; }

        public string? Notes { get; set; }
        public long RecipientTypeId { get; set; }
        public string RecipientType { get; set; }
        [NotMapped]public long RecipientId { get => FemaleId; set => FemaleId=value; }
                
        public static BreedingRecordModel? Create(BreedingRecord breedingRecord)
        {
            if(breedingRecord==null) return null;
            var model = PopulateBaseModel(breedingRecord, new BreedingRecordModel
            {
                FemaleId = breedingRecord.FemaleId,
                RecipientTypeId = breedingRecord.RecipientTypeId,
                RecipientType = breedingRecord.RecipientType,
                MaleId = breedingRecord.MaleId,
                ServiceDate = breedingRecord.ServiceDate,
                ResolutionDate = breedingRecord.ResolutionDate,
                StillbornMales = breedingRecord.StillbornMales,
                StillbornFemales = breedingRecord.StillbornFemales,
                Notes = breedingRecord.Notes,
                BornFemales= breedingRecord.BornFemales,
                BornMales= breedingRecord.BornMales,
                Resolution= breedingRecord.Resolution
            }) as BreedingRecordModel;
            return model;
        }
        
        public override BaseEntity Map(BaseEntity breedingRecord)
        {

            ((BreedingRecord)breedingRecord).FemaleId = FemaleId;
            ((BreedingRecord)breedingRecord).RecipientTypeId = RecipientTypeId;
            ((BreedingRecord)breedingRecord).RecipientType = RecipientType;
            ((BreedingRecord)breedingRecord).MaleId = MaleId;
            ((BreedingRecord)breedingRecord).ServiceDate = ServiceDate;
            ((BreedingRecord)breedingRecord).ResolutionDate = ResolutionDate;
            ((BreedingRecord)breedingRecord).StillbornMales = StillbornMales;
            ((BreedingRecord)breedingRecord).StillbornFemales = StillbornFemales;
            ((BreedingRecord)breedingRecord).Notes = Notes;
            ((BreedingRecord)breedingRecord).BornMales = BornMales;
            ((BreedingRecord)breedingRecord).BornFemales = BornFemales;
            ((BreedingRecord)breedingRecord).Resolution = Resolution;
            breedingRecord.ModifiedOn = DateTime.UtcNow;
            if (((BreedingRecord)breedingRecord).MaleId == 0) ((BreedingRecord)breedingRecord).MaleId = null;
            return breedingRecord;
        }

        public override BaseModel Map(BaseModel breedingRecord)
        {
            if (breedingRecord == null || breedingRecord is not BreedingRecordModel) return null;

            ((BreedingRecordModel)breedingRecord).FemaleId = FemaleId;
            ((BreedingRecordModel)breedingRecord).RecipientTypeId = RecipientTypeId;
            ((BreedingRecordModel)breedingRecord).RecipientType = RecipientType;
            ((BreedingRecordModel)breedingRecord).MaleId = MaleId;
            ((BreedingRecordModel)breedingRecord).ServiceDate = ServiceDate;
            ((BreedingRecordModel)breedingRecord).ResolutionDate = ResolutionDate;
            ((BreedingRecordModel)breedingRecord).StillbornMales = StillbornMales;
            ((BreedingRecordModel)breedingRecord).StillbornFemales = StillbornFemales;
            ((BreedingRecordModel)breedingRecord).Notes = Notes;
            ((BreedingRecordModel)breedingRecord).BornMales = BornMales;
            ((BreedingRecordModel)breedingRecord).BornFemales = BornFemales;
            ((BreedingRecordModel)breedingRecord).Resolution = Resolution;
            ((BreedingRecordModel)breedingRecord).EntityModifiedOn = EntityModifiedOn;
            return breedingRecord;
        }
        public void PopulateDynamicRelations(DbContext genericContext)
        {
            var db = genericContext as IFrontEndDbContext;
            if (db is null) return;
            RecipientLogic.PopulateDynamicRelations(genericContext, this);
            if (RecipientType == nameof(LivestockAnimal) || RecipientType == nameof(LivestockBreed))
                MaleName = db.Livestocks.Find(MaleId)?.Name ?? string.Empty;
        }
        [NotMapped] public string FemaleName { get; set; } = string.Empty;
        [NotMapped] public string MaleName { get; private set; } = string.Empty;
        [NotMapped] public string RecipientTypeItem { get; set; } = string.Empty;
        
        [NotMapped] string IHasRecipient.RecipientItem { get=> FemaleName; set => FemaleName=value; }
    }
}
