using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstancer : MonoBehaviour, INeedButton
{
    [SerializeField] private InstancerData instancerData;
    [SerializeField] private string groupName;
    [SerializeField] private bool instantiateOnStart;
    
    private GameObject _groupObject;
    private WaitForSeconds _wait = new (1);
    
    public void SetInstancerData(InstancerData data) => instancerData = data;
    
    private void Start()
    {
        if (instancerData == null) 
        { 
            Debug.LogError("InstancerData is missing.");
            return;
        }
        if (instantiateOnStart) { StartCoroutine(StartAfterDelay()); }
    }

    private IEnumerator StartAfterDelay()
    {
        yield return _wait;
        InstantiateObjects();
    }
    
    public void InstantiateObjects()
    {
        _groupObject = new GameObject();
        _groupObject.transform.SetParent(transform);
        _groupObject.transform.localPosition = Vector3.zero;
        _groupObject.name = string.IsNullOrEmpty(groupName) ? $"{name} - Instances" : groupName;
        
        foreach (var instanceData in instancerData.instances)
        {
            var instanceOffset = Vector3.zero;
            if (instanceData.instanceOffset != null) {instanceOffset = instanceData.instanceOffset;}

            var finalOffset = !instanceData.excludePrefabOffset && instancerData.prefabOffset != null ? instanceOffset + instancerData.prefabOffset.value : instanceOffset;
            InstantiateObject(instanceData.targetPosition, finalOffset);
        }
    }
    
    private void InstantiateObject(TransformData location, Vector3 offset)
    {
        var newInstance = Instantiate(instancerData.prefab, location.position, location.rotation);
        newInstance.transform.localPosition += location.rotation * offset;
        newInstance.transform.SetParent(_groupObject.transform);
    }

#if UNITY_EDITOR
    public List<(System.Action, string)> GetButtonActions()
    {
        return new List<(System.Action, string)> {(InstantiateObjects, "Instantiate Objects")};
    }
#endif
}