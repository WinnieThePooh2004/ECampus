using FluentAssertions.Collections;

namespace ECampus.Tests.Shared.Extensions;

public static class GenericDictionaryAssertionsExtensions
{
    public static GenericDictionaryAssertions<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue>
        ContainsKeysWithValues<TKey, TValue>
        (this GenericDictionaryAssertions<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue> actual,
            IEnumerable<KeyValuePair<TKey, TValue>> expected)
    {
        foreach (var item in expected)
        {
            actual.ContainEquivalentOf(item,
                opt => opt.IncludingFields().ComparingByMembers<KeyValuePair<TKey, TValue>>());
        }

        return actual;
    }
}