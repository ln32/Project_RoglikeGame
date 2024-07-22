public static class _SlotGUI_CookResult
{
    internal static bool CheckUpAvailable(this iRoot_DDO_Manager _inven, SlotGUI_CookResult _src, IResponedByDrop _dst)
    {
        if (_inven as RDM_Info_CampCook == true)
        {
            return true;
        }

        return true;
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_CookResult _src, SlotGUI_InvenSlot _dst)
    {
        _inven.GetInvenSGT().AddItemUnit_byPurchase(_src._itemGUI._myData);

        GUI_ItemUnit _srcGUI = _src._itemGUI;
        _dst.SetGUI_byItemGUI(_srcGUI);
        _src.SetGUI_byItemGUI(null);
        return;
    }
}
