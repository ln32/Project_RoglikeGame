internal static class GUI_ShopGoods_RBD
{
    internal static bool CheckUpAvailable(this iRoot_DDO_Manager _inven, SlotGUI_ShopGoods _src, IResponedByDrop _dst)
    {
        if (!(_inven is RDM_ShopSC shopSC)) return false;

        if (_dst is RBD_CasherZone) return true;

        if (!shopSC.IsCanBuy_compGold(_src))
        {
            shopSC.SetEffect_TradeDisable(_src);
            return false;
        }

        shopSC.SetEffect_TradeAble(_src);
        return _dst is SlotGUI_InvenSlot invenSlot && invenSlot._itemGUI == null;
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_ShopGoods _src, SlotGUI_InvenSlot _dst)
    {
        if (_inven is RDM_ShopSC shopSC && shopSC.BuyItem_byDragDrop(_src, _dst))
        {
            return;
        }

        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_ShopGoods _src, SlotGUI_EquipSlot _dst)
    {
        _src._itemGUI.SetSizeAuto();
    }

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_ShopGoods _src, RBD_CasherZone _dst)
    {
        _dst.SetItemInfo_ShopGoods(_src);
        _src._itemGUI.SetSizeAuto();
    }
}
