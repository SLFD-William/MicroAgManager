using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Log //log object for Ilogger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)][Key]public long Id { get; set; }
        public string Level { get; set; }
        public string CategoryName { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }

    }
}
