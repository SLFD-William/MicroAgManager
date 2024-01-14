namespace MicroAgManager.Components.Shared
{
    class ComponentMetadata
    {
        public string? Name { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = [];
    }
}
