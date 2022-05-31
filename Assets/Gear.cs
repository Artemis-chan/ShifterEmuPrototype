using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public int GearNum;
    public TextMeshProUGUI TextMeshProUGUI;
    
    public void SetGearNum(int gearNum, string text = null)
    {
        GearNum = gearNum;
        TextMeshProUGUI.text = text ?? GearNum.ToString();
    }
}
