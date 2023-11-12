using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.BusinessLogic.Unit
{
   public class UnitDto
    {
        public UnitDto(long count, ICollection<UnitModel> models)
        {
            Count = count;
            Models = models;
        }
        public long Count { get; set; }
        public ICollection<UnitModel> Models { get; set; }
    }
}
