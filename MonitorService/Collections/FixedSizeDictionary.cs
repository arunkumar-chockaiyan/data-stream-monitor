using Microsoft.AspNetCore.Routing.Template;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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
            if(this.dictionary.Count < this.maxSize)
            {
                return this.dictionary.TryAdd(key, valueFactory(key));
            }
            return false;
        }


        public bool TryAdd(TValue value)
        {
            if (this.dictionary.Count < this.maxSize)
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
