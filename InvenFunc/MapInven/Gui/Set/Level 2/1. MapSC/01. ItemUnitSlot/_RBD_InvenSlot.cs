using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class _RBD_InvenSlot
{
    internal static bool CheckUpAvailable(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, IResponedByDrop _dst)
    {
        Debug.Log("Check can Interact");
        if ((_dst as SlotGUI_EquipSlot == true))
        {
            SlotGUI_EquipSlot _dst2 = _dst as SlotGUI_EquipSlot;
            if (_src._itemGUI != null)
            {
                if (_dst2.CompareItem_withStat(_src._itemGUI._myData))
                    return true;
                else
                    return false;
            }
        }

        if ((_src as SlotGUI_EquipSlot == true) && _dst as SlotGUI_InvenSlot)
        {
            SlotGUI_EquipSlot _src2 = _src as SlotGUI_EquipSlot;
            SlotGUI_InvenSlot _dst2 = _dst as SlotGUI_InvenSlot;

            if(_dst2._itemGUI == null) return true;

            if (_src2.CompareItem_withStat(_dst2._itemGUI._myData))
                return true;
            else
                return false;
        }

        if ((_dst as RBD_IngridimentSlot == true))
        {
            RBD_IngridimentSlot _dst2 = _dst as RBD_IngridimentSlot;

            if (_src._itemGUI._myData.itemData[0] == 0)
                return true;
            else
                return false;
        }

        if ((_dst as RBD_UseDisposable == true))
        {
            return _src._itemGUI._myData.itemData[0] == 0;
        }

        return true;
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src)
    {
        Debug.Log("Not Allocated Case");
        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_InvenSlot _dst)
    {
        if ((_inven as RDM_ShopSC == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst); return;
        }

        if ((_inven as RDM_MapSC == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst); return;
        }

        if ((_inven as RDM_CampSC == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst); return;
        }

        if ((_inven as RDM_Inven_Info == true))
        {
            _inven.ItemToItem_EventDDO(_src, _dst); return;
        }

        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_EquipSlot _dst)
    {
        if ((_inven as RDM_ShopSC == true))
        {
            Debug.Log("RDM_ShopSC / SlotGUI_MapInventory -> SlotGUI_MapEquip");
            _inven.ItemToItem_EventDDO(_src, _dst); return;
        }

        if ((_inven as RDM_MapSC == true))
        {
            Debug.Log("RDM_MapSC / SlotGUI_MapInventory -> SlotGUI_MapEquip");
            _inven.ItemToItem_EventDDO(_src, _dst); return;
        }

        Debug.Log("Not Allocated Case");    
        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_CasherZone _dst)
    {
        _dst.DDO_Event_byInvenSlot(_src);
        _src._itemGUI.SetSizeAuto();
        return;
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_SellZone _dst)
    {
        _dst.DDO_Event_byInvenSlot(_src);
        return;
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_IngridimentSlot _dst)
    {
        Debug.Log("Moment 1");
        _dst.DDO_Event_byInvenSlot(_src);
        return;
    }
}
