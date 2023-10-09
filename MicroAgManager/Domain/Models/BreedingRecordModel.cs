using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;

namespace Domain.Models
{
    public class BreedingRecordModel:BaseModel
    {
        [Required][ForeignKey("Female")] public long FemaleId { get; set; }
        [ForeignKey("Male")] public long? MaleId { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public int? StillbornMales { get; set; }
        public int? StillbornFemales { get; set; }
        public int? BornMales { get; set; }
        public int? BornFemales { get; set; }
        [MaxLength(40)]public string? Resolution { get; set; }

        public string Notes { get; set; }

        public static BreedingRecordModel? Create(BreedingRecord breedingRecord)
        {
            if(breedingRecord==null) return null;
            var model = PopulateBaseModel(breedingRecord, new BreedingRecordModel
            {
                FemaleId = breedingRecord.FemaleId,
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
        
        public override BaseModel Map(BaseModel breedingRecord)
        {
            if (breedingRecord == null || breedingRecord is not BreedingRecordModel) return null;

            ((BreedingRecordModel)breedingRecord).FemaleId = FemaleId;
            ((BreedingRecordModel)breedingRecord).MaleId = MaleId;
            ((BreedingRecordModel)breedingRecord).ServiceDate = ServiceDate;
            ((BreedingRecordModel)breedingRecord).ResolutionDate = ResolutionDate;
            ((BreedingRecordModel)breedingRecord).StillbornMales = StillbornMales;
            ((BreedingRecordModel)breedingRecord).StillbornFemales = StillbornFemales;
            ((BreedingRecordModel)breedingRecord).Notes = Notes;
            ((BreedingRecordModel)breedingRecord).BornMales = BornMales;
            ((BreedingRecordModel)breedingRecord).BornFemales = BornFemales;
            ((BreedingRecordModel)breedingRecord).Resolution = Resolution;
            return breedingRecord;
        }

        public override BaseEntity Map(BaseEntity breedingRecord)
        {

            ((BreedingRecord)breedingRecord).FemaleId = FemaleId;
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
            return breedingRecord;
        }
    }
}
