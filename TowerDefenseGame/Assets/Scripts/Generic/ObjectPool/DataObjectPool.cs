using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObjectPool<T> : IPool where T : IPoolable, new()
{
    private int increaseSize;
    private Queue<T> objectQueue;

    public DataObjectPool(int _poolSize,int _increaseSize = 4)
    {
        increaseSize = _increaseSize;
        objectQueue = new Queue<T>(_poolSize);
    }

    public T GetObject()
    {
        T obj;

        Debug.Log("PoolCount " + objectQueue.Count);

        if (objectQueue.TryDequeue(out obj))
        {
            return obj;
        }
        else
        {
            IncreasePool();
            return objectQueue.Dequeue();
        }
    }

    public void RetrunObject(T obj)
    {
        objectQueue.Enqueue(obj);

        Debug.Log("PoolCount " + objectQueue.Count);

    }

    private void IncreasePool()
    {
        for(int i = 0; i<increaseSize;i++)
        {
            objectQueue.Enqueue(new T());
        }
    }
}
