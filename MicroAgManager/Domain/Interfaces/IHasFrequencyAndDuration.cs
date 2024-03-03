using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IHasFrequencyAndDuration
    {
        decimal Duration { get; set; }
        long? DurationUnitId { get; set; }
        decimal Frequency { get; set; }
        long? FrequencyUnitId { get; set; }
    }
}
