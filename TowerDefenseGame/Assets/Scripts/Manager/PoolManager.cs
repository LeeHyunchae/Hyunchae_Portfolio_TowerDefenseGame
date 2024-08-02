using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<Type, IPool> poolDict = new Dictionary<Type, IPool>();

    ~PoolManager()
    {
        poolDict.Clear();
        poolDict = null;
    }

    public ObjectPool<T> GetObjectPool<T>() where T : IPoolable, new()
    {
        poolDict.TryGetValue(typeof(T), out var pool);

        if(pool == null)
        {
            pool = CreateObjectPool<T>();
        }

        return (ObjectPool<T>)pool;
    }

    private ObjectPool<T> CreateObjectPool<T>() where T : IPoolable, new()
    {
        ObjectPool<T> pool = new ObjectPool<T>();
        Type type = typeof(T);

        poolDict.Add(type, pool);

        return pool;
    }

    public void RemoveObjectPool<T>() where T : IPoolable, new()
    {
        Type type = typeof(T);
        if(poolDict.TryGetValue(type, out var pool))
        {
            poolDict.Remove(type);
        }
        else
        {
            Debug.Log("Not Found Pool");
        }
    }

}
