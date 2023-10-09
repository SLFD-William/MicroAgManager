using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UnitModel:BaseModel
    {
        [Required][MaxLength(20)] required public string Name { get; set; }
        [Required][MaxLength(20)] required public string Category { get; set; }
        [Required][MaxLength(20)] required public string Symbol { get; set; }
        [Required] required public double ConversionFactorToSIUnit { get; set; } = 0;

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
        public Unit MapToEntity(Unit unit)
        {
            unit.Name = Name;
            unit.Category = Category;
            unit.Symbol = Symbol;
            unit.ConversionFactorToSIUnit = ConversionFactorToSIUnit;
            unit.ModifiedOn = DateTime.UtcNow;
            return unit;
        }
    }
   
}
