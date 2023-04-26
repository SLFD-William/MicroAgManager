namespace Domain.ValueObjects
{
    public class NotificationMessage : ValueObject
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public NotificationMessage() { }

        public NotificationMessage(string from, string to, string subject, string body)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return From;
            yield return To;
            yield return Subject;
            yield return Body;
        }
        
    }
}
