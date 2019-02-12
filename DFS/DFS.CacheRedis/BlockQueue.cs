using System;
using System.Collections.Generic;
using System.Threading;

namespace DFS.CacheRedis
{
    /// <summary>
    /// 阻塞队列
    /// </summary>
    public class BlockQueue<T>
    {
        private Queue<T> _inner_queue = null;

        private ManualResetEvent _dequeue_wait = null;

        private bool _IsShutdown = false;
        public void Shutdown()
        {
            this._IsShutdown = true;
            this._dequeue_wait.Set();
        }
        public int Count { get { return _inner_queue.Count; } }

        public BlockQueue(int capacity = -1)
        {
            this._inner_queue = (capacity == -1 ? new Queue<T>() : new Queue<T>(capacity));
            this._dequeue_wait = new ManualResetEvent(false);
        }

        public void EnQueue(T item)
        {
            if (this._IsShutdown == true) throw new InvalidOperationException("服务器未开启.[EnQueue]");
            lock (this._inner_queue)
            {
                this._inner_queue.Enqueue(item);
                this._dequeue_wait.Set();
            }
        }

        public T DeQueue(int waitTime)
        {
            bool _queueEmpty = false;
            T item = default(T);
            while (true)
            {
                lock (this._inner_queue)
                {
                    if (this._inner_queue.Count > 0)
                    {
                        item = this._inner_queue.Dequeue();
                        this._dequeue_wait.Reset();
                    }
                    else
                    {
                        if (this._IsShutdown == true) throw new InvalidOperationException("服务器未开启.[EnQueue]");
                        else _queueEmpty = true;
                    }
                }
                if (item != null) return item;
                if (_queueEmpty) this._dequeue_wait.WaitOne(waitTime);
            }
        }

        public void Clear()
        {
            this._inner_queue.Clear();
        }
    }
}
