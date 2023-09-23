using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;

namespace FrontEnd.Components.Milestone
{
    public class MilestoneSummary : ValueObject
    {
        private MilestoneModel _milestoneModel;
        public int DutyCount { get; private set; }
        public int EventCount { get; private set; }
        public MilestoneSummary(MilestoneModel milestoneModel, FrontEndDbContext context)
        {
            _milestoneModel = milestoneModel;
            DutyCount = context.Duties.Count(d=>d.Milestones.Any(m=>m.Id==_milestoneModel.Id));
            EventCount = context.Events.Count(d => d.Milestones.Any(m => m.Id == _milestoneModel.Id));
        }
        public long Id => _milestoneModel.Id;
        public string Milestone => _milestoneModel.Subcategory;
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _milestoneModel;
        }
    }
}
