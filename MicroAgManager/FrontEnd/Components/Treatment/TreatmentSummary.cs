using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.Treatment
{
    public class TreatmentSummary : ValueObject
    {
        private TreatmentModel _treatmentModel;
        public TreatmentSummary(TreatmentModel treatmentModel, FrontEndDbContext context)
        {
            _treatmentModel = treatmentModel;
            DosageUnit = context.Units.Find(treatmentModel.DosageUnitId)?.Symbol ?? string.Empty;
            RecipientMassUnit = context.Units.Find(treatmentModel.RecipientMassUnitId)?.Symbol ?? string.Empty;
            FrequencyUnit = context.Units.Find(treatmentModel.FrequencyUnitId)?.Symbol ?? string.Empty;
            DurationUnit = context.Units.Find(treatmentModel.DurationUnitId)?.Symbol ?? string.Empty;
        }
        public string DosageUnit { get; private set; }
        public string RecipientMassUnit { get; private set; }
        public string FrequencyUnit { get; private set; }
        public string DurationUnit { get; private set; }

        public long Id => _treatmentModel.Id;
        public string Name => _treatmentModel.Name;
        public string BrandName => _treatmentModel.BrandName;
        public string Reason => _treatmentModel.Reason;
        public string LabelMethod => _treatmentModel.LabelMethod;
        public int MeatWithdrawal => _treatmentModel.MeatWithdrawal;
        public int MilkWithdrawal => _treatmentModel.MilkWithdrawal;
        public decimal DosageAmount => _treatmentModel.DosageAmount;
        public decimal RecipientMass=> _treatmentModel.RecipientMass;
        public decimal Frequency => _treatmentModel.Frequency;
        public decimal Duration => _treatmentModel.Duration;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _treatmentModel;
        }
    }
}

