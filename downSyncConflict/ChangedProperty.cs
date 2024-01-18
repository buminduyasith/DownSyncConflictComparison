namespace downSyncConflict
{
    public class ChangedProperty
    {
        public string Id { get;} = Guid.NewGuid().ToString();

        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
