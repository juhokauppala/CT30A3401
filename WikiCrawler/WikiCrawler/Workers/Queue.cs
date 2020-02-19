using System;
using System.Collections.Concurrent;
using System.Text;

namespace WikiCrawler.Workers
{
    class Queue<T> where T : class
    {
        public bool IsEmpty { get => queue.IsEmpty; }
        public int Count { get => queue.Count; }

        private ConcurrentQueue<T> queue;

        public Queue()
        {
            queue = new ConcurrentQueue<T>();
        }

        public T GetNext()
        {
            T item;
            bool success = queue.TryDequeue(out item);
            return success ? item : null;
        }

        public void AddNew(T item)
        {
            queue.Enqueue(item);
        }
    }
}
