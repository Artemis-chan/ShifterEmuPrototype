using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using UnityEngine;

public class WindowsControllerEmu : MonoBehaviour
{
    private readonly ViGEmClient _viGEm = new ViGEmClient();
    private IXbox360Controller _cont;

    private void Awake()
    {
        _cont = _viGEm.CreateXbox360Controller();
        _cont.Connect();
    }
    
    public void SetButton(int id, bool value)
    {
        _cont.SetButtonState(id, value);
    }

    private void OnDestroy()
    {
        _cont.Disconnect();
        _viGEm.Dispose();
    }
    
}
