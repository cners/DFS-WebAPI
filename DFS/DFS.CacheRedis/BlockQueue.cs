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

        /// <summary>
        /// 关闭
        /// </summary>
        public void Shutdown()
        {
            this._IsShutdown = true;
            this._dequeue_wait.Set();
        }

        /// <summary>
        /// 队列长度
        /// </summary>
        public int Count { get { return _inner_queue.Count; } }

        /// <summary>
        /// 初始化阻塞队列
        /// </summary>
        /// <param name="capacity">队列容量</param>
        public BlockQueue(int capacity = -1)
        {
            this._inner_queue = (capacity == -1 ? new Queue<T>() : new Queue<T>(capacity));
            this._dequeue_wait = new ManualResetEvent(false);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="item"></param>
        public void EnQueue(T item)
        {
            if (this._IsShutdown == true) throw new InvalidOperationException("服务器未开启.[EnQueue]");
            lock (this._inner_queue)
            {
                this._inner_queue.Enqueue(item);
                this._dequeue_wait.Set();
            }
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <param name="waitTime">等待时长，单位：毫秒</param>
        /// <returns></returns>
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
                        if (this._IsShutdown == true) throw new InvalidOperationException("服务器未开启.[DeQueue]");
                        else _queueEmpty = true;
                    }
                }
                if (item != null) return item;
                if (_queueEmpty) this._dequeue_wait.WaitOne(waitTime);
            }
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            this._inner_queue.Clear();
        }
    }
}
