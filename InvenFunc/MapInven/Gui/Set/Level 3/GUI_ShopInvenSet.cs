using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_ShopInvenSet : MonoBehaviour
{
    public List<int> myAddr = new();
    public List<SlotGUI_InvenSlot> MySlotList = new List<SlotGUI_InvenSlot>();
    public int index = -1;
}