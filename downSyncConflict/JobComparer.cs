using System.Reflection;

namespace downSyncConflict
{
    public class JobComparer<T>
    {
        public List<ChangedProperty> Compare(T oldJob, T newJob)
        {
            List<ChangedProperty> changeProps = new List<ChangedProperty>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                var oldValue = property.GetValue(oldJob)?.ToString();
                var newValue = property.GetValue(newJob)?.ToString();

                if (oldValue != newValue)
                {
                    changeProps.Add(new ChangedProperty
                    {
                        FieldName = property.Name,
                        OldValue = oldValue,
                        NewValue = newValue
                    });
                }
            }

            return changeProps;
        }
    }
}
