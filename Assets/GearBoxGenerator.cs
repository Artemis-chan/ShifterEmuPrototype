using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBoxGenerator : MonoBehaviour
{
    public int GearsCount = 5;
    public RectTransform[] Gears;
    
    [SerializeField] private Gear _gearPrefab;
    [SerializeField] private RectTransform _gearTop;
    [SerializeField] private RectTransform _gearBottom;
    [SerializeField] private Gear _gearNeutral;
    
    void Start()
    {
        Generate();
    }

    [ContextMenu("Generate")]
    void Generate()
    {
        DestroyChildren(_gearTop);
        DestroyChildren(_gearBottom);
        Gears = new RectTransform[GearsCount + 2];
        for (int i = 1; i <= GearsCount; i++)
        {
            var g = Instantiate(_gearPrefab, i % 2 == 0 ? _gearBottom : _gearTop);
            g.SetGearNum(i);
            Gears[i] = (RectTransform)g.transform;
        }
        _gearNeutral.SetGearNum(0, "N");
        Gears[0] = (RectTransform)_gearNeutral.transform;
        var r = Instantiate(_gearPrefab, _gearBottom);
        r.SetGearNum(GearsCount + 1, "R");
        Gears[GearsCount + 1] = (RectTransform)r.transform;
        
        if(GearsCount % 2 == 0)
        {
            Instantiate(new GameObject("padding", typeof(RectTransform)), _gearTop);
        }
    }

    private void DestroyChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; --i)
        {
            var child = parent.GetChild(i).gameObject;
#if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlaying)
                DestroyImmediate(child);
            else
#endif
            Destroy(child);
        }
    }
}
