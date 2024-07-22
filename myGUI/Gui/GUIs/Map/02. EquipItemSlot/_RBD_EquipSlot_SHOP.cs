public static class _RBD_EquipSlot_SHOP
{
    internal static bool CheckUpScale(this RDM_ShopSC _inven, SlotGUI_EquipSlot _src, IResponedByDrop _dst)
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

    internal static void InteractFuncByRBD(this RDM_ShopSC _inven, SlotGUI_EquipSlot _src)
    {
        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this RDM_ShopSC _inven, SlotGUI_EquipSlot _src, SlotGUI_EquipSlot _dst)
    {
        GUI_ItemUnit _srcGUI = _src._itemGUI;
        GUI_ItemUnit _dstGUI = _dst._itemGUI;
        _src.SetGUI_byItemGUI(_dstGUI);
        _dst.SetGUI_byItemGUI(_srcGUI);
    }
}
