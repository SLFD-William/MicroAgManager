using Domain.Models;

namespace BackEnd.BusinessLogic.Event
{
    public class EventDto
    {
        public EventDto(long count, ICollection<EventModel> models)
        {
            Count = count;
            Models = models;
        }

        public long Count { get; set; }
        public ICollection<EventModel> Models { get; set; }
    }
}
