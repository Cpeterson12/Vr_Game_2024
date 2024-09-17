using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "InstancerData", menuName = "Data/ManagerData/InstancerData")]
public class InstancerData : ScriptableObject
{
    [SerializeField] private PrefabData prefabData;
    public Vector3Data prefabOffset;
    
    public GameObject prefab => prefabData.prefab;
    
    public void SetPrefabData(PrefabData data)
    {
        prefabData = data;
    }
    
    [System.Serializable]
    public class InstanceData
    {
        public TransformData targetPosition;
        public Vector3Data instanceOffset;
        public bool excludePrefabOffset;
    }
    
    public List<InstanceData> instances = new();
}