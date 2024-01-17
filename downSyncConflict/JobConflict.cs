using System.Text.Json;

namespace downSyncConflict
{
    public class JobConflict : BaseJob
    {
        public List<ChangedProperty> ChangedProperties { get; set; } = new();

        public override string ToString()
        {
            return $"{Id} - | {JsonSerializer.Serialize(ChangedProperties)}";
        }
    }
}
