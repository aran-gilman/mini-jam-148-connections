using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnEntry
{
    SpawnEntry()
    {
        Instance = this;
    }

    public static SpawnEntry Instance;

    [SerializeField] public GameObject Enemy;
    [SerializeField] public int Cost;
    [SerializeField] public int Threshold;
}
