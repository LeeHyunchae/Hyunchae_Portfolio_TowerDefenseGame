using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    private ObjectPool<MonsterController> monsterPool;

    public override void Init()
    {
        InitMonsterPool();
        base.Init();
    }

    private void InitMonsterPool()
    {
        monsterPool = PoolManager.getInstance.GetObjectPool<MonsterController>();
        monsterPool.Init("Prefabs/Monster");

    }

    public MonsterController GetMonster()
    {
        if (monsterPool == null)
        {
            InitMonsterPool();
        }

        return monsterPool.GetObject();
    }

    public void EnqueueMonster(MonsterController _monster)
    {
        monsterPool.EnqueueObject(_monster);
    }
}
