using Domain.Models;
using Domain.ValueObjects;
using FrontEnd.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Milestone
{
    public class MilestoneSummary : ValueObject
    {
        private MilestoneModel _milestoneModel;
        public int DutyCount { get; private set; }
        public int EventCount { get; private set; }
        public MilestoneSummary(MilestoneModel milestoneModel, FrontEndDbContext context)
        {
            _milestoneModel = context.Milestones.Include(m=>m.Duties).Include(m => m.Events).First(m=>m.Id==milestoneModel.Id);
            DutyCount = _milestoneModel.Duties?.Count() ?? 0;
            EventCount = _milestoneModel.Events?.Count() ?? 0;
        }
        public long Id => _milestoneModel.Id;
        public string Milestone => _milestoneModel.Name;
        public string Description => _milestoneModel.Description;
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _milestoneModel;
        }
    }
}
