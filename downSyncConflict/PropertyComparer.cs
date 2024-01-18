using System.Reflection;

namespace downSyncConflict
{
    public class PropertyComparer<TNew, TOld>
    {
        private readonly List<PropertyMapping> propertyMappings;

        public PropertyComparer(List<PropertyMapping> propertyMappings = null)
        {
            this.propertyMappings = propertyMappings ?? new List<PropertyMapping>();
        }

        public List<ChangedProperty> Compare(TNew newJob, TOld oldJob)
        {
            List<ChangedProperty> changeProps = new List<ChangedProperty>();

            foreach (PropertyInfo newProperty in typeof(TNew).GetProperties())
            {
                string fieldName = GetFieldName(newProperty.Name);

                // PropertyInfo oldProperty2 = typeof(TOld).GetProperty(GetMappedPropertyName(newProperty.Name));

                string mappedPropertyName = GetMappedPropertyName(newProperty.Name);

                PropertyInfo? oldProperty = mappedPropertyName != null ? typeof(TOld).GetProperty(mappedPropertyName): null;

                if (oldProperty == null ){
                    continue;
                }

                var oldValue = oldProperty.GetValue(oldJob)?.ToString();
                var newValue = newProperty.GetValue(newJob)?.ToString();

                if (oldValue == newValue || string.IsNullOrWhiteSpace(newValue))
                {
                    continue;
                }

                changeProps.Add(new ChangedProperty
                {
                    FieldName = fieldName,
                    OldValue = oldValue != null ? oldValue : string.Empty,
                    NewValue = newValue
                });

               /* if (oldProperty != null)
                {
                    var oldValue = oldProperty.GetValue(oldJob)?.ToString();
                    var newValue = newProperty.GetValue(newJob)?.ToString();

                    if (oldValue != newValue)
                    {
                        changeProps.Add(new ChangedProperty
                        {
                            FieldName = fieldName,
                            OldValue = oldValue,
                            NewValue = newValue
                        });
                    }
                }*/
            }

            return changeProps;
        }

        private string GetMappedPropertyName(string propertyName)
        {
            var mapping = propertyMappings.FirstOrDefault(m => m.ProviderPropertyName == propertyName);
            return mapping?.WXFPropertyName ?? propertyName;
        }

        private string GetFieldName(string propertyName)
        {
            var mapping = propertyMappings.FirstOrDefault(m => m.ProviderPropertyName == propertyName);
            return mapping?.CustomFieldName ?? propertyName;
        }
    }

}
