using UnityEngine;
internal static class GUI_ShopGoods_RBD
{
    internal static bool CheckUpAvailable(this iRoot_DDO_Manager _inven, SlotGUI_ShopGoods _src, IResponedByDrop _dst)
    {
        if (_inven as RDM_ShopSC == false)
        {
            return false;
        }

        RDM_ShopSC _RDM_ShopSC = _inven as RDM_ShopSC;

        if ((_dst as RBD_CasherZone == true))
        {
            return true;
        }

        if (_RDM_ShopSC.IsCanBuy_compGold(_src) == false)
        {
            _RDM_ShopSC.SetEffect_TradeDisable(_src);
            return false;
        }
        else
            _RDM_ShopSC.SetEffect_TradeAble(_src);

        if ((_dst as SlotGUI_InvenSlot == true))
        {
            return (_dst as SlotGUI_InvenSlot)._itemGUI == null;
        }

        return false;
    }
    

    internal static void InteractFuncByRBD(this iRoot_DDO_Manager _inven, SlotGUI_ShopGoods _src, SlotGUI_InvenSlot _dst)
    {
        if ((_inven as RDM_ShopSC == true))
        {
            RDM_ShopSC _RDM_ShopSC = _inven as RDM_ShopSC;
            if (_RDM_ShopSC.BuyItem_byDragDrop(_src, _dst))
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
        return;
    }
}