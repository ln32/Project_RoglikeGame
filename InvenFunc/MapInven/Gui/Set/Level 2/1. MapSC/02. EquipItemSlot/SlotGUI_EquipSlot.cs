using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotGUI_EquipSlot : SlotGUI_InvenSlot
{
    [SerializeField] internal ItemData_0 itemType = 0;
    [SerializeField] internal ItemData_1 detailType = 0;
    internal int charIndex = 0;


    public bool IsInteractable_byGetRBD(RDM_MapSC _inven, IResponedByDrop target)
    {
        return _inven.Check_Upscale(this,target);
    }

    new public void InteractDDO_byGetRBD(IResponedByDrop target)
    {
        if (true)
        {
            if ((target as SlotGUI_EquipSlot == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, target as SlotGUI_EquipSlot);
                return;
            }

            else if ((target as SlotGUI_InvenSlot == true))
            {
                GetDDO_Manager().InteractFuncByRBD(this, target as SlotGUI_InvenSlot);
                return;
            }
        }

        GetDDO_Manager().InteractFuncByRBD(this);
        return;
    }
}

internal enum ItemData_0
{
    Food, Equip, Skill
}
internal enum ItemData_1
{
    Power, Sustain, Util
}