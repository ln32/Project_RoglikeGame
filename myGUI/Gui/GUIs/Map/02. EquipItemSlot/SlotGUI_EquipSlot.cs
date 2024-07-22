using System;
using UnityEngine;

public class SlotGUI_EquipSlot : SlotGUI_InvenSlot
{
    [SerializeField] internal ItemData_0 itemType = 0;
    [SerializeField] internal ItemData_1 detailType = 0;
    [SerializeField] internal int charIndex = 0;

    public bool IsInteractable_byGetRBD(RDM_MapSC _inven, IResponedByDrop target)
    {
        return _inven.Check_Upscale(this, target);
    }

    new public void InteractDDO_byGetRBD(IResponedByDrop target)
    {
        if (target is SlotGUI_EquipSlot equipSlot)
        {
            GetDDO_Manager().InteractFuncByRBD(this, equipSlot);
        }
        else if (target is SlotGUI_InvenSlot invenSlot)
        {
            GetDDO_Manager().InteractFuncByRBD(this, invenSlot);
        }
        else
        {
            GetDDO_Manager().InteractFuncByRBD(this);
        }
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
