using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerAttackBehaviour
{
    protected TowerData towerData;

    public virtual void Update(){}

    public void SetTowerData(TowerData _towerData)
    {
        towerData = _towerData;
    }

    public virtual void Fire() { }
}

public class SingleFireBehaviour : BaseTowerAttackBehaviour
{
}

public class BoomBehaviour : BaseTowerAttackBehaviour
{
}

public class DebuffBehaviour : BaseTowerAttackBehaviour
{
}

public class PierceBehaviour : BaseTowerAttackBehaviour
{
}

public class LauncherBehaviour : BaseTowerAttackBehaviour
{
}

public class AutoFireBehaviour : BaseTowerAttackBehaviour
{
}

public class MultiBehaviour : BaseTowerAttackBehaviour
{
}