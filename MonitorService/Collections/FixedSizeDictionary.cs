using System.Collections.Concurrent;

namespace MonitorService.Collections
{
    public class FixedSizeDictionary<TKey, TValue> 
        where TKey : notnull
    {
        public delegate TKey GetKey(TValue value);

        private readonly int maxSize;
        private readonly GetKey getKey;
        private readonly ConcurrentDictionary<TKey, TValue> dictionary;
        private readonly int DEFAULT_CONCURRENCY = 10;


        public FixedSizeDictionary(int maxSize, GetKey getKey)
        {
            ArgumentNullException.ThrowIfNull(getKey, nameof(getKey));
            if(maxSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxSize), "Value should be more than 0");
            }

            this.maxSize = maxSize;
            this.getKey = getKey;
            this.dictionary = new ConcurrentDictionary<TKey, TValue>(DEFAULT_CONCURRENCY, maxSize);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values()
        {
            return this.dictionary.AsReadOnly().Values;
        }

        public IReadOnlyDictionary<TKey, TValue> getReadOnlyDictionary()
        {
            return this.dictionary;
        }

        public bool TryAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            ArgumentNullException.ThrowIfNull(valueFactory, nameof(valueFactory));

            if (CanAddNewValue())
            {
                return this.dictionary.TryAdd(key, valueFactory(key));
            }
            return false;
        }

        private bool CanAddNewValue()
        {
            return this.dictionary.Count < this.maxSize;
        }

        public bool TryAdd(TValue value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            if (CanAddNewValue())
            {
                return this.dictionary.TryAdd(getKey(value), value);
            }
            return false;
        }

        public bool TryRemove(TKey key, out TValue value)
        {
            return this.dictionary.TryRemove(key, out value);
        }

    }
}
