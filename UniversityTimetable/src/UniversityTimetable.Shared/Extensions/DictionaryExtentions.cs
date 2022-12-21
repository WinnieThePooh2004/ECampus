namespace UniversityTimetable.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> addTo, IEnumerable<KeyValuePair<TKey, TValue>> addFrom)
        {
            foreach (var item in addFrom)
            {
                addTo.TryAdd(item.Key, item.Value);
            }
        }
    }
}
