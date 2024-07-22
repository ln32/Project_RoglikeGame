using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class _RBD_InvenSlot
{
    internal static bool CheckUpAvailable(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, IResponedByDrop _dst)
    {
        if (_dst is SlotGUI_EquipSlot _dst2)
        {
            return _src._itemGUI != null && _dst2.CompareItem_withStat(_src._itemGUI._myData);
        }

        if (_src is SlotGUI_EquipSlot _src2 && _dst is SlotGUI_InvenSlot _dst2Inven)
        {
            return _dst2Inven._itemGUI == null || _src2.CompareItem_withStat(_dst2Inven._itemGUI._myData);
        }

        if (_dst is RBD_IngridimentSlot _dstIngridiment)
        {
            return _src._itemGUI._myData.itemData[0] == 0;
        }

        if (_dst is RBD_UseDisposable)
        {
            return _src._itemGUI._myData.itemData[0] == 0;
        }

        return true;
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src)
    {
        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_InvenSlot _dst)
    {
        if (_inven is RDM_ShopSC || _inven is RDM_MapSC || _inven is RDM_CampSC || _inven is RDM_Inven_Info)
        {
            _inven.ItemToItem_EventDDO(_src, _dst);
        }
        else
        {
            _src._itemGUI.SetSizeAuto();
        }
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, SlotGUI_EquipSlot _dst)
    {
        if (_inven is RDM_ShopSC || _inven is RDM_MapSC)
        {
            _inven.ItemToItem_EventDDO(_src, _dst);
        }
        else
        {
            _src._itemGUI.SetSizeAuto();
        }
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_CasherZone _dst)
    {
        _dst.DDO_Event_byInvenSlot(_src);
        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_SellZone _dst)
    {
        _dst.DDO_Event_byInvenSlot(_src);
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_InvenSlot _src, RBD_IngridimentSlot _dst)
    {
        _dst.DDO_Event_byInvenSlot(_src);
    }
}
