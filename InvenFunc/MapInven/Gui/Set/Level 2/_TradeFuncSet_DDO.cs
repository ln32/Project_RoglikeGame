using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class _TradeFuncSet_DDO
{
    internal static void SetEffect_TradeAble(this RDM_ShopSC _inven, SlotGUI_ShopGoods _src)
    {
        _inven._REF.Inven_M.GoldEffect.SetImg_TradeAble();
        return;
    }

    internal static void SetEffect_TradeDisable(this RDM_ShopSC _inven, SlotGUI_ShopGoods _src)
    {
        _inven._REF.Inven_M.GoldEffect.SetImg_TradeDisable();
        return;
    }
    internal static void SetEffect_TradeDefault(this RDM_ShopSC _inven)
    {
        _inven._REF.Inven_M.GoldEffect.SetImg_TradeDefault();
        return;
    }

    internal static void Check_DragStart(this RDM_ShopSC _inven, IDragDropObj _src)
    {
        if (_src as SlotGUI_ShopGoods == false)
        {
            return;
        }

        SlotGUI_ShopGoods _ShopGoods = _src as SlotGUI_ShopGoods;
        _inven.ShowGoldPrice(_ShopGoods._itemGUI._myData.GoldValue);

        return;
    }

    internal static void Check_DragEnd(this RDM_ShopSC _inven, IDragDropObj _src)
    {
        if (_src as SlotGUI_ShopGoods == false)
        {
            return;
        }

        SlotGUI_ShopGoods _ShopGoods = _src as SlotGUI_ShopGoods;
        _inven.CloseGoldPrice(_ShopGoods._itemGUI._myData.GoldValue);

        return;
    }

    internal static bool BuyItem_byDragDrop(this RDM_ShopSC _inven, SlotGUI_ShopGoods _src, SlotGUI_InvenSlot _dst)
    {
        // no gold
        if (_inven.IsCanBuy_compGold(_src) == false)
        {
            return false;
        }

        _inven.PurchaseItemEffect(_src._itemGUI._myData.GoldValue);

        _inven.GetInvenSGT().AddItemUnit_byPurchase(_src._itemGUI._myData);

        GUI_ItemUnit _srcGUI = _src._itemGUI;
        _dst.SetGUI_byItemGUI(_srcGUI);
        _src.SetGUI_byItemGUI(null);

        return true;
    }

    internal static void Sell_Item_byBtnClick(this RDM_ShopSC _inven, SlotGUI_InvenSlot _src)
    {
        // Sell Event
        ItemUnit tempItem = _src._itemGUI._myData;

        _inven._REF.GoldEFF.GoldEffect_byTrans(_src.transform, _inven._REF.Inven_M.GoldEffect.GetGoldTextTransform());
        _inven.GainGoldEffect(tempItem.GoldValue);
        _inven.GetInvenSGT().RemoveItem_byItem(_src._itemGUI._myData);
        _src.SetItemData_byData(null);

        return;
    }

    internal static bool IsCanBuy_compGold(this RDM_ShopSC _inven, SlotGUI_ShopGoods _item)
    {
        return _inven._REF.InvenSC.invenData_SGT.GetGold() > _item._itemGUI._myData.GoldValue;
    }
}