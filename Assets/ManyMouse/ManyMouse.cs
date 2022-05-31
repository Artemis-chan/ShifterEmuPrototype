using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class ManyMouse
{
    [DllImport("ManyMouse", EntryPoint = "ManyMouse_Init")]
    public static extern int Init();
    
    [DllImport("ManyMouse", EntryPoint = "ManyMouse_Quit")]
    public static extern int Quit();
    
    [DllImport("ManyMouse", EntryPoint = "ManyMouse_PollEvent")]
    public static extern int PollEvent(ref ManyMouseEvent evt);
    
    [DllImport("ManyMouse", EntryPoint = "ManyMouse_GetDeviceCount")]
    public static extern int GetDeviceCount();
    
    [DllImport("ManyMouse", EntryPoint = "ManyMouse_GetDeviceName")]
    public static extern int GetDeviceName(int device, StringBuilder name, int maxlen);

    public static Vector2Int GetDeltaMovement()
    {
        var v2 = Vector2Int.zero;
        var evt = new ManyMouseEvent();

        while (PollEvent(ref evt) != 0)
        {
            if (evt.type != ManyMouseEventType.MANYMOUSE_EVENT_RELMOTION)
                continue;
            
            switch (evt.item)
            {
                case 0:
                    v2.x = evt.value;
                    break;
                case 1:
                    v2.y = evt.value;
                    break;
            }
        }
        
        return v2;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct ManyMouseEvent
{
    public ManyMouseEventType type;
    public uint device;
    public uint item;
    public int value;
    public int minval;
    public int maxval;
}

public enum ManyMouseEventType
{
    MANYMOUSE_EVENT_ABSMOTION = 0,
    MANYMOUSE_EVENT_RELMOTION,
    MANYMOUSE_EVENT_BUTTON,
    MANYMOUSE_EVENT_SCROLL,
    MANYMOUSE_EVENT_DISCONNECT,
    MANYMOUSE_EVENT_MAX
}
