namespace Domain.Interfaces
{
    public interface IHasRecipient
    {
        public long RecipientTypeId { get; set; }
        public string RecipientType { get; set; }
        public long RecipientId { get; set; }
        string RecipientTypeItem { get; set; }
        string RecipientItem { get; set; }

    }
}
