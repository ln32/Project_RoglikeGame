using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class _RBD_EquipSlot_MAP
{
    internal static bool Check_Upscale(this iRoot_DDO_Manager _inven, SlotGUI_EquipSlot _src, IResponedByDrop _dst)
    {
        if ((_dst as SlotGUI_EquipSlot == true))
        {
            SlotGUI_EquipSlot _dst2 = _dst as SlotGUI_EquipSlot;

            if (_dst2.CompareItem_withStat(_src._itemGUI._myData) == false)
                return false;


        }

        if ((_dst as SlotGUI_InvenSlot == true))
        {
            SlotGUI_InvenSlot _dst2 = _dst as SlotGUI_InvenSlot;
            if (_dst2._itemGUI != null)
            {
                if (_src.CompareItem_withStat(_dst2._itemGUI._myData) == false)
                    return false;
            }
        }

        return true;
    }


    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_EquipSlot _src)
    {
        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_EquipSlot _src, SlotGUI_InvenSlot _dst)
    {
        if ((_inven as RDM_ShopSC == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst);
            return;
        }

        if ((_inven as RDM_MapSC == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst);
        }
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_EquipSlot _src, SlotGUI_EquipSlot _dst)
    {
        if ((_inven as RDM_ShopSC == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst);
            return;
        }

        if ((_inven as RDM_MapSC == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst);
            return;
        }
    }
}
