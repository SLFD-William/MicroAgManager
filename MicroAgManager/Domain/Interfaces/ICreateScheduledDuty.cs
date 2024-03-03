using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICreateScheduledDuty
    {
        public Guid CreatedBy { get ; set ; }
        public ScheduledDutyModel ScheduledDuty { get; set; }
    }
}
