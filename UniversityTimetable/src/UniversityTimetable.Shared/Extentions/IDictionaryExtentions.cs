namespace UniversityTimetable.Shared.Extentions
{
    public static class IDictionaryExtentions
    {
        public static void AddRange<TKey, TVavue>(this IDictionary<TKey, TVavue> addTo, IEnumerable<KeyValuePair<TKey, TVavue>> addFrom)
        {
            foreach (var item in addFrom)
            {
                addTo.TryAdd(item.Key, item.Value);
            }
        }
    }
}
