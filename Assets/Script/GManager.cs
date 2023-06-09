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
    [SerializeField] private List<GameObject> pawsModelPrefabs = null;
    [SerializeField] private List<GameObject> tailModelPrefabs = null;

    private Transform _currentBase = null;
    private int _currentBaseIndex = 0;
    private int _currentHeadIndex = 0;
    private int _currentWingsIndex = 0;
    private int _currentFPawsIndex = 0;
    private int _currentBPawsIndex = 0;
    private int _currentTailIndex = 0;

    void Start()
    {
        float y = MakeModel(
            _currentBaseIndex,
            _currentHeadIndex,
            _currentWingsIndex,
            _currentFPawsIndex,
            _currentBPawsIndex,
            _currentTailIndex
            );
        PoseCreature(y);
    }

    private void PoseCreature(float minY)
    {
        float delta = minY <= 0f ? -minY : minY;
        target.position = new Vector3(
            0,
            delta,
            0);
    }

    private int GetNextIndex(int index, List<GameObject> prefabList)
    {
        int newindex = index + 1 < prefabList.Count ? index + 1 : 0;
        return newindex;
    }
    public void NextBase()
    {
        float y = MakeModel(
            GetNextIndex(_currentBaseIndex, baseModelPrefabs),
            _currentHeadIndex,
            _currentWingsIndex,
            _currentFPawsIndex,
            _currentBPawsIndex,
            _currentTailIndex);
        PoseCreature(y);
    }

    public void NextHead()
    {
        float y = MakeModel(
            _currentBaseIndex,
            GetNextIndex(_currentHeadIndex, headModelPrefabs),
            _currentWingsIndex,
            _currentFPawsIndex,
            _currentBPawsIndex,
            _currentTailIndex);
        PoseCreature(y);
    }

    public void NextWings()
    {
        float y = MakeModel(
            _currentBaseIndex,
            _currentHeadIndex,
            GetNextIndex(_currentWingsIndex, wingModelPrefabs),
            _currentFPawsIndex,
            _currentBPawsIndex,
            _currentTailIndex);
        PoseCreature(y);
    }

    public void NextFpaws()
    {
        float y = MakeModel(
            _currentBaseIndex,
            _currentHeadIndex,
            _currentWingsIndex,
            GetNextIndex(_currentFPawsIndex, pawsModelPrefabs),
            _currentBPawsIndex,
            _currentTailIndex);
        PoseCreature(y);
    }

    public void NextBpaws()
    {
        float y = MakeModel(
            _currentBaseIndex,
            _currentHeadIndex,
            _currentWingsIndex,
            _currentFPawsIndex,
            GetNextIndex(_currentBPawsIndex, pawsModelPrefabs),
            _currentTailIndex);
        PoseCreature(y);
    }

    public void NextTail()
    {
        float y = MakeModel(
            _currentBaseIndex,
            _currentHeadIndex,
            _currentWingsIndex,
            _currentFPawsIndex,
            _currentBPawsIndex,
            GetNextIndex(_currentTailIndex,
            tailModelPrefabs));
        PoseCreature(y);
    }

    public float MakeModel(int baseIndex, int headIndex, int wingIndex, int fpawsIndex, int bpawsIndex, int tailIndex)
    {
        ClearModel();
        List<GameObject> go = new List<GameObject>();
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        float minY = bounds.min.y;
        Renderer renderer = null;

        if (baseIndex < baseModelPrefabs.Count && baseModelPrefabs.Count > 0)
        {
            _currentBase = Instantiate(baseModelPrefabs[baseIndex], target).transform;
            _currentBaseIndex = baseIndex;
            go.Add(_currentBase.gameObject);
        }

        if (_currentBase && headIndex < headModelPrefabs.Count && headModelPrefabs.Count > 0)
        {
            go.Add(LocalInstanciate(_currentBase, headModelPrefabs[headIndex], "Head"));
            _currentHeadIndex = headIndex;
        }

        if (_currentBase && wingIndex < wingModelPrefabs.Count && wingModelPrefabs.Count > 0)
        {
            go.Add(LocalInstanciate(_currentBase, wingModelPrefabs[wingIndex], "Wings"));
            _currentWingsIndex = wingIndex;
        }

        if (_currentBase && fpawsIndex < pawsModelPrefabs.Count && pawsModelPrefabs.Count > 0)
        {
            go.Add(LocalInstanciate(_currentBase, pawsModelPrefabs[fpawsIndex], "FPaws"));
            _currentFPawsIndex = fpawsIndex;
        }

        if (_currentBase && bpawsIndex < pawsModelPrefabs.Count && pawsModelPrefabs.Count > 0)
        {
            go.Add(LocalInstanciate(_currentBase, pawsModelPrefabs[bpawsIndex], "BPaws"));
            _currentBPawsIndex = bpawsIndex;
        }

        if (_currentBase && tailIndex < tailModelPrefabs.Count && tailModelPrefabs.Count > 0)
        {
            go.Add(LocalInstanciate(_currentBase, tailModelPrefabs[tailIndex], "Tail"));
            _currentTailIndex = tailIndex;
        }
        foreach (GameObject g in go)
        {
            if (g.TryGetComponent<Renderer>(out renderer))
            {
                bounds.Encapsulate(renderer.bounds);
                minY = bounds.min.y < minY ? bounds.min.y : minY;
            }
        }
        return minY;
    }

    private void ClearModel()
    {
        foreach (Transform child in target.transform) {
            GameObject.Destroy(child.gameObject);
        }
        _currentBaseIndex = 0;
        _currentHeadIndex = 0;
        _currentWingsIndex = 0;
        _currentFPawsIndex = 0;
        _currentBPawsIndex = 0;
        _currentTailIndex = 0;
        target.position = Vector3.zero;
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
