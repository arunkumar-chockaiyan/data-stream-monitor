using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MonitorService.Collections
{

    public class IndexedPQ<TElement, TKey, TPriority> 
        where TElement : notnull
        where TKey : notnull
        where TPriority : IComparable
    {
        public delegate TKey GetKey(TElement element);
        public delegate TPriority GetPriority(TElement element);

        private readonly PriorityQueue<TElement, TPriority> priorityQueue;
        private readonly Dictionary<TKey, TElement> elementByKey;
        private readonly GetKey getKey;
        private readonly GetPriority getPriority;
        private readonly object locks;

        public IndexedPQ(GetKey getKey, GetPriority getPriority)
        {
            this.locks = new object();
            this.getKey = getKey;
            this.getPriority = getPriority;
            this.priorityQueue = new PriorityQueue<TElement, TPriority>(Comparer<TPriority>.Create((a,b) => b.CompareTo(a)));
            this.elementByKey = new Dictionary<TKey, TElement>();
        }

        public void Enqueue(TElement element)
        {
            lock (this.locks)
            {
                this.priorityQueue.Enqueue(element, getPriority(element));
                this.elementByKey[getKey(element)] = element;
            }
        }

        public TElement Dequeue()
        {
            lock (this.locks) {
                TElement element = this.priorityQueue.Dequeue(); 
                this.elementByKey.Remove(getKey(element));
                return element;
            }
        }
        
        public TElement? getByKey(TKey key)
        {
            return this.elementByKey.GetValueOrDefault(key);
        }
        
        public TElement? peekTopElement()
        {
            return this.priorityQueue.Peek();
        }

    }
}
