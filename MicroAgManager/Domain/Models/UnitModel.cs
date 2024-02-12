using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class UnitModel:BaseModel,IUnit
    {
        [Required][MaxLength(20)]  public string Name { get; set; }
        [Required][MaxLength(20)]  public string Category { get; set; }
        [Required][MaxLength(20)]  public string Symbol { get; set; }
        [Required] public double ConversionFactorToSIUnit { get; set; } = 0;
        [NotMapped]DateTime IUnit.ModifiedOn { get => EntityModifiedOn; set => EntityModifiedOn = value== EntityModifiedOn ? EntityModifiedOn: EntityModifiedOn; }

        public static UnitModel Create(Unit unit)
        {
            var model = PopulateBaseModel(unit, new UnitModel
            {
                Name = unit.Name,
                Category = unit.Category,
                Symbol = unit.Symbol,
                ConversionFactorToSIUnit = unit.ConversionFactorToSIUnit
            }) as UnitModel;
            return model;
        }
        public override BaseModel Map(BaseModel unit)
        {
            if (unit is not UnitModel || unit is null) return null;
            ((UnitModel)unit).Name = Name;
            ((UnitModel)unit).Category = Category;
            ((UnitModel)unit).Symbol = Symbol;
            ((UnitModel)unit).ConversionFactorToSIUnit = ConversionFactorToSIUnit;
            ((UnitModel)unit).EntityModifiedOn = EntityModifiedOn;
            return unit;
        }

        public override BaseEntity Map(BaseEntity unit)
        {
            if (unit is not Unit || unit is null) return null;
            ((Unit)unit).Name = Name;
            ((Unit)unit).Category = Category;
            ((Unit)unit).Symbol = Symbol;
            ((Unit)unit).ConversionFactorToSIUnit = ConversionFactorToSIUnit;
            unit.ModifiedOn = DateTime.UtcNow;
            return unit;
        }
    }
   
}
