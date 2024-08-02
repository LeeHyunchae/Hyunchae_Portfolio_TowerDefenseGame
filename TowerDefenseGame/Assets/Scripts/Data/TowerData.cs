using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETowerType
{
    ONESHOOT,
    BOOM,
    DEBUFF,
    PIERCE,
    LAUNCHER,
    CONTINUOUS,
    MULTI
}

[System.Serializable]
public struct TowerData
{
    public int uid;
    public ETowerType type;
    public int nextuid;
    public int grade;
    public float cooltime;
    public float atkInterval;
    public float atkrange;
}
