using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
public class MouseHook : MonoBehaviour
{
    public event Action<Vector2> onMouseMove;

    private void Awake()
    {
        ManyMouse.Init();
    }

    private void Update()
    {
        if (onMouseMove == null) return;
        var d = ManyMouse.GetDeltaMovement();
        d.y = -d.y;
        onMouseMove.Invoke(d);
    }

    private void OnDestroy()
    {
        ManyMouse.Quit();
    }
}