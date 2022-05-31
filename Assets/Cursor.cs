using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public float Spd = 0.2f;
    public float RetSpd = 3f;
    public bool AutoReturn = true;
    public bool DisableSideLimits;
    [HideInInspector]
    public new RectTransform transform;

    [SerializeField] private GearBoxGenerator _gearBox;
    
    private int CurrentGear = 0;
    private int _nextGear;
    private bool _moving = false;
    
    private WindowsControllerEmu _controller;

    private void Awake()
    {
        transform = (RectTransform)base.transform;
#if UNITY_EDITOR
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    private IEnumerator Start()
    {
        _controller = gameObject.AddComponent<WindowsControllerEmu>();
        gameObject.AddComponent<MouseHook>().onMouseMove += Move;
        
        yield return new WaitForSeconds(2f);
        transform.anchoredPosition = Vector2.zero;
    }
    
    private void Update()
    {
        if(_moving)
        {
            if(_nextGear != CurrentGear)
                ChangeGear(_nextGear);
            return;
        }
        if(!AutoReturn)
            return;
        transform.position = Vector2.Lerp(transform.position, _gearBox.Gears[CurrentGear].position, Time.deltaTime * RetSpd);
    }

    private bool CheckBounds(Vector2? pos = null)
    {
        var worldPoint = pos ?? transform.WorldRect().center;
        // Log(worldPoint.ToString());
        for (var i = 0; i < _gearBox.Gears.Length; i++)
        {
            var gear = _gearBox.Gears[i];
            if (gear.WorldRect().Contains(worldPoint))
            {
                _nextGear = i;
                return true;
            }
        }

        return false;
    }
    
    // public void Move(InputAction.CallbackContext cxt)
    // {
    //     var readValue = cxt.action.ReadValue<Vector2>();
    //     // Move(readValue);
    // }

    private void Move(Vector2 delta)
    {
        Log(delta.ToString());
        _moving = delta.sqrMagnitude > 0.5f;
        if (!_moving)
            return;

        var newPos = transform.anchoredPosition + (delta * Spd);

        var isInBounds = (CheckBounds(transform.WorldRect().center + delta * Spd));

        if (DisableSideLimits)
        {
            newPos.y = Mathf.Clamp(newPos.y, -100, 100);
            newPos.x = Mathf.Clamp(newPos.x, -180, 180);
        }
        else if (!isInBounds)
            return;

        transform.anchoredPosition = newPos;
    }

    private void ChangeGear(int i)
    {
        Log($"Gear changed from {CurrentGear} to {i}");
        
        if (CurrentGear != 0)
            _controller.SetButton(CurrentGear, false);
        
        CurrentGear = i;
        
        if(CurrentGear != 0)
            _controller.SetButton(CurrentGear, true);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Log(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
    
}

public static class RectTransformExtensions
{
    public static bool Overlaps(this RectTransform a, RectTransform b) {
        return a.WorldRect().Overlaps(b.WorldRect());
    }
    public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse) {
        return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
    }

    public static Rect WorldRect(this RectTransform rectTransform) {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
    }
}