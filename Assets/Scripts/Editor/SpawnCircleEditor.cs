using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(SpawnablesManager))]
public class SpawnCircleEditor : Editor
{
    private void OnSceneGUI()
    {
        SpawnablesManager spawnable = target as SpawnablesManager;

        Handles.color = Color.yellow;
        Handles.DrawWireArc(spawnable.transform.position, Vector3.forward, Vector3.up, 360, spawnable.spawnRadius);
    }
}
