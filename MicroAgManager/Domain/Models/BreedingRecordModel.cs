using Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;

namespace Domain.Models
{
    public class BreedingRecordModel:BaseModel
    {
        [Required][ForeignKey("Female")] public required long FemaleId { get; set; }
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

        public BreedingRecord MapToEntity(BreedingRecord breedingRecord)
        { 
            breedingRecord.FemaleId= FemaleId;
            breedingRecord.MaleId = MaleId;
            breedingRecord.ServiceDate = ServiceDate;
            breedingRecord.ResolutionDate = ResolutionDate;
            breedingRecord.StillbornMales = StillbornMales; 
            breedingRecord.StillbornFemales = StillbornFemales; 
            breedingRecord.Notes = Notes;   
            breedingRecord.BornMales = BornMales;
            breedingRecord.BornFemales = BornFemales;
            breedingRecord.Resolution = Resolution;
            return breedingRecord;
        }

    }
}
