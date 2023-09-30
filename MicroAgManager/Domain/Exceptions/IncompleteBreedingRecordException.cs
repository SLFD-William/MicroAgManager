namespace Domain.Exceptions
{
    public class IncompleteBreedingRecordException : Exception
    {
        public IncompleteBreedingRecordException(string name, object key, string message)
            : base($"Incomplete Breeding Record for \"{name}\" ({key}) exists. {message}")
        {
        }
    }
}
