using Domain.Constants;
using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.Duty
{
    public class DutySummary : ValueObject
    {
        private DutyModel _dutyModel;
        public DutySummary(DutyModel dutyModel, FrontEndDbContext context)
        {
            _dutyModel = dutyModel;
            EntityName = _dutyModel.GetType().Name.Replace("Model", string.Empty); ;
            Gender= string.IsNullOrEmpty(_dutyModel.Gender) ? string.Empty : _dutyModel.Gender == GenderConstants.Male ? nameof(GenderConstants.Male) : nameof(GenderConstants.Female);
            switch (_dutyModel.Command)
            {
                case nameof(DutyCommandConstants.Registration):
                    CommandInstance = context.Registrars.Find(_dutyModel.CommandId).Name ?? "Registrar Not Found";
                    break;
                case nameof(DutyCommandConstants.Measurement):
                    CommandInstance = context.Measures.Find(_dutyModel.CommandId).Name ?? "Measure Not Found";
                    break;
                case nameof(DutyCommandConstants.Treatment):
                    CommandInstance = context.Treatments.Find(_dutyModel.CommandId).Name ?? "Treatment Not Found";
                    break;
                default:
                    CommandInstance = "No Application";
                    break;
            }
            switch (_dutyModel.RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    RecipientInstance = context.LivestockAnimals.Find(_dutyModel.RecipientTypeId).Name;
                    break;
                case nameof(RecipientTypeConstants.None):
                    RecipientInstance = "Self";
                    break;
                default:
                    RecipientInstance = "No Application";
                    break;
            }

        }
        public long Id => _dutyModel.Id;
        public string Name => _dutyModel.Name;
        public int DaysDue => _dutyModel.DaysDue;
        public string Command => _dutyModel.Command;
        public string CommandInstance { get; private set; }
        public string Procedure => _dutyModel.ProcedureLink;

        public string RecipientType => _dutyModel.RecipientType;
        public string RecipientInstance { get; private set; }
        public string Relationship => _dutyModel.Relationship;
        public string EntityName { get; private set; }
        public string Gender { get; private set; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _dutyModel;
        }
    }
}
