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

    void Start()
    {
        if (baseModelPrefabs.Count > 0)
        {
            _currentBase = Instantiate(
                baseModelPrefabs[0],
                target).transform;
        }

        if (headModelPrefabs.Count > 0)
        {
            _currentHead = LocalInstanciate(_currentBase, headModelPrefabs[0], "Head").transform;
        }
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
                ret = Instantiate(headModelPrefabs[0], _currentChild.transform);
            }
        }
        return ret;
    }


}
