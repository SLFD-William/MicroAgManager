namespace Domain.ValueObjects
{
    public class ModifiedEntity : ValueObject
    {
        public ModifiedEntity(string id, string entityName, string modification, Guid modifiedBy)
        {
            Id = id;
            EntityName = entityName;
            Modification = modification;
            ModifiedBy = modifiedBy;
        }

        public string Id { get; set; }
        public string EntityName { get; set; }
        public string Modification { get; set; }
        public Guid ModifiedBy { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return EntityName;
            yield return ModifiedBy;
            yield return Modification;
        }
    }
}
