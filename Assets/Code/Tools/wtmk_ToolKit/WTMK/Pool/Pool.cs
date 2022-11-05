using System;
using System.Collections;
using System.Collections.Generic;

public class Pool : IPool
{
    public int QueueCount { get { return _Queue.Count; } }
    public IPoolable GetPoolable() 
    {
        IPoolable pool = _Queue.Dequeue();
        pool.OnReturnRequest += PlaceInQueue;
        return pool;
    }

    public void PlaceInQueue(IPoolable t)
    {
        t.OnReturnRequest -= PlaceInQueue;
        _Queue.Enqueue(t);
        t.SetActive(false);
    }

    private IPoolable[] _Pool;
    private Queue<IPoolable> _Queue;

    public Pool(IPoolable[] pool)
    {
        _Queue = new Queue<IPoolable>();
        _Pool = pool;

        for (int i = 0; i < _Pool.Length; i++)
        {
            if(_Pool[i] != null)
            {
                _Pool[i].OnReturnRequest += PlaceInQueue;
                _Queue.Enqueue(_Pool[i]);
            }
        }
    }

    ~Pool()
    {
        _Queue.Clear();
        for (int i = 0; i < _Pool.Length; i++)
        {
            _Pool[i].OnReturnRequest -= PlaceInQueue;
        }
    }
}
