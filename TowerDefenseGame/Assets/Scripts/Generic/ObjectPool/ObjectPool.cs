using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ObjectPool<T> : IPool where T : IPoolable
{
    private Queue<T> objectQueue = null;

    private GameObject originPrefab_GameObj = null;

    private bool isInitilaze = false;

    private int increaseSize;
    private Transform parentTransform;


    public ObjectPool()
    {
        objectQueue = new Queue<T>();
        objectQueue.Clear();
    }

    public void Init(string _prefabPath, int _increaseSize = 4)
    {
        var originPrefab = Resources.Load(_prefabPath);

        parentTransform = new GameObject(originPrefab.name).GetComponent<Transform>();

        originPrefab_GameObj = originPrefab as GameObject;
        increaseSize = _increaseSize;

        IncreasePool();

        isInitilaze = true;
    }

    ~ObjectPool()
    {
        OnRelease();
    }

    public void EnqueueObject(T _obj)
    {
        _obj.OnEnqueue();
        objectQueue.Enqueue(_obj);
    }

    public T GetObject()
    {
        // 오브젝트 가져오기
        T obj;

        // 풀이 비어있을 시 풀 확장
        if (!objectQueue.TryDequeue(out obj))
        {
            IncreasePool();
            obj = objectQueue.Dequeue();
        }

        obj.OnDequeue();
        return obj;
    }

    private void IncreasePool()
    {
        for(int i = 0; i<increaseSize;i++)
        {
            GameObject obj = GameObject.Instantiate(originPrefab_GameObj,parentTransform);
            T componenet = obj.GetComponent<T>();
            componenet.Init();

            EnqueueObject(componenet);
        }
    }


    public void OnRelease()
    {
        objectQueue.Clear();
    }
}
