using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnableData 
{
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnablesSO", order = 1)]
public class SpawnablesSO : ScriptableObject
{
    public List<SpawnableData> enemies;
    public LayerMask spawnOnLayerMask;
}
