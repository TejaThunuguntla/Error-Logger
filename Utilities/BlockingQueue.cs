using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utilities
{
    public class BlockingQueue<T>
    {
        private readonly Queue<T> queue; //= new Queue<T>();
                                         // private readonly int maxSize;
        public BlockingQueue()
        {
            queue = new Queue<T>();
        }

        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    // wake up any blocked dequeue
                    Monitor.PulseAll(queue);
                }
            }
        }
        public T Dequeue()
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(queue);
                }
                T item = queue.Dequeue();
                if (queue.Count > 0)
                {
                    // wake up any blocked enqueue
                    Monitor.PulseAll(queue);
                }
                return item;
            }
        }

        public int Size()
        {
            lock (queue)
            {
                return queue.Count();
            }
        }

        public void Clear()
        {
            lock (queue)
            {
                queue.Clear();
            }
        }
    }
}
