namespace Domain.Constants
{
    public static class RecipientTypeConstants
    {
        public static string None { get; private set; } = "None";
        public static string LivestockAnimal { get; private set; } = nameof(Entity.LivestockAnimal);
        public static string LivestockBreed { get; private set; } = nameof(Entity.LivestockBreed);

    }
}
