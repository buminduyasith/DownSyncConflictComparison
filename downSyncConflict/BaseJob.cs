namespace downSyncConflict
{
    public class BaseJob
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public int Budget { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Name} - {Status} - {Description} - {Budget}";
        }
    }
}
