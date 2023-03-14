using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private List<GameObject> baseModelPrefabs = null;
    [SerializeField] private List<GameObject> headModelPrefabs = null;
    [SerializeField] private List<GameObject> wingModelPrefabs = null;

    private Transform _currentBase = null;
    private Transform _currentHead = null;
    private Transform _currentWings = null;

    void Start()
    {
        MakeModel(0,0,0);
    }

    public void MakeModel(int baseIndex, int headIndex, int wingIndex)
    {
        ClearModel();
        if (baseIndex < baseModelPrefabs.Count && baseModelPrefabs.Count > 0)
        {
            _currentBase = Instantiate(baseModelPrefabs[0], target).transform;
        }

        if (_currentBase && headIndex < headModelPrefabs.Count && headModelPrefabs.Count > 0)
        {
            _currentHead = LocalInstanciate(_currentBase, headModelPrefabs[0], "Head").transform;
        }

        if (_currentBase && wingIndex < wingModelPrefabs.Count && wingModelPrefabs.Count > 0)
        {
            _currentWings = LocalInstanciate(_currentBase, wingModelPrefabs[0], "Wings").transform;
        }
    }

    private void ClearModel()
    {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        _currentBase = null;
        _currentHead = null;
        _currentWings = null;
    }

    void Update()
    {
    }

    private GameObject LocalInstanciate(Transform parentBase, GameObject prefabToInstanciate, string childKeyWord)
    {
        GameObject ret = null;
        for (int i = 0; i < parentBase.transform.childCount; i++)
        {
            Transform _currentChild = parentBase.GetChild(i);
            if (_currentChild.gameObject.name.Contains(childKeyWord))
            {
                ret = Instantiate(prefabToInstanciate, _currentChild.transform);
                break;
            }
        }
        return ret;
    }


}
